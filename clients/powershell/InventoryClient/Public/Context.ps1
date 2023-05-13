
function Set-InventoryContext
{
    [CmdletBinding()]
    param(
        [System.Uri]
        $ConfigurationUri,

        [Switch]
        $DisableSSL

    )

    Begin
    {

    }
    End
    {

    }
    Process
    {
        Trace-Message -Message ("Configuration Uri => {0}" -f $ConfigurationUri) -CommandName $MyInvocation.MyCommand.Name
        $global:ConfigurationUri = $ConfigurationUri

        if ($DisableSSL)
        {
            Set-InternalUnsecureSSL
        }

        Get-InventoryContext
    }
}

function Get-InventoryContext
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
        write-Host ("Configuration Uri : {0}" -f $global:ConfigurationUri)

    }
}