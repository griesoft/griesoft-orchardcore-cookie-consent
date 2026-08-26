#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Updates the version in Version.props file.

.DESCRIPTION
    This script updates the version number in the Version.props file.
    It can be used manually or by CI/CD pipelines.

.PARAMETER Version
    The new version number to set (e.g., "1.0.0", "1.2.3-beta")

.PARAMETER IncrementMajor
    Increment the major version number (e.g., 1.0.0 -> 2.0.0)

.PARAMETER IncrementMinor
    Increment the minor version number (e.g., 1.0.0 -> 1.1.0)

.PARAMETER IncrementPatch
    Increment the patch version number (e.g., 1.0.0 -> 1.0.1)

.EXAMPLE
    ./update-version.ps1 -Version "1.2.3"
    Sets the version to 1.2.3

.EXAMPLE
    ./update-version.ps1 -IncrementMinor
    Increments the minor version number

.EXAMPLE
    ./update-version.ps1 -IncrementPatch
    Increments the patch version number
#>

param(
    [Parameter(ParameterSetName = 'SetVersion')]
    [string]$Version,

    [Parameter(ParameterSetName = 'IncrementVersion')]
    [switch]$IncrementMajor,

    [Parameter(ParameterSetName = 'IncrementVersion')]
    [switch]$IncrementMinor,

    [Parameter(ParameterSetName = 'IncrementVersion')]
    [switch]$IncrementPatch
)

$ErrorActionPreference = 'Stop'
$VersionPropsPath = Join-Path $PSScriptRoot 'Version.props'

if (-not (Test-Path $VersionPropsPath)) {
    Write-Error "Version.props file not found at: $VersionPropsPath"
    exit 1
}

# Read current version from Version.props
$content = Get-Content $VersionPropsPath -Raw
$versionMatch = [regex]::Match($content, '<Version>(.*?)</Version>')
if (-not $versionMatch.Success) {
    Write-Error "Could not find <Version> tag in Version.props"
    exit 1
}

$currentVersion = $versionMatch.Groups[1].Value
Write-Host "Current version: $currentVersion"

# Determine new version
$newVersion = $null

if ($PSCmdlet.ParameterSetName -eq 'SetVersion') {
    $newVersion = $Version
}
elseif ($PSCmdlet.ParameterSetName -eq 'IncrementVersion') {
    # Parse current version (handle both regular and pre-release versions)
    $baseVersionMatch = [regex]::Match($currentVersion, '^(\d+)\.(\d+)\.(\d+)')
    if (-not $baseVersionMatch.Success) {
        Write-Error "Could not parse current version: $currentVersion"
        exit 1
    }

    $major = [int]$baseVersionMatch.Groups[1].Value
    $minor = [int]$baseVersionMatch.Groups[2].Value
    $patch = [int]$baseVersionMatch.Groups[3].Value

    if ($IncrementMajor) {
        $major++
        $minor = 0
        $patch = 0
    }
    elseif ($IncrementMinor) {
        $minor++
        $patch = 0
    }
    elseif ($IncrementPatch) {
        $patch++
    }

    $newVersion = "$major.$minor.$patch"
}
else {
    Write-Error "Please specify either -Version or one of the increment options"
    exit 1
}

# Update Version.props
$newContent = $content -replace '<Version>.*?</Version>', "<Version>$newVersion</Version>"
[System.IO.File]::WriteAllText($VersionPropsPath, $newContent)

Write-Host "Updated version to: $newVersion" -ForegroundColor Green
Write-Host "Version.props has been updated successfully."
