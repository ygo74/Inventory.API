function New-InventoryLocation
{
    [CmdletBinding(DefaultParameterSetName="Default")]
    param(
        [Parameter(ParameterSetName="Pipeline", Position=0, Mandatory=$true, ValueFromPipeline=$true, ValueFromPipelineByPropertyName=$false)]
        [PsObject]
        $Input,

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
            $Name          = $Input.Name
            $RegionCode    = $Input.RegionCode
            $CountryCode   = $Input.CountryCode
            $CityCode      = $Input.CityCode
            $InventoryCode = $Input.InventoryCode
            $Description   = $Input.Description
            $Deprecated    = $Input.Deprecated
            $ValidFrom     = $Input.ValidFrom
            $ValidTo       = $Input.ValidTo

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

        $command = $script:CreateLocationMutation + $script:LocationDtoFragment

        $result = Invoke-InternalGraphql -Query $command -Variables $Variables -uri $global:ConfigurationUri
        $result

        if ($result.createLocation.errors.Count -gt 0)
        {
            throw (ConvertFrom-InternalGraphqlErrors -Errors $result.createLocation.errors)
        }
        $result.createLocation.data

    }
}
