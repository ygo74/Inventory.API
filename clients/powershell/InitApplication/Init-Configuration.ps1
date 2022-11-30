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
    New-InventoryPlugin -Name $_.Name -Code $_.Code -Version $_.Version -Path $_.Path
    write-Host "Ok"
}


