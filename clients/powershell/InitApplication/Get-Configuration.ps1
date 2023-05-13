
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
# PLugins
# -----------------------------------------------------------------------------
$locationsDefinition = @(
    [Ordered]@{
        Name = "Paris"
        Description = "Paris Datacenter On premises"
        CountryCode = "FR"
        CityCode = "PAR"
        RegionCode = "EMEA"
        InventoryCode = "emea.fr.par"}
)

$locations = @($locationsDefinition | % {New-Object -TypeName psobject -Property $_})


return @{
    Plugins = $plugins
    Locations = $locations
}