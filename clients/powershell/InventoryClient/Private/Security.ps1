$script:TrustAllCertsPolicyType = @"
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

function Get-InternalJwtToken
{
    [CmdletBinding()]
    param()

    Begin
    {

    }
    End
    {

    }
    Process
    {

    }
}

function Set-InternalUnsecureSSL
{
    # Ignore SSL errors
    if ($PSVersionTable.PSEdition -eq 'Core') {
        Trace-Message -Message "Disable SSL check for powershell core" -CommandName $MyInvocation.MyCommand.Name
        $Script:PSDefaultParameterValues = @{
            "invoke-restmethod:SkipCertificateCheck" = $true
            "invoke-webrequest:SkipCertificateCheck" = $true
        }
    } else {
        Trace-Message -Message "Disable SSL check for powershell" -CommandName $MyInvocation.MyCommand.Name
        try
        {
            add-type $script:TrustAllCertsPolicyType

        [System.Net.ServicePointManager]::CertificatePolicy = New-Object TrustAllCertsPolicy

    }
    catch {}


    }

}