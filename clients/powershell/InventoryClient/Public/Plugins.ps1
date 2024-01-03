function New-InventoryPlugin
{
    [CmdletBinding(DefaultParameterSetName="Default")]
    param(
        [Parameter(ParameterSetName="Pipeline", Position=0, Mandatory=$true, ValueFromPipeline=$true, ValueFromPipelineByPropertyName=$false)]
        [PsObject]
        $InputObject,

        [Parameter(ParameterSetName="Default", Position=0, Mandatory=$true, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $Name,

        [Parameter(ParameterSetName="Default", Position=1, Mandatory=$true, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $Code,

        [Parameter(ParameterSetName="Default", Position=2, Mandatory=$true, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $Version,

        [Parameter(ParameterSetName="Default", Position=3, Mandatory=$true, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $Path,

        [Parameter(ParameterSetName="Default", Position=4, Mandatory=$true, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $InventoryCode
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

        if ($PsCmdlet.ParameterSetName -eq "Pipeline")
        {
            $Name          = $InputObject.Name
            $Code          = $InputObject.Code
            $Version       = $InputObject.Version
            $Path          = $InputObject.Path
            $InventoryCode = $InputObject.InventoryCode
        }

        Trace-Message -Message ("Plugin Name : {0}" -f $Name) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Plugin Code : {0}" -f $Code) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Plugin Version : {0}" -f $Version) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Plugin Path : {0}" -f $Path) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Plugin Inventory code : {0}" -f $InventoryCode) -CommandName $MyInvocation.MyCommand.Name

        # Assert Mandatory variables
        if ([string]::IsNullOrWhiteSpace($Name)) {throw "Name is mandatory"}
        if ([string]::IsNullOrWhiteSpace($Code)) {throw "Code is mandatory"}
        if ([string]::IsNullOrWhiteSpace($Version)) {throw "Version is mandatory"}
        if ([string]::IsNullOrWhiteSpace($Path)) {throw "Path is mandatory"}
        if ([string]::IsNullOrWhiteSpace($InventoryCode)) {throw "InventoryCode is mandatory"}


        $Variables = @{
            input = @{
                name = $Name
                code = $Code
                version = $Version
                path = $Path
                inventoryCode = $InventoryCode
            }
        }


        $command = $script:CreatePluginMutation + $script:pluginDtoFragment + $script:ErrorsFragment
        $result = Invoke-InternalGraphql -Query $command -Variables $Variables -uri $global:ConfigurationUri

        if ($result.createPlugin.errors.Count -gt 0)
        {
            throw (ConvertFrom-InternalGraphqlErrors -Errors $result.createPlugin.errors)
        }
        $result.createPlugin.data

    }
}

function Update-InventoryPlugin
{
    [CmdletBinding(DefaultParameterSetName="Default")]
    param(
        [Parameter(ParameterSetName="Pipeline", Position=0, Mandatory=$true, ValueFromPipeline=$true, ValueFromPipelineByPropertyName=$false)]
        [PsObject]
        $InputObject,

        [Parameter(ParameterSetName="Default", Position=0, Mandatory=$true, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [int]
        $Id,

        [Parameter(ParameterSetName="Default", Position=1, Mandatory=$true, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $Name,

        [Parameter(ParameterSetName="Default", Position=2, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $Code,

        [Parameter(ParameterSetName="Default", Position=3, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $Version,

        [Parameter(ParameterSetName="Default", Position=4, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $Path,

        [Parameter(ParameterSetName="Default", Position=5, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $InventoryCode
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

        if ($PsCmdlet.ParameterSetName -eq "Pipeline")
        {
            $Id            = $InputObject.Id
            $Name          = $InputObject.Name
            $Code          = $InputObject.Code
            $Version       = $InputObject.Version
            $Path          = $InputObject.Path
            $InventoryCode = $InputObject.InventoryCode
        }

        Trace-Message -Message ("Plugin Name : {0}" -f $Name) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Plugin Code : {0}" -f $Code) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Plugin Version : {0}" -f $Version) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Plugin Path : {0}" -f $Path) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Plugin Inventory code : {0}" -f $InventoryCode) -CommandName $MyInvocation.MyCommand.Name

        # Assert Mandatory variables
        if ([string]::IsNullOrWhiteSpace($Id)) {throw "Id is mandatory"}
        if ([string]::IsNullOrWhiteSpace($Name)) {throw "Name is mandatory"}


        $Variables = @{
            input = @{
                id   = $id
                # name = $Name
                # code = $Code
                # version = $Version
                path = $Path
                # inventoryCode = $InventoryCode
            }
        }


        $command = $script:UpdatePluginMutation + $script:pluginDtoFragment + $script:ErrorsFragment
        $result = Invoke-InternalGraphql -Query $command -Variables $Variables -uri $global:ConfigurationUri

        if ($result.updatePlugin.errors.Count -gt 0)
        {
            throw (ConvertFrom-InternalGraphqlErrors -Errors $result.updatePlugin.errors)
        }
        $result.updatePlugin.data

    }
}


function Get-InventoryPlugin
{
    [CmdletBinding(DefaultParameterSetName="Default")]
    param(
        [Parameter(ParameterSetName="ById", Position=0, Mandatory=$true, ValueFromPipeline=$true, ValueFromPipelineByPropertyName=$false)]
        [int]
        $Id,

        [Parameter(ParameterSetName="Default", Position=0, Mandatory=$true, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $Code

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

        $Variables = @{}
        switch ($PsCmdlet.ParameterSetName)
        {
            "ById"
            {
                Trace-Message -Message ("Get Plugin By Id : '{0}'" -f $Id) -CommandName $MyInvocation.MyCommand.Name
                $Variables[ "id" ] = $Id
                $command = $script:GetPluginByIdQuery + $script:pluginDtoFragment + $script:ErrorsFragment
            }
            "Default"
            {
                Trace-Message -Message ("Get Plugin By Code : '{0}'" -f $Code) -CommandName $MyInvocation.MyCommand.Name
                $Variables[ "code" ] = $Code

                $command = $script:GetPluginByCodeQuery + $script:pluginDtoFragment + $script:ErrorsFragment
            }
            default {
                throw "Invalid ParameterSetName"
            }
        }

        $result = Invoke-InternalGraphql -Query $command -Variables $Variables -uri $global:ConfigurationUri

        if ($result.plugin.errors.Count -gt 0)
        {
            throw (ConvertFrom-InternalGraphqlErrors -Errors $result.plugin.errors)
        }
        $result.plugin.data

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
        }

        # $Variables = $null
        $Query = $script:PluginQuery + $script:PaginationInfo +$script:pluginDtoFragment
        # $Query = $script:PluginQuery + $script:pluginDtoFragment
        $result = Invoke-InternalGraphql -Query $Query -Variables $Variables -uri $global:ConfigurationUri

        $result

    }
}

