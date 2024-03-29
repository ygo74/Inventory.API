function New-InventoryDatacenter
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
        [ValidateSet("Cloud","On_Premise")]
        $Type,

        [Parameter(ParameterSetName="Default", Position=3, Mandatory=$true, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $InventoryCode,

        [Parameter(ParameterSetName="Default", Position=4, Mandatory=$true, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $RegionCode,

        [Parameter(ParameterSetName="Default", Position=5, Mandatory=$true, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $CountryCode,

        [Parameter(ParameterSetName="Default", Position=6, Mandatory=$true, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $CityCode,

        [Parameter(ParameterSetName="Default", Position=7, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $Description,

        [Parameter(ParameterSetName="Default", Position=8, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [bool]
        $Deprecated = $false,

        [Parameter(ParameterSetName="Default", Position=9, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [DateTime]
        $ValidFrom,

        [Parameter(ParameterSetName="Default", Position=10, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [DateTime]
        $ValidTo
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
            $Type          = $InputObject.Type
            $InventoryCode = $InputObject.InventoryCode
            $RegionCode    = $InputObject.RegionCode
            $CountryCode   = $InputObject.CountryCode
            $CityCode      = $InputObject.CityCode
            $Deprecated    = $InputObject.Deprecated
            if (![string]::IsNullOrWhiteSpace($Description)) { $Description = $InputObject.Description }
            if ($null -ne $InputObject.ValidFrom) { $ValidFrom = $InputObject.ValidFrom }
            if ($null -ne $InputObject.ValidTo)   { $ValidTo   = $InputObject.ValidTo }

        }

        # Display input properties
        Trace-Message -Message ("Datacenter Name : '{0}'" -f $Name) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Datacenter Code : '{0}'" -f $Code) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Datacenter Type : '{0}'" -f $Type) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Datacenter RegionCode : '{0}'" -f $RegionCode) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Datacenter CountryCode : '{0}'" -f $CountryCode) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Datacenter CityCode : '{0}'" -f $CityCode) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Datacenter Description : '{0}'" -f $Description) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Datacenter InventoryCode : '{0}'" -f $InventoryCode) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Datacenter Deprecated : '{0}'" -f $Deprecated) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Datacenter ValidFrom : '{0}'" -f $ValidFrom) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Datacenter ValidTo : '{0}'" -f $ValidTo) -CommandName $MyInvocation.MyCommand.Name

        # Assert Mandatory variables
        if ([string]::IsNullOrWhiteSpace($Name)) {throw "Name is mandatory"}
        if ([string]::IsNullOrWhiteSpace($Code)) {throw "Code is mandatory"}
        if ([string]::IsNullOrWhiteSpace($InventoryCode)) {throw "InventoryCode is mandatory"}
        if ([string]::IsNullOrWhiteSpace($Type)) {throw "Type is mandatory"}
        if ([string]::IsNullOrWhiteSpace($RegionCode)) {throw "RegionCode is mandatory"}
        if ([string]::IsNullOrWhiteSpace($CountryCode)) {throw "CountryCode is mandatory"}
        if ([string]::IsNullOrWhiteSpace($CityCode)) {throw "CityCode is mandatory"}


        # Create payload input
        $graphqlInput = @{
            name = $Name
            code = $Code
            inventoryCode = $InventoryCode
            datacenterType = $Type.ToUpper()
            regionCode = $RegionCode
            countryCode = $CountryCode
            cityCode = $CityCode
        }
        if (![string]::IsNullOrWhiteSpace($Description)) {$graphqlInput[ "description"] = $Description}
        if ($null -ne $Deprecated ) {$graphqlInput[ "deprecated"] = $Deprecated}
        if ($null -ne $ValidFrom ) {$graphqlInput[ "validFrom"] = $ValidFrom}
        if ($null -ne $ValidTo ) {$graphqlInput[ "ValidTo"] = $ValidTo}

        $Variables = @{
            input = $graphqlInput
        }

        $command = $script:CreateDatacenterMutation + $script:DatacenterDtoFragment + $script:ErrorsFragment

        $result = Invoke-InternalGraphql -Query $command -Variables $Variables -uri $global:ConfigurationUri

        if ($result.createDatacenter.errors.Count -gt 0)
        {
            throw (ConvertFrom-InternalGraphqlErrors -Errors $result.createDatacenter.errors)
        }
        $result.createDatacenter.data

    }
}

function Update-InventoryDatacenter
{
    [CmdletBinding(DefaultParameterSetName="Default")]
    param(
        [Parameter(ParameterSetName="Pipeline", Position=0, Mandatory=$true, ValueFromPipeline=$true, ValueFromPipelineByPropertyName=$false)]
        [PsObject]
        $InputObject,

        [Parameter(ParameterSetName="Default", Position=0, Mandatory=$true, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [Parameter(ParameterSetName="UpdateLocation", Position=0, Mandatory=$true, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [int]
        $Id,

        [Parameter(ParameterSetName="Default", Position=1, Mandatory=$true, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [Parameter(ParameterSetName="UpdateLocation", Position=1, Mandatory=$true, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $Name,

        [Parameter(ParameterSetName="Default", Position=2, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [Parameter(ParameterSetName="UpdateLocation", Position=2, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $Code,

        [Parameter(ParameterSetName="Default", Position=3, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [Parameter(ParameterSetName="UpdateLocation", Position=3, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        [ValidateSet("Cloud","On_Premise")]
        $Type,

        [Parameter(ParameterSetName="Default", Position=4, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [Parameter(ParameterSetName="UpdateLocation", Position=4, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $InventoryCode,

        [Parameter(ParameterSetName="UpdateLocation", Position=5, Mandatory=$true, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $RegionCode,

        [Parameter(ParameterSetName="UpdateLocation", Position=6, Mandatory=$true, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $CountryCode,

        [Parameter(ParameterSetName="UpdateLocation", Position=7, Mandatory=$true, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $CityCode,

        [Parameter(ParameterSetName="Default", Position=5, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [Parameter(ParameterSetName="UpdateLocation", Position=8, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $Description,

        [Parameter(ParameterSetName="Default", Position=6, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [Parameter(ParameterSetName="UpdateLocation", Position=9, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [bool]
        $Deprecated = $false,

        [Parameter(ParameterSetName="Default", Position=7, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [Parameter(ParameterSetName="UpdateLocation", Position=10, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [DateTime]
        $ValidFrom,

        [Parameter(ParameterSetName="Default", Position=8, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [Parameter(ParameterSetName="UpdateLocation", Position=11, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [DateTime]
        $ValidTo
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
            $Type          = $InputObject.Type
            $InventoryCode = $InputObject.InventoryCode
            $RegionCode    = $InputObject.RegionCode
            $CountryCode   = $InputObject.CountryCode
            $CityCode      = $InputObject.CityCode
            $Deprecated    = $InputObject.Deprecated
            if (![string]::IsNullOrWhiteSpace($Description)) { $Description = $InputObject.Description }
            if ($null -ne $InputObject.ValidFrom) { $ValidFrom = $InputObject.ValidFrom }
            if ($null -ne $InputObject.ValidTo)   { $ValidTo   = $InputObject.ValidTo }

        }

        # Display input properties
        Trace-Message -Message ("Datacenter Id : '{0}'" -f $Id) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Datacenter Name : '{0}'" -f $Name) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Datacenter Code : '{0}'" -f $Code) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Datacenter Type : '{0}'" -f $Type) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Datacenter RegionCode : '{0}'" -f $RegionCode) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Datacenter CountryCode : '{0}'" -f $CountryCode) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Datacenter CityCode : '{0}'" -f $CityCode) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Datacenter Description : '{0}'" -f $Description) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Datacenter InventoryCode : '{0}'" -f $InventoryCode) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Datacenter Deprecated : '{0}'" -f $Deprecated) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Datacenter ValidFrom : '{0}'" -f $ValidFrom) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Datacenter ValidTo : '{0}'" -f $ValidTo) -CommandName $MyInvocation.MyCommand.Name

        # Assert Mandatory variables
        if ([string]::IsNullOrWhiteSpace($Id)) {throw "Id is mandatory"}
        if ([string]::IsNullOrWhiteSpace($Name)) {throw "Name is mandatory"}


        # Create payload input
        $graphqlInput = @{
            id = $Id
            name = $Name
        }
        if (![string]::IsNullOrWhiteSpace($Code)) {$graphqlInput[ "code"] = $Code}
        if (![string]::IsNullOrWhiteSpace($Type)) {$graphqlInput[ "datacenterType"] = $Type.ToUpper()}
        if (![string]::IsNullOrWhiteSpace($RegionCode)) {$graphqlInput[ "regionCode"] = $RegionCode}
        if (![string]::IsNullOrWhiteSpace($CountryCode)) {$graphqlInput[ "countryCode"] = $CountryCode}
        if (![string]::IsNullOrWhiteSpace($CityCode)) {$graphqlInput[ "cityCode"] = $CityCode}
        if (![string]::IsNullOrWhiteSpace($InventoryCode)) {$graphqlInput[ "inventoryCode"] = $InventoryCode}
        if ($null -ne $Description) {$graphqlInput[ "description"] = $Description}
        if ($null -ne $Deprecated ) {$graphqlInput[ "deprecated"] = $Deprecated}
        if ($null -ne $ValidFrom ) {$graphqlInput[ "validFrom"] = $ValidFrom}
        if ($null -ne $ValidTo ) {$graphqlInput[ "ValidTo"] = $ValidTo}

        $Variables = @{
            input = $graphqlInput
        }

        $command = $script:UpdateDatacenterMutation + $script:DatacenterDtoFragment + $script:ErrorsFragment

        $result = Invoke-InternalGraphql -Query $command -Variables $Variables -uri $global:ConfigurationUri

        if ($result.updateDatacenter.errors.Count -gt 0)
        {
            throw (ConvertFrom-InternalGraphqlErrors -Errors $result.updateDatacenter.errors)
        }
        $result.updateDatacenter.data

    }
}

function Set-InventoryDatacenterPluginEndpoint
{
    [CmdletBinding(DefaultParameterSetName="Default")]
    param(
        [Parameter(ParameterSetName="Pipeline", Position=0, Mandatory=$true, ValueFromPipeline=$true, ValueFromPipelineByPropertyName=$false)]
        [PsObject]
        $InputObject,

        [Parameter(ParameterSetName="Default", Position=0, Mandatory=$true, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [string]
        $DatacenterCode,

        [Parameter(ParameterSetName="Default", Position=1, Mandatory=$true, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $CredentialName,

        [Parameter(ParameterSetName="Default", Position=2, Mandatory=$true, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $PluginCode,

        [Parameter(ParameterSetName="Default", Position=3, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [Switch]
        $Remove

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
            $DatacenterCode  = $InputObject.DatacenterCode
            $CredentialName  = $InputObject.CredentialName
            $PluginCode      = $InputObject.PluginCode
            $PipelineAction  = $InputObject.Action
        }

        # Manage action
        $Action = "ADD"
        if ($Remove -or $PipelineAction -eq "REMOVE") {$Action = "REMOVE"}

        # Display input properties
        Trace-Message -Message ("Datacenter Code : '{0}'" -f $DatacenterCode) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Plugin Code : '{0}'" -f $PluginCode) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Credential Name : '{0}'" -f $CredentialName) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Plugin endpoint action : '{0}'" -f $Action) -CommandName $MyInvocation.MyCommand.Name

        # Assert Mandatory variables
        if ([string]::IsNullOrWhiteSpace($DatacenterCode)) {throw "DatacenterCode is mandatory"}
        if ([string]::IsNullOrWhiteSpace($PluginCode)) {throw "PluginCode is mandatory"}
        if ([string]::IsNullOrWhiteSpace($CredentialName)) {throw "CredentialName is mandatory"}


        # Create payload input
        $graphqlInput = @{
            datacenterCode = $DatacenterCode
            pluginCode = $PluginCode
            credentialName = $CredentialName
            action = $Action
        }

        $Variables = @{
            input = $graphqlInput
        }

        $command = $script:SetDatacenterPluginEndpointMutation + $script:DatacenterPluginsDtoFragment + $script:ErrorsFragment

        $result = Invoke-InternalGraphql -Query $command -Variables $Variables -uri $global:ConfigurationUri

        if ($result.setDataCenterPluginEnpoint.errors.Count -gt 0)
        {
            throw (ConvertFrom-InternalGraphqlErrors -Errors $result.setDataCenterPluginEnpoint.errors)
        }
        $result.setDataCenterPluginEnpoint.data

    }
}


function Get-InventoryDatacenter
{
    [CmdletBinding(DefaultParameterSetName="Default")]
    param(
        [Parameter(ParameterSetName="ById", Position=0, Mandatory=$true, ValueFromPipeline=$true, ValueFromPipelineByPropertyName=$false)]
        [int]
        $Id,

        [Parameter(ParameterSetName="Default", Position=0, Mandatory=$true, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $Name,

        [Parameter(ParameterSetName="ByCode", Position=0, Mandatory=$true, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
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
                Trace-Message -Message ("Get Datacenter By Id : '{0}'" -f $Id) -CommandName $MyInvocation.MyCommand.Name
                $Variables[ "id" ] = $Id
                $command = $script:GetDatacenterByIdQuery + $script:DatacenterDtoFragment + $script:ErrorsFragment
            }
            "ByCode"
            {
                Trace-Message -Message ("Get Datacenter By Id : '{0}'" -f $Id) -CommandName $MyInvocation.MyCommand.Name
                $Variables[ "code" ] = $Code
                $command = $script:GetDatacenterByCodeQuery + $script:DatacenterDtoFragment + $script:ErrorsFragment
            }
            "Default"
            {
                Trace-Message -Message ("Get Datacenter By Name : '{0}'" -f $Name) -CommandName $MyInvocation.MyCommand.Name
                $Variables[ "name" ] = $Name

                $command = $script:GetDatacenterByNameQuery + $script:DatacenterDtoFragment + $script:ErrorsFragment
            }
            default {
                throw "Invalid ParameterSetName"
            }
        }

        $result = Invoke-InternalGraphql -Query $command -Variables $Variables -uri $global:ConfigurationUri

        if ($result.datacenter.errors.Count -gt 0)
        {
            throw (ConvertFrom-InternalGraphqlErrors -Errors $result.datacenter.errors)
        }
        $result.datacenter.data

    }
}

function Find-InventoryDatacenter
{
    [CmdletBinding(DefaultParameterSetName="Default")]
    param(
        [Parameter(Position=0, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$false)]
        [String]
        $InventoryCode,

        [Parameter(Position=3, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$false)]
        [DateTime]
        $ValidFrom,

        [Parameter(Position=4, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$false)]
        [DateTime]
        $ValidTo,

        [Parameter(Position=5, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$false)]
        [switch]
        $IncludeAllEntities,

        [Parameter(Position=6, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$false)]
        [switch]
        $IncludeDeprecated,

        [Parameter(Position=7, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$false)]
        [Int]
        $Limit,

        [Parameter(ParameterSetName="next", Position=8, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$false)]
        [string]
        $After,

        [Parameter(ParameterSetName="previous", Position=8, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$false)]
        [string]
        $Before
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

        Trace-Message -Message ("Datacenter Inventory code   : {0}" -f $InventoryCode) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Location ValidFrom          : {0}" -f $ValidFrom) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Location ValidTo            : {0}" -f $ValidTo) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Location Include deprecated : {0}" -f $IncludeDeprecated) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Location Include All        : {0}" -f $IncludeAllEntities) -CommandName $MyInvocation.MyCommand.Name

        # Filter
        $query = @{}
        if (![string]::IsNullOrWhiteSpace($RegionCode)) {$query[ "regionCode"] = $RegionCode}
        if (![string]::IsNullOrWhiteSpace($CountryCode)) {$query[ "countryCode"] = $CountryCode}
        if (![string]::IsNullOrWhiteSpace($CityCode)) {$query[ "cityCode"] = $CityCode}
        if ($null -ne $ValidFrom) {$query[ "validFrom"] = $ValidFrom}
        if ($null -ne $ValidTo) {$query[ "validTo"] = $ValidTo}

        $query[ "includeDeprecated" ] = $IncludeDeprecated.IsPresent
        $query[ "allEntities" ] = $IncludeAllEntities.IsPresent

        $Variables = @{
            query = $query
        }

        # Pagination
        if ($PsCmdlet.ParameterSetName -ne "Default")
        {
            if ($PsCmdlet.ParameterSetName -eq "next")
            {
                Trace-Message -Message ("Forward pagination for {0} items, after '{1}'" -f $Limit, $after ) -CommandName $MyInvocation.MyCommand.Name
                if ($Limit -gt 0) { $Variables[ "first" ] = $Limit }
                if (![string]::IsNullOrWhiteSpace($After)) { $Variables[ "after" ] = $After }
            }
            else
            {
                Trace-Message -Message ("Backward pagination for {0} items, before '{1}'" -f $Limit, $Before ) -CommandName $MyInvocation.MyCommand.Name
                if ($Limit -gt 0) { $Variables[ "last" ] = $Limit }
                if (![string]::IsNullOrWhiteSpace($Before)) { $Variables[ "before" ] = $Before }
            }
        }
        else
        {
            if ($Limit -gt 0) { $Variables[ "first" ] = $Limit }
        }


        $Query = $script:FindDatacenterQuery + $script:PaginationInfo +$script:DatacenterDtoFragment
        # $Query = $script:PluginQuery + $script:pluginDtoFragment
        $result = Invoke-InternalGraphql -Query $Query -Variables $Variables -uri $global:ConfigurationUri

        $pageInfo = $result.datacenters.pageInfo
        $pageInfo | Add-Member -MemberType NoteProperty -Name totalCount -Value $result.datacenters.totalCount
        return $pageInfo, $result.datacenters.edges

    }
}
