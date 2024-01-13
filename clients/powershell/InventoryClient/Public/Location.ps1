function New-InventoryLocation
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
        $RegionCode,

        [Parameter(ParameterSetName="Default", Position=2, Mandatory=$true, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $CountryCode,

        [Parameter(ParameterSetName="Default", Position=3, Mandatory=$true, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $CityCode,

        [Parameter(ParameterSetName="Default", Position=4, Mandatory=$true, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $InventoryCode,

        [Parameter(ParameterSetName="Default", Position=5, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $Description = "",

        [Parameter(ParameterSetName="Default", Position=6, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [bool]
        $Deprecated = $false,

        [Parameter(ParameterSetName="Default", Position=7, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [DateTime]
        $ValidFrom,

        [Parameter(ParameterSetName="Default", Position=8, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
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
            $RegionCode    = $InputObject.RegionCode
            $CountryCode   = $InputObject.CountryCode
            $CityCode      = $InputObject.CityCode
            $InventoryCode = $InputObject.InventoryCode
            $Description   = $InputObject.Description
            $Deprecated    = $InputObject.Deprecated
            if ($null -ne $InputObject.ValidFrom) { $ValidFrom = $InputObject.ValidFrom }
            if ($null -ne $InputObject.ValidTo)   { $ValidTo   = $InputObject.ValidTo }

        }

        # Display input properties
        Trace-Message -Message ("Location Name : '{0}'" -f $Name) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Location RegionCode : '{0}'" -f $RegionCode) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Location CountryCode : '{0}'" -f $CountryCode) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Location CityCode : '{0}'" -f $CityCode) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Location InventoryCode : '{0}'" -f $InventoryCode) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Location Description : '{0}'" -f $Description) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Location Deprecated : '{0}'" -f $Deprecated) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Location ValidFrom : '{0}'" -f $ValidFrom) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Location ValidTo : '{0}'" -f $ValidTo) -CommandName $MyInvocation.MyCommand.Name

        # Assert Mandatory variables
        if ([string]::IsNullOrWhiteSpace($Name)) {throw "Name is mandatory"}
        if ([string]::IsNullOrWhiteSpace($RegionCode)) {throw "RegionCode is mandatory"}
        if ([string]::IsNullOrWhiteSpace($CountryCode)) {throw "CountryCode is mandatory"}
        if ([string]::IsNullOrWhiteSpace($CityCode)) {throw "CityCode is mandatory"}
        if ([string]::IsNullOrWhiteSpace($InventoryCode)) {throw "InventoryCode is mandatory"}

        # Create payload input
        $graphqlInput = @{
            name = $Name
            regionCode = $RegionCode
            cityCode = $CityCode
            countryCode = $CountryCode
            inventoryCode = $InventoryCode

        }
        if (![string]::IsNullOrWhiteSpace($Description)) {$graphqlInput[ "description"] = $Description}
        if ($null -ne $Deprecated ) {$graphqlInput[ "deprecated"] = $Deprecated}
        if ($null -ne $ValidFrom ) {$graphqlInput[ "validFrom"] = $ValidFrom}
        if ($null -ne $ValidTo ) {$graphqlInput[ "ValidTo"] = $ValidTo}

        $Variables = @{
            input = $graphqlInput
        }

        $command = $script:CreateLocationMutation + $script:LocationDtoFragment + $script:ErrorsFragment

        $result = Invoke-InternalGraphql -Query $command -Variables $Variables -uri $global:ConfigurationUri

        if ($result.createLocation.errors.Count -gt 0)
        {
            throw (ConvertFrom-InternalGraphqlErrors -Errors $result.createLocation.errors)
        }
        $result.createLocation.data

    }
}

function Update-InventoryLocation
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
        [string]
        $Name,

        [Parameter(ParameterSetName="Default", Position=2, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $InventoryCode,

        [Parameter(ParameterSetName="Default", Position=3, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $Description = "",

        [Parameter(ParameterSetName="Default", Position=4, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [bool]
        $Deprecated,

        [Parameter(ParameterSetName="Default", Position=5, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [DateTime]
        $ValidFrom,

        [Parameter(ParameterSetName="Default", Position=6, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
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
            $Id            = $InputObject.Id
            $InventoryCode = $InputObject.InventoryCode
            $Description   = $InputObject.Description
            $Deprecated    = $InputObject.Deprecated
            if ($null -ne $InputObject.ValidFrom) { $ValidFrom = $InputObject.ValidFrom }
            if ($null -ne $InputObject.ValidTo)   { $ValidTo   = $InputObject.ValidTo }

        }

        # Display input properties
        Trace-Message -Message ("Location Name          : '{0}'" -f $Name) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Location Id            : '{0}'" -f $Id) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Location InventoryCode : '{0}'" -f $InventoryCode) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Location Description   : '{0}'" -f $Description) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Location Deprecated    : '{0}'" -f $Deprecated) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Location ValidFrom     : '{0}'" -f $ValidFrom) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Location ValidTo       : '{0}'" -f $ValidTo) -CommandName $MyInvocation.MyCommand.Name

        # Assert Mandatory variables
        if ([string]::IsNullOrWhiteSpace($Name)) {throw "Name is mandatory"}
        if ($null -eq $Id) {throw "Id is mandatory"}

        # Create payload input
        $graphqlInput = @{
            id = $Id
        }
        if (![string]::IsNullOrWhiteSpace($InventoryCode)) {$graphqlInput[ "inventoryCode"] = $InventoryCode}
        if (![string]::IsNullOrWhiteSpace($Description)) {$graphqlInput[ "description"] = $Description}
        if ($null -ne $Deprecated ) {$graphqlInput[ "deprecated"] = $Deprecated}
        if ($null -ne $ValidFrom ) {$graphqlInput[ "validFrom"] = $ValidFrom}
        if ($null -ne $ValidTo ) {$graphqlInput[ "ValidTo"] = $ValidTo}

        $Variables = @{
            input = $graphqlInput
        }

        $command = $script:UpdateLocationMutation + $script:LocationDtoFragment + $script:ErrorsFragment

        $result = Invoke-InternalGraphql -Query $command -Variables $Variables -uri $global:ConfigurationUri

        if ($result.updateLocation.errors.Count -gt 0)
        {
            throw (ConvertFrom-InternalGraphqlErrors -Errors $result.updateLocation.errors)
        }
        $result.updateLocation.data

    }
}

function Remove-InventoryLocation
{
    [CmdletBinding(DefaultParameterSetName="Default")]
    param(
        [Parameter(ParameterSetName="Default", Position=0, Mandatory=$true, ValueFromPipeline=$true, ValueFromPipelineByPropertyName=$false)]
        [int]
        $Id
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

        Trace-Message -Message ("Location Id            : '{0}'" -f $Id) -CommandName $MyInvocation.MyCommand.Name
        if ($null -eq $Id) {throw "Id is mandatory"}

        # Create payload input
        $graphqlInput = @{
            id = $Id
        }
        $Variables = @{
            input = $graphqlInput
        }

        $command = $script:RemoveLocationMutation + $script:LocationDtoFragment + $script:ErrorsFragment

        $result = Invoke-InternalGraphql -Query $command -Variables $Variables -uri $global:ConfigurationUri

        if ($result.removeLocation.errors.Count -gt 0)
        {
            throw (ConvertFrom-InternalGraphqlErrors -Errors $result.removeLocation.errors)
        }
        $result.removeLocation.data


    }
}


function Get-InventoryLocation
{
    [CmdletBinding(DefaultParameterSetName="Default")]
    param(
        [Parameter(ParameterSetName="ById", Position=0, Mandatory=$true, ValueFromPipeline=$true, ValueFromPipelineByPropertyName=$false)]
        [int]
        $Id,

        [Parameter(ParameterSetName="Default", Position=0, Mandatory=$true, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$true)]
        [String]
        $Name
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
        if ($PsCmdlet.ParameterSetName -eq "ById")
        {
            Trace-Message -Message ("Get Location By Id : '{0}'" -f $Id) -CommandName $MyInvocation.MyCommand.Name
            $Variables[ "id" ] = $Id
            $command = $script:GetLocationByIdQuery + $script:LocationDtoFragment

            $result = Invoke-InternalGraphql -Query $command -Variables $Variables -uri $global:ConfigurationUri

            if ($result.location.errors.Count -gt 0)
            {
                throw (ConvertFrom-InternalGraphqlErrors -Errors $result.location.errors)
            }
            $result.location.data

        }
        else
        {
            Trace-Message -Message ("Get Location By Name : '{0}'" -f $Name) -CommandName $MyInvocation.MyCommand.Name
            $Variables[ "name" ] = $Name

            $command = $script:GetLocationByNameQuery + $script:LocationDtoFragment

            $result = Invoke-InternalGraphql -Query $command -Variables $Variables -uri $global:ConfigurationUri

            if ($result.locationByName.errors.Count -gt 0)
            {
                throw (ConvertFrom-InternalGraphqlErrors -Errors $result.locationByName.errors)
            }
            $result.locationByName.data

        }


    }
}

function Find-InventoryLocation
{
    [CmdletBinding(DefaultParameterSetName="Default")]
    param(
        [Parameter(Position=0, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$false)]
        [String]
        $RegionCode,

        [Parameter(Position=1, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$false)]
        [String]
        $CountryCode,

        [Parameter(Position=2, Mandatory=$false, ValueFromPipeline=$false, ValueFromPipelineByPropertyName=$false)]
        [String]
        $CityCode,

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

        Trace-Message -Message ("Location Region code        : {0}" -f $RegionCode) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Location Country code       : {0}" -f $CountryCode) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Location City code          : {0}" -f $CityCode) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Location ValidFrom          : {0}" -f $ValidFrom) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Location ValidTo            : {0}" -f $ValidTo) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Location Include deprecated : {0}" -f $IncludeDeprecated) -CommandName $MyInvocation.MyCommand.Name
        Trace-Message -Message ("Location Include All        : {0}" -f $IncludeAllEntities) -CommandName $MyInvocation.MyCommand.Name

        # Query
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


        $Query = $script:FindLocationQuery + $script:PaginationInfo +$script:LocationDtoFragment
        # $Query = $script:PluginQuery + $script:pluginDtoFragment
        $result = Invoke-InternalGraphql -Query $Query -Variables $Variables -uri $global:ConfigurationUri

        $pageInfo = $result.locations.pageInfo
        $pageInfo | Add-Member -MemberType NoteProperty -Name totalCount -Value $result.locations.totalCount
        return $pageInfo, $result.locations.edges

    }
}
