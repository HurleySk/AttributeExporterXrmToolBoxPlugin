# XRM ToolBox Plugin Build & Deploy Script
# Builds and deploys AttributeExporterXrmToolBoxPlugin to XRM ToolBox

param(
    [switch]$SkipBuild,
    [switch]$Force,
    [switch]$WhatIf
)

$ErrorActionPreference = "Stop"

# File list - defined once
$pluginFiles = @(
    "AttributeExporterXrmToolBoxPlugin.dll",
    "AttributeExporterXrmToolBoxPlugin.pdb",
    "CsvHelper.dll"
)

$dependencyFiles = @(
    @{
        Name = "Microsoft.Bcl.AsyncInterfaces.dll"
        NuGetPath = "$env:USERPROFILE\.nuget\packages\microsoft.bcl.asyncinterfaces\1.0.0\lib\net461\Microsoft.Bcl.AsyncInterfaces.dll"
    },
    @{
        Name = "System.Threading.Tasks.Extensions.dll"
        NuGetPath = "$env:USERPROFILE\.nuget\packages\system.threading.tasks.extensions\4.5.4\lib\net461\System.Threading.Tasks.Extensions.dll"
    }
)

Write-Host "`n=== XRM ToolBox Plugin Deployment ===" -ForegroundColor Cyan

# Check if XRM ToolBox is running
Write-Host "`nChecking for running XRM ToolBox processes..." -ForegroundColor Yellow
$xrmProcesses = Get-Process -Name "XrmToolBox" -ErrorAction SilentlyContinue

if ($xrmProcesses) {
    Write-Host "  WARNING: XRM ToolBox is currently running!" -ForegroundColor Red
    Write-Host "  Found $($xrmProcesses.Count) process(es) with PID(s): $($xrmProcesses.Id -join ', ')" -ForegroundColor Yellow

    if ($Force) {
        Write-Host "  -Force specified, attempting to close XRM ToolBox..." -ForegroundColor Yellow
        foreach ($proc in $xrmProcesses) {
            try {
                $proc.CloseMainWindow() | Out-Null
                Start-Sleep -Milliseconds 500
                if (!$proc.HasExited) {
                    $proc.Kill()
                }
                Write-Host "  Closed process $($proc.Id)" -ForegroundColor Green
            }
            catch {
                Write-Host "  Failed to close process $($proc.Id): $($_.Exception.Message)" -ForegroundColor Red
            }
        }
        Start-Sleep -Seconds 2
    }
    else {
        Write-Host "`n  Please close XRM ToolBox before deploying, or use -Force to close automatically." -ForegroundColor Yellow
        Write-Host "  Press Ctrl+C to cancel, or press Enter to continue anyway..." -ForegroundColor Gray
        Read-Host
    }
}
else {
    Write-Host "  No running XRM ToolBox processes found." -ForegroundColor Green
}

# Build
if (-not $SkipBuild) {
    Write-Host "`nBuilding plugin..." -ForegroundColor Green

    if ($WhatIf) {
        Write-Host "  [WhatIf] Would run: dotnet clean" -ForegroundColor Gray
        Write-Host "  [WhatIf] Would run: dotnet build" -ForegroundColor Gray
    }
    else {
        dotnet clean AttributeExporterXrmToolBoxPlugin.sln --configuration Release --verbosity quiet
        dotnet build AttributeExporterXrmToolBoxPlugin.sln --configuration Release

        if ($LASTEXITCODE -ne 0) {
            Write-Host "`nBuild failed!" -ForegroundColor Red
            exit 1
        }
        Write-Host "Build successful!" -ForegroundColor Green
    }
}
else {
    Write-Host "`nSkipping build (using existing binaries)..." -ForegroundColor Yellow
}

# Verify build output exists
$buildPath = "bin\Release\net48"
if (-not (Test-Path "$buildPath\AttributeExporterXrmToolBoxPlugin.dll")) {
    Write-Host "`nError: Build output not found at $buildPath\AttributeExporterXrmToolBoxPlugin.dll" -ForegroundColor Red
    exit 1
}

