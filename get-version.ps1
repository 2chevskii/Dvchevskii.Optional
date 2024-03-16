[CmdletBinding()]
param(
  [Parameter()]
  [switch] $All
)

$gitversion = (. dotnet gitversion) | ConvertFrom-Json -AsHashtable

if ($All) {
  Write-Output $gitversion
} else {
  Write-Output $gitversion.SemVer
}
