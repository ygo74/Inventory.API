

$pluginsDefinition = @(
    [Ordered]@{Name="Azure Plugin"; Code="Azure.Plugin"; Version="1.0"; Path="D:\devel\github\ansible_inventory\Services\plugins\Azure\Inventory.Plugins.Azure\bin\Debug\net6.0\Inventory.Plugins.Azure.dll"}
    [Ordered]@{Name="Internal Plugin"; Code="Internal.Plugin"; Version="1.0"; Path="Null"}
    [Ordered]@{Name="Google Plugin"; Code="Google.Plugin"; Version="1.0"; Path="Null"}
    [Ordered]@{Name="Amazon AWS Plugin"; Code="Aws.Plugin"; Version="1.0"; Path="Null"}
)

$plugins = @($pluginsDefinition | % {New-Object -TypeName psobject -Property $_})

return @{
    Plugins = $plugins
}