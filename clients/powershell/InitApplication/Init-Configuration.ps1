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
    $existingPlugin = Get-InventoryPlugin -Code $_.Code -ErrorAction SilentlyContinue
    if ($null -eq $existingPlugin)
    {
        $result = New-InventoryPlugin -InputObject $_
    }
    else
    {
        $_ | Add-Member -MemberType NoteProperty -Name Id -Value $existingPlugin.id
        $result = Update-InventoryPlugin -InputObject $_
    }
    write-Host "Ok"
    write-Output $result
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
        $result = New-InventoryLocation -InputObject $_
    }
    else
    {
        $_ | Add-Member -MemberType NoteProperty -Name Id -Value $existingLocation.id
        $result = Update-InventoryLocation -InputObject $_
    }

    write-Host "Ok"
    write-Output $result
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
        $result = New-InventoryDatacenter -InputObject $_
    }
    else
    {
        $_ | Add-Member -MemberType NoteProperty -Name Id -Value $existingDatacenter.id
        $result = Update-InventoryLocation -InputObject $_
    }

    write-Host "Ok"
    write-Output $result
}


# -----------------------------------------------------------------------------
# Load Credentials
# -----------------------------------------------------------------------------
Write-Host ""
Write-Host "Load Credentials" -ForegroundColor Green
Write-Host $titleSeparator -ForegroundColor Green
$configuration.Credentials | Foreach-Object {
    Write-Host "`t - credential $($_.Name) : " -NoNewLine
    $existingCredential = Get-InventoryCredential -Name $_.Name -ErrorAction SilentlyContinue
    if ($null -eq $existingCredential)
    {
        $result = New-InventoryCredential -InputObject $_
    }
    else
    {
        $_ | Add-Member -MemberType NoteProperty -Name Id -Value $existingCredential.id
        $result = Update-InventoryCredential -InputObject $_
    }

    write-Host "Ok"
    write-Output $result
}

