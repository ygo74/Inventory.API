[CmdletBinding()]
param(
    [Parameter(Position=0,mandatory=$false)]
    [string]
    $DockerComposeExeFilePath="C:\Program Files\Docker\docker-compose_2.7.0.exe",

    [Parameter(Position=1,mandatory=$false)]
    [Switch]
    $ForceRecreateMonitoring,

    [Parameter(Position=1,mandatory=$false)]
    [Switch]
    $ForceRecreateCluster,

    [Parameter(Position=0,mandatory=$false)]
    [string]
    $ElasticUri="https://localhost:9200/",

    [Parameter(Position=0,mandatory=$false)]
    [string]
    $KibanaUri="https://localhost:5601/",

    [Parameter(Position=0,mandatory=$false)]
    [string]
    $FleetServerUri="https://localhost:8220/",

    [Parameter(Position=0,mandatory=$false)]
    [string]
    $ApmServerUri="http://localhost:8200/",

    [Parameter(Position=0,mandatory=$true)]
    [PsCredential]
    $Credential

)

# -----------------------------------------------------------------------------
# Functions
# -----------------------------------------------------------------------------
function Init-InternalByPassSSLCheck
{
    add-type @"
using System.Net;
using System.Security.Cryptography.X509Certificates;
public class TrustAllCertsPolicy : ICertificatePolicy {
    public bool CheckValidationResult(
        ServicePoint srvPoint, X509Certificate certificate,
        WebRequest request, int certificateProblem) {
        return true;
    }
}
"@
    [System.Net.ServicePointManager]::CertificatePolicy = New-Object TrustAllCertsPolicy
}

function Write-InternalStepHeader($message)
{
    Write-Host ""
    Write-Host $message -ForegroundColor Green
    Write-Host "".padRight(80,"=") -ForegroundColor Green
}

function Get-InternalBasicAuthorizationHeader($credential)
{
    $user = $Credential.UserName
    $pass = $Credential.GetNetworkCredential().Password
    $pair = "$($user):$($pass)"
    $encodedCreds = [System.Convert]::ToBase64String([System.Text.Encoding]::ASCII.GetBytes($pair))
    $basicAuthValue = "Basic $encodedCreds"
    $Headers = @{
        Authorization = $basicAuthValue
    }

    return $Headers
}

# Retrieve APM policy enrollment token and configure environment variables
function Set-InternalApmPolicyEnrollmentToken($credential, $kibanaUri)
{
    # Get Authorization Basic headers
    $Headers = Get-InternalBasicAuthorizationHeader -credential $credential

    # Get APM Policy Enrollment token
    $enrollmmentTokenApi = "{0}api/fleet/enrollment_api_keys" -f $kibanaUri
    $response = Invoke-RestMethod -Method Get -Uri $enrollmmentTokenApi -Headers $Headers -UseBasicParsing

    $enrollmentToken = $response.items | Where-Object {$_.policy_id -eq "apm-1-server"} | select-object -ExpandProperty api_key
    if ([string]::IsNullOrWhiteSpace($enrollmentToken))
    {
        throw "Unable to retrieve the enrollment token for policy apm-1-server"
    }

    # Update .env file with enrollment token
    $envFileContent = Get-Content -Path .\.env
    $envFileContent = $envFileContent -replace "^APM_ENROLLMENT_TOKEN=.*$", ("APM_ENROLLMENT_TOKEN={0}" -f $enrollmentToken)
    Set-Content -Path .\.env -Value $envFileContent

}

