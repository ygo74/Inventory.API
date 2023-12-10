[CmdletBinding()]
param()
$ErrorActionPreference = "Stop"
$scriptPath = Resolve-Path (Join-Path -Path $PsScriptRoot -ChildPath Get-Configuration.ps1)
$configuration = . $scriptPath

$titleSeparator = "".PadRight(80,"=")

# -----------------------------------------------------------------------------
# Load plugins
# -----------------------------------------------------------------------------
Write-Host ""
Write-Host "Load plugins" -ForegroundColor Green
Write-Host $titleSeparator -ForegroundColor Green
$configuration.plugins | Foreach-Object {
    Write-Host "`t - plugin $($_.Name) : " -NoNewLine
    New-InventoryPlugin -Name $_.Name -Code $_.Code -Version $_.Version -Path $_.Path -InventoryCode $_.InventoryCode -verbose
    write-Host "Ok"
}


# -----------------------------------------------------------------------------
# Load Locations
# -----------------------------------------------------------------------------
Write-Host ""
Write-Host "Load locations" -ForegroundColor Green
Write-Host $titleSeparator -ForegroundColor Green
$configuration.locations | Foreach-Object {
    Write-Host "`t - location $($_.Name) : " -NoNewLine
    $existingLocation = Get-InventoryLocation -Name $_.Name -ErrorAction SilentlyContinue
    if ($null -eq $existingLocation)
    {
        New-InventoryLocation -InputObject $_
    }
    else
    {
        $_ | Add-Member -MemberType NoteProperty -Name Id -Value $existingLocation.id
        Update-InventoryLocation -InputObject $_
    }

    write-Host "Ok"
}

# -----------------------------------------------------------------------------
# Load Datacenters
# -----------------------------------------------------------------------------
Write-Host ""
Write-Host "Load Datacenters" -ForegroundColor Green
Write-Host $titleSeparator -ForegroundColor Green
$configuration.Datacenters | Foreach-Object {
    Write-Host "`t - datacenter $($_.Name) : " -NoNewLine
    $existingDatacenter = Get-InventoryDatacenter -Name $_.Name -ErrorAction SilentlyContinue
    if ($null -eq $existingDatacenter)
    {
        New-InventoryDatacenter -InputObject $_
    }
    else
    {
        $_ | Add-Member -MemberType NoteProperty -Name Id -Value $existingDatacenter.id
        Update-InventoryLocation -InputObject $_
    }

    write-Host "Ok"
}


