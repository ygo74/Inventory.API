function New-InventoryPlugin
{
    [CmdletBinding()]
    param(
        [Parameter(ParameterSetName="Default", Position=0, Mandatory=$true, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $Name,

        [Parameter(ParameterSetName="Default", Position=1, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $Code,

        [Parameter(ParameterSetName="Default", Position=2, Mandatory=$true, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $Version,

        [Parameter(ParameterSetName="Default", Position=3, Mandatory=$true, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $Path


    )

    Begin
    {
        $startFunction = Get-Date
        Trace-StartFunction -CommandName $MyInvocation.MyCommand.Name
    }
    End
    {
        $endFunction = Get-Date
        Trace-EndFunction -CommandName $MyInvocation.MyCommand.Name -Duration ($endFunction -$startFunction)

    }
    Process
    {
        Trace-Message -Message ("Plugin Name : {0}" -f $Name) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Plugin Code : {0}" -f $Code) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Plugin Version : {0}" -f $Version) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Plugin Path : {0}" -f $Path) -CommandName $MyInvocation.MyCommand.Name

        $Variables = @{
            input = @{
                name = $Name
                code = $Code
                version = $Version
                path = $Path
            }
        }


        $result = Invoke-InternalGraphql -Query $script:CreatePluginMutation -Variables $Variables -uri $global:ConfigurationUri

        if ($result.createPlugin.errors.Count -gt 0)
        {
            throw (ConvertFrom-InternalGraphqlErrors -Errors $result.createPlugin.errors)
        }
        $result.createPlugin.plugin

    }
}


function Find-InventoryPlugin
{
    [CmdletBinding()]
    param(
        [Parameter(ParameterSetName="Default", Position=0, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [Int]
        $First = 10
    )

    Begin
    {
        $startFunction = Get-Date
        Trace-StartFunction -CommandName $MyInvocation.MyCommand.Name
    }
    End
    {
        $endFunction = Get-Date
        Trace-EndFunction -CommandName $MyInvocation.MyCommand.Name -Duration ($endFunction -$startFunction)

    }
    Process
    {
        Trace-Message -Message ("Plugin Name : {0}" -f $Name) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Plugin Code : {0}" -f $Code) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Plugin Version : {0}" -f $Version) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Plugin Path : {0}" -f $Path) -CommandName $MyInvocation.MyCommand.Name

        $Variables = @{
            first = $First
            order = @(
                @{name="ASC"}
            )
        }

        $Query = $script:PluginQuery + $script:PaginationInfo +$script:pluginDtoFragment
        $result = Invoke-InternalGraphql -Query $Query -Variables $Variables -uri $global:ConfigurationUri

        $result.plugins

    }
}