# Check Execution environment
function Assert-InternalExecutionEnvironment
{
    Write-InternalStepHeader -Message "Check Execution environment"
    $EnvironmentReady = $true

    $currentLocationPath = Get-Location | select-object -ExpandProperty Path
    Write-Host ("Current Location : {0}" -f $currentLocationPath)

    # Environment variable template file
    $envTemplatefilePath = Join-Path -Path $currentLocationPath -ChildPath "configuration/.env.template"
    Write-Host ("Environment variable template file : {0} => " -f $envTemplatefilePath) -NoNewLine
    try
    {
        Resolve-Path -Path $envTemplatefilePath | out-null
        write-Host "OK" -ForegroundColor Green
    }
    catch
    {
        write-Host "Missing file" -ForegroundColor Red
        $EnvironmentReady = $false
    }

    # Docker compose executable file
    Write-Host ("Docker compose executable file path : {0} => " -f $DockerComposeExeFilePath) -NoNewLine
    if (Test-path -Path $DockerComposeExeFilePath -PathType Leaf)
    {
        write-Host "OK" -ForegroundColor Green
    }
    else
    {
        write-Host "Missing file" -ForegroundColor Red
        $EnvironmentReady = $false
    }

    if (-not $EnvironmentReady)
    {
        throw "Environment execution is not ready"
    }

    # Docker compose file
    $dockerComposefilePath = Join-Path -Path $currentLocationPath -ChildPath "docker-compose.yml"
    Write-Host ("Docker compose file path : {0} => " -f $dockerComposefilePath) -NoNewLine
    try
    {
        Resolve-Path -Path $dockerComposefilePath | out-null
        write-Host "OK" -ForegroundColor Green
    }
    catch
    {
        write-Host "Missing file" -ForegroundColor Red
        $EnvironmentReady = $false
    }

    if (-not $EnvironmentReady)
    {
        throw "Environment execution is not ready"
    }

}

function Start-InternalDockerComposeServices
{
    [CmdletBinding()]
    param(
        [Parameter(Position=0,mandatory=$false)]
        [string]
        $DockerProfile="",

        [Parameter(Position=1,mandatory=$false)]
        [string]
        $Service="",

        [Parameter(Position=1,mandatory=$false)]
        [Switch]
        $ForceRecreate

    )
    Process
    {

        $dockerArguments = ""
        if (![string]::IsNullOrWhiteSpace($Profile))
        {
            $dockerArguments = "--profile {0}" -f $DockerProfile
        }

        $dockerArguments += " -f .\docker-compose.yml up"
        if (![string]::IsNullOrWhiteSpace($Service))
        {
            $dockerArguments += " {0}" -f $Service
        }

        $dockerArguments += " -d"

        if ($ForceRecreate)
        {
            $dockerArguments += " --force-recreate"
        }

        write-Host $dockerArguments
        . $DockerComposeExeFilePath ($dockerArguments.Split(" "))
    }
}

# -----------------------------------------------------------------------------
# Main
# -----------------------------------------------------------------------------
function Main
{
    $ErrorActionPreference="Stop"
    Init-InternalByPassSSLCheck

    # Check Execution environment
    Assert-InternalExecutionEnvironment

    # Check input
    if (-not $ElasticUri.EndsWith("/")) {$ElasticUri = "$($ElasticUri)/"}
    if (-not $KibanaUri.EndsWith("/")) {$KibanaUri = "$($KibanaUri)/"}
    if (-not $FleetServerUri.EndsWith("/")) {$FleetServerUri = "$($FleetServerUri)/"}
    if (-not $ApmServerUri.EndsWith("/")) {$ApmServerUri = "$($ApmServerUri)/"}

    # Assert platform status

    Write-InternalStepHeader -Message "Start Elastic Platform Step 1"
    Start-InternalDockerComposeServices

    Write-InternalStepHeader -Message "Retrieve APM Enrollment Token"
    Set-InternalApmPolicyEnrollmentToken -credential $Credential -kibanaUri $KibanaUri

    Write-InternalStepHeader -Message "Start Elastic Platform Step 2"
    if ($ForceRecreateMonitoring)
    {
        Start-InternalDockerComposeServices -DockerProfile monitoring -Service apm-server -ForceRecreate
        Start-InternalDockerComposeServices -DockerProfile monitoring -Service opentelemetry-collector -ForceRecreate
    }
    else
    {
        Start-InternalDockerComposeServices -DockerProfile monitoring
    }

}

Main