# Deployment paths
Write-Host "`nDetecting deployment paths..." -ForegroundColor Green
$pluginsPath = "$env:APPDATA\MscrmTools\XrmToolBox\Plugins"
Write-Host "  Primary: $pluginsPath" -ForegroundColor White

# Auto-detect OneDrive paths
$oneDrivePaths = @()
$possibleOneDrivePaths = @(
    "$env:USERPROFILE\OneDrive\Documents\XrmToolbox",
    "$env:USERPROFILE\OneDrive - *\Documents\XrmToolbox",
    "$env:OneDrive\Documents\XrmToolbox",
    "$env:OneDriveCommercial\Documents\XrmToolbox"
)

foreach ($pattern in $possibleOneDrivePaths) {
    $resolved = Resolve-Path $pattern -ErrorAction SilentlyContinue
    if ($resolved) {
        $oneDrivePaths += $resolved.Path
    }
}

if ($oneDrivePaths.Count -gt 0) {
    Write-Host "  OneDrive locations found:" -ForegroundColor Yellow
    foreach ($path in $oneDrivePaths) {
        Write-Host "    - $path" -ForegroundColor Gray
    }
}

# Function to clean files from a path
function Remove-PluginFiles {
    param([string]$Path, [string]$Location)

    if (-not (Test-Path $Path)) {
        return
    }

    Write-Host "  - Cleaning $Location..." -ForegroundColor Gray

    $filesToRemove = @(
        "$Path\AttributeExporterXrmToolBoxPlugin*",
        "$Path\CsvHelper.dll",
        "$Path\Microsoft.Bcl.AsyncInterfaces.dll",
        "$Path\System.Threading.Tasks.Extensions.dll"
    )

    foreach ($filePattern in $filesToRemove) {
        try {
            if ($WhatIf) {
                $files = Get-Item $filePattern -ErrorAction SilentlyContinue
                if ($files) {
                    Write-Host "    [WhatIf] Would remove: $($files.Name -join ', ')" -ForegroundColor Gray
                }
            }
            else {
                Remove-Item $filePattern -Force -ErrorAction SilentlyContinue
            }
        }
        catch {
            Write-Host "    Warning: Could not remove $filePattern - $($_.Exception.Message)" -ForegroundColor Yellow
        }
    }
}

# Clean all locations
Write-Host "`nCleaning old plugin files..." -ForegroundColor Green
Remove-PluginFiles -Path $pluginsPath -Location "AppData"
foreach ($oneDrivePath in $oneDrivePaths) {
    Remove-PluginFiles -Path $oneDrivePath -Location "OneDrive"
}

# Copy plugin files
Write-Host "`nCopying plugin files..." -ForegroundColor Green
$deploymentFailed = $false

foreach ($file in $pluginFiles) {
    $sourcePath = Join-Path $buildPath $file

    if (-not (Test-Path $sourcePath)) {
        if ($file -like "*.pdb") {
            Write-Host "  - Skipping $file (not found, optional)" -ForegroundColor Gray
            continue
        }
        Write-Host "  - ERROR: Source file not found: $file" -ForegroundColor Red
        $deploymentFailed = $true
        continue
    }

    try {
        if ($WhatIf) {
            Write-Host "  [WhatIf] Would copy: $file" -ForegroundColor Gray
        }
        else {
            Copy-Item $sourcePath -Destination $pluginsPath -Force -ErrorAction Stop
            Write-Host "  - Copied $file" -ForegroundColor Green
        }
    }
    catch {
        Write-Host "  - FAILED to copy $file`: $($_.Exception.Message)" -ForegroundColor Red
        $deploymentFailed = $true
    }
}

# Copy dependencies
Write-Host "`nCopying dependencies..." -ForegroundColor Green
foreach ($dep in $dependencyFiles) {
    $sourcePath = $dep.NuGetPath
    $fallbackPath = Join-Path $buildPath $dep.Name

    # Try NuGet cache first
    if (Test-Path $sourcePath) {
        try {
            if ($WhatIf) {
                Write-Host "  [WhatIf] Would copy: $($dep.Name) (from NuGet cache)" -ForegroundColor Gray
            }
            else {
                Copy-Item $sourcePath -Destination $pluginsPath -Force -ErrorAction Stop
                Write-Host "  - Copied $($dep.Name) (from NuGet cache)" -ForegroundColor Green
            }
        }
        catch {
            Write-Host "  - FAILED to copy $($dep.Name): $($_.Exception.Message)" -ForegroundColor Red
            $deploymentFailed = $true
        }
    }
    # Try build output as fallback
    elseif (Test-Path $fallbackPath) {
        Write-Host "  - NuGet cache not found, using build output for $($dep.Name)" -ForegroundColor Yellow
        try {
            if ($WhatIf) {
                Write-Host "  [WhatIf] Would copy: $($dep.Name) (from build output)" -ForegroundColor Gray
            }
            else {
                Copy-Item $fallbackPath -Destination $pluginsPath -Force -ErrorAction Stop
                Write-Host "  - Copied $($dep.Name) (from build output)" -ForegroundColor Green
            }
        }
        catch {
            Write-Host "  - FAILED to copy $($dep.Name): $($_.Exception.Message)" -ForegroundColor Red
            $deploymentFailed = $true
        }
    }
    else {
        Write-Host "  - ERROR: $($dep.Name) not found in NuGet cache or build output!" -ForegroundColor Red
        $deploymentFailed = $true
    }
}

# Verify deployment
Write-Host "`nVerifying deployment..." -ForegroundColor Green
$allRequiredFiles = $pluginFiles + ($dependencyFiles | ForEach-Object { $_.Name })
$allFilesPresent = $true

foreach ($file in $allRequiredFiles) {
    if ($file -like "*.pdb") {
        continue  # PDB files are optional
    }

    $filePath = Join-Path $pluginsPath $file
    if (Test-Path $filePath) {
        $fileInfo = Get-Item $filePath
        Write-Host "  [OK] $file ($([math]::Round($fileInfo.Length / 1KB, 0)) KB)" -ForegroundColor Green
    }
    else {
        Write-Host "  [MISSING] $file" -ForegroundColor Red
        $allFilesPresent = $false
    }
}

# Delete manifest to force rescan (only if deployment succeeded)
if ($allFilesPresent -and -not $deploymentFailed) {
    Write-Host "`nRemoving manifest cache to force plugin rescan..." -ForegroundColor Gray
    if ($WhatIf) {
        Write-Host "  [WhatIf] Would remove: $pluginsPath\manifest.json" -ForegroundColor Gray
    }
    else {
        Remove-Item "$pluginsPath\manifest.json" -Force -ErrorAction SilentlyContinue
    }
}

# Summary
Write-Host "`n=== Deployment Summary ===" -ForegroundColor Cyan
if ($allFilesPresent -and -not $deploymentFailed) {
    Write-Host "Status: " -NoNewline
    Write-Host "SUCCESS" -ForegroundColor Green
    Write-Host "`nPlugin deployed to: $pluginsPath" -ForegroundColor White

    if ($oneDrivePaths.Count -gt 0) {
        Write-Host "`nNote: Old plugin files were removed from OneDrive locations." -ForegroundColor Yellow
    }

    Write-Host "`nNext steps:" -ForegroundColor Yellow
    Write-Host "  1. Start XRM ToolBox" -ForegroundColor White
    Write-Host "  2. Connect to your environment" -ForegroundColor White
    Write-Host "  3. Look for 'Attribute Metadata Exporter' in Tools menu" -ForegroundColor White
    exit 0
}
else {
    Write-Host "Status: " -NoNewline
    Write-Host "FAILED" -ForegroundColor Red
    Write-Host "`nDeployment incomplete. Check errors above." -ForegroundColor Red
    Write-Host "Files may be locked by XRM ToolBox. Close it completely and try again." -ForegroundColor Yellow
    exit 1
}
