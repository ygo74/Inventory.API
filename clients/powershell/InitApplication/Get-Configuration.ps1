
# -----------------------------------------------------------------------------
# PLugins
# -----------------------------------------------------------------------------
$pluginsDefinition = @(
    [Ordered]@{
        Name="Azure Plugin";
        Code="Azure.Plugin";
        Version="1.0";
        Path="D:\devel\github\ansible_inventory\Services\plugins\Azure\Inventory.Plugins.Azure\bin\Debug\net6.0\Inventory.Plugins.Azure.dll";
        InventoryCode="Azure.Plugin"
    }
    [Ordered]@{Name="Internal Plugin"; Code="Internal.Plugin"; Version="1.0"; Path="Null";InventoryCode="Internal.Plugin"}
    [Ordered]@{Name="Google Plugin"; Code="Google.Plugin"; Version="1.0"; Path="Null";InventoryCode="Google.Plugin"}
    [Ordered]@{Name="Amazon AWS Plugin"; Code="Aws.Plugin"; Version="1.0"; Path="Null";InventoryCode="Aws.Plugin"}
)

$plugins = @($pluginsDefinition | % {New-Object -TypeName psobject -Property $_})


# -----------------------------------------------------------------------------
# Locations
# -----------------------------------------------------------------------------
$locationsDefinition = @(
    [Ordered]@{
        Name = "Paris"
        Description = "Paris city"
        CountryCode = "FR"
        CityCode = "PAR"
        RegionCode = "EMEA"
        InventoryCode = "emea.fr.par"},
    [Ordered]@{
            Name = "London"
            Description = "London city"
            CountryCode = "UK"
            CityCode = "LDN"
            RegionCode = "EMEA"
            InventoryCode = "emea.uk.ldn"}

)

$locations = @($locationsDefinition | % {New-Object -TypeName psobject -Property $_})

# -----------------------------------------------------------------------------
# Datacenters
# -----------------------------------------------------------------------------
$datacentersDefinition = @(
    [Ordered]@{
        Name = "Azure France Central"
        Description = "Azure France Central Datacenter"
        Code = "AZR-FR-CENTRAL"
        Type = "Cloud"
        InventoryCode = "azure.fr"
        CountryCode = "FR"
        CityCode = "PAR"
        RegionCode = "EMEA"
    }
)

$datacenters = @($datacentersDefinition | % {New-Object -TypeName psobject -Property $_})

# -----------------------------------------------------------------------------
# Credentials
# -----------------------------------------------------------------------------
$propertyBag = @{}
Get-Content -Path $(Join-Path -Path $env:USERPROFILE -ChildPath "azure_credentials") | Foreach-Object {
    $line = $_
    $lineValues = $line.split("=", [StringSplitOptions]::RemoveEmptyEntries)
    $keyAzureCredential = $lineValues[0]

    if (($null -ne $keyAzureCredential) -and ($keyAzureCredential.startsWith("AZURE")))
    {
        $propertyBag[ $keyAzureCredential.ToLower() ] = $lineValues[1].Trim()
    }
}

$CredentialDefinitions = @(
    [Ordered]@{
        Name = "Azure Credential"
        Description = "Default credential for my azure subscription"
        PropertyBag = $(New-Object -TypeName psobject -Property $propertyBag)
    }
)

$crdentials = @($CredentialDefinitions | % {New-Object -TypeName psobject -Property $_})

# -----------------------------------------------------------------------------
# Datacenter's plugin endpoints
# -----------------------------------------------------------------------------
$datacenterPluginEndpointsDefinition = @(
    [Ordered]@{
        DatacenterCode = "AZR-FR-CENTRAL"
        CredentialName = "Azure Credential"
        PluginCode     = "Azure.Plugin"
        Action         = "ADD"
    }
)

$datacenterPluginEndpoints = @($datacenterPluginEndpointsDefinition | % {New-Object -TypeName psobject -Property $_})


return @{
    Plugins = $plugins
    Locations = $locations
    Datacenters = $datacenters
    Credentials = $crdentials
    DatacenterPluginEndpoints = $datacenterPluginEndpoints
}