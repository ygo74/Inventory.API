[CmdletBinding()]
param(
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

# -----------------------------------------------------------------------------
# Elastic status
# -----------------------------------------------------------------------------
function Assert-InternalElasticStatus
{
    Write-InternalStepHeader -Message "Elastic cluster status"
    try
    {
        Write-Host "Get Cluster Info : " -NoNewLine
        $clusterInfo = Invoke-RestMethod -Uri $ElasticUri -UseBasicParsing
        write-Host "OK" -ForegroundColor Green
        $clusterInfo
    }
    catch {
        write-Host "KO" -ForegroundColor Red
        throw "Elastic cluster Info is not availbale"
    }

    try
    {
        Write-Host "Get Cluster Status : " -NoNewLine
        $clusterStatus = Invoke-RestMethod -Uri "$($ElasticUri)_cluster/health?wait_for_status=yellow&timeout=500ms" -UseBasicParsing
        write-Host "OK" -ForegroundColor Green
        $clusterStatus
    }
    catch {
        write-Host "KO" -ForegroundColor Red
        throw "Elastic cluster Status is not availbale"
    }
}

# -----------------------------------------------------------------------------
# Kibana
# -----------------------------------------------------------------------------
function Assert-InternalKibanaStatus
{

    Write-InternalStepHeader -Message "Kibana status"
    $user = $Credential.UserName
    $pass = $Credential.GetNetworkCredential().Password
    $pair = "$($user):$($pass)"
    $encodedCreds = [System.Convert]::ToBase64String([System.Text.Encoding]::ASCII.GetBytes($pair))
    $basicAuthValue = "Basic $encodedCreds"
    $Headers = @{
        Authorization = $basicAuthValue
    }

    try
    {
        Write-Host "Get Kibana Status : " -NoNewLine
        $kibanaStatus = Invoke-RestMethod -Uri "$($KibanaUri)api/status" -Headers $Headers -UseBasicParsing
        write-Host "OK" -ForegroundColor Green
        $kibanaStatus
    }
    catch {
        write-Host "KO" -ForegroundColor Red
        throw "Kibana Status is not availbale"
    }
}

# -----------------------------------------------------------------------------
# Fleet Server
# -----------------------------------------------------------------------------
function Assert-InternalFleetServerStatus
{
    Write-InternalStepHeader -Message "Fleet server status"
    try
    {
        Write-Host "Get Fleet server Status : " -NoNewLine
        $FleetStatus = Invoke-RestMethod -Uri "$($FleetServerUri)api/status" -UseBasicParsing
        write-Host "OK" -ForegroundColor Green
        $FleetStatus
    }
    catch {
        write-Host "KO" -ForegroundColor Red
        throw "Fleet server is not availbale"
    }
}

# -----------------------------------------------------------------------------
# Apm Server
# -----------------------------------------------------------------------------
function Assert-InternalApmServerStatus
{
    Write-InternalStepHeader -Message "APM Server status"
    try
    {
        Write-Host "Get APM server Status : " -NoNewLine
        $ApmStatus = Invoke-RestMethod -Uri "$($ApmServerUri)" -UseBasicParsing
        write-Host "OK" -ForegroundColor Green
        $ApmStatus
    }
    catch {
        write-Host "KO" -ForegroundColor Red
        throw "APM server is not availbale"
    }
}


# Main
function Main
{
    $ErrorActionPreference="Stop"
    Init-InternalByPassSSLCheck

    # Check input
    if (-not $ElasticUri.EndsWith("/")) {$ElasticUri = "$($ElasticUri)/"}
    if (-not $KibanaUri.EndsWith("/")) {$KibanaUri = "$($KibanaUri)/"}
    if (-not $FleetServerUri.EndsWith("/")) {$FleetServerUri = "$($FleetServerUri)/"}
    if (-not $ApmServerUri.EndsWith("/")) {$ApmServerUri = "$($ApmServerUri)/"}

    # Assert platform status
    Assert-InternalElasticStatus
    Assert-InternalKibanaStatus
    Assert-InternalFleetServerStatus
    Assert-InternalApmServerStatus
}

Main