# XRM ToolBox Plugin Build & Deploy Script
# Builds and deploys AttributeExporterXrmToolBoxPlugin to XRM ToolBox

param(
    [switch]$SkipBuild
)

Write-Host "`n=== XRM ToolBox Plugin Deployment ===" -ForegroundColor Cyan

# Build
if (-not $SkipBuild) {
    Write-Host "`nBuilding plugin..." -ForegroundColor Green

    dotnet clean AttributeExporterXrmToolBoxPlugin.sln --configuration Release | Out-Null
    dotnet build AttributeExporterXrmToolBoxPlugin.sln --configuration Release

    if ($LASTEXITCODE -ne 0) {
        Write-Host "`nBuild failed!" -ForegroundColor Red
        exit 1
    }
    Write-Host "Build successful!" -ForegroundColor Green
} else {
    Write-Host "`nSkipping build (using existing binaries)..." -ForegroundColor Yellow
}

# Deploy
Write-Host "`nDeploying plugin..." -ForegroundColor Green
$pluginsPath = "$env:APPDATA\MscrmTools\XrmToolBox\Plugins"

# Verify build output exists
if (-not (Test-Path "bin\Release\net48\AttributeExporterXrmToolBoxPlugin.dll")) {
    Write-Host "Error: Build output not found at bin\Release\net48\AttributeExporterXrmToolBoxPlugin.dll" -ForegroundColor Red
    exit 1
}

# Clean old files
Write-Host "  - Cleaning old plugin files..." -ForegroundColor Gray
Remove-Item "$pluginsPath\AttributeExporterXrmToolBoxPlugin*" -Force -ErrorAction SilentlyContinue
Remove-Item "$pluginsPath\CsvHelper.dll" -Force -ErrorAction SilentlyContinue
Remove-Item "$pluginsPath\Microsoft.Bcl.AsyncInterfaces.dll" -Force -ErrorAction SilentlyContinue
Remove-Item "$pluginsPath\System.Threading.Tasks.Extensions.dll" -Force -ErrorAction SilentlyContinue

# Copy plugin files
Write-Host "  - Copying plugin DLL..." -ForegroundColor Gray
Copy-Item "bin\Release\net48\AttributeExporterXrmToolBoxPlugin.dll" -Destination $pluginsPath -Force
Copy-Item "bin\Release\net48\CsvHelper.dll" -Destination $pluginsPath -Force

# Copy dependencies from NuGet cache
Write-Host "  - Copying dependencies..." -ForegroundColor Gray
$asyncInterfacesPath = "$env:USERPROFILE\.nuget\packages\microsoft.bcl.asyncinterfaces\1.0.0\lib\net461\Microsoft.Bcl.AsyncInterfaces.dll"
$threadingTasksPath = "$env:USERPROFILE\.nuget\packages\system.threading.tasks.extensions\4.5.4\lib\net461\System.Threading.Tasks.Extensions.dll"

if (Test-Path $asyncInterfacesPath) {
    Copy-Item $asyncInterfacesPath -Destination $pluginsPath -Force
} else {
    Write-Host "  Warning: Could not find Microsoft.Bcl.AsyncInterfaces v1.0.0 in NuGet cache" -ForegroundColor Yellow
    Write-Host "  Trying to copy from build output..." -ForegroundColor Yellow
    if (Test-Path "bin\Release\net48\Microsoft.Bcl.AsyncInterfaces.dll") {
        Copy-Item "bin\Release\net48\Microsoft.Bcl.AsyncInterfaces.dll" -Destination $pluginsPath -Force
    }
}

if (Test-Path $threadingTasksPath) {
    Copy-Item $threadingTasksPath -Destination $pluginsPath -Force
} else {
    Write-Host "  Warning: Could not find System.Threading.Tasks.Extensions v4.5.4 in NuGet cache" -ForegroundColor Yellow
    Write-Host "  Trying to copy from build output..." -ForegroundColor Yellow
    if (Test-Path "bin\Release\net48\System.Threading.Tasks.Extensions.dll") {
        Copy-Item "bin\Release\net48\System.Threading.Tasks.Extensions.dll" -Destination $pluginsPath -Force
    }
}

# Delete manifest to force rescan
Write-Host "  - Removing manifest cache..." -ForegroundColor Gray
Remove-Item "$pluginsPath\manifest.json" -Force -ErrorAction SilentlyContinue

# Verify deployment
Write-Host "`nVerifying deployment..." -ForegroundColor Green
$deployedFiles = @(
    "AttributeExporterXrmToolBoxPlugin.dll",
    "CsvHelper.dll",
    "Microsoft.Bcl.AsyncInterfaces.dll",
    "System.Threading.Tasks.Extensions.dll"
)

$allFilesPresent = $true
foreach ($file in $deployedFiles) {
    $filePath = Join-Path $pluginsPath $file
    if (Test-Path $filePath) {
        $fileInfo = Get-Item $filePath
        Write-Host "  [OK] $file ($([math]::Round($fileInfo.Length / 1KB, 0)) KB)" -ForegroundColor Green
    } else {
        Write-Host "  [MISSING] $file" -ForegroundColor Red
        $allFilesPresent = $false
    }
}

# Summary
Write-Host "`n=== Deployment Summary ===" -ForegroundColor Cyan
if ($allFilesPresent) {
    Write-Host "Status: " -NoNewline
    Write-Host "SUCCESS" -ForegroundColor Green
    Write-Host "`nPlugin deployed to: $pluginsPath" -ForegroundColor White
    Write-Host "`nNext steps:" -ForegroundColor Yellow
    Write-Host "  1. Close XRM ToolBox completely (check Task Manager)" -ForegroundColor White
    Write-Host "  2. Start XRM ToolBox" -ForegroundColor White
    Write-Host "  3. Look for 'Attribute Metadata Exporter' in Tools menu" -ForegroundColor White
} else {
    Write-Host "Status: " -NoNewline
    Write-Host "FAILED" -ForegroundColor Red
    Write-Host "Some files are missing. Check errors above." -ForegroundColor Red
    exit 1
}

Write-Host ""
