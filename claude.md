# XRM ToolBox Plugin Deployment Troubleshooting

## Issue Summary
Plugin was not appearing in XRM ToolBox Tools menu despite successful build.

## Root Causes Identified

### 1. **Missing .resx File**
- `AttributeExporterControl.resx` was missing
- Required for Windows Forms controls to serialize properly
- **Solution:** Created standard .resx file with ResX schema

### 2. **Wrong Plugin Deployment Location**
- Initially deployed to: `C:\Users\shurley\OneDrive - Procentrix, Inc\Documents\XrmToolbox\`
- Correct location: `C:\Users\shurley\AppData\Roaming\MscrmTools\XrmToolBox\Plugins\`
- **Solution:** Deployed to correct AppData folder

### 3. **Costura.Fody Dependency Embedding Issues**
- Costura.Fody was embedding dependencies inside the main DLL
- XRM ToolBox's MEF (Managed Extensibility Framework) loader couldn't extract embedded dependencies
- **Solution:** Disabled Costura.Fody, output all DLLs separately

### 4. **CsvHelper Version Dependency Conflicts** (PRIMARY ISSUE)
- CsvHelper 30.0.1 requires `Microsoft.Bcl.AsyncInterfaces` v9.0.0.7 (not available)
- CsvHelper 15.0.10 requires `Microsoft.Bcl.AsyncInterfaces` v8.0.0.0 (version mismatch)
- **Final Solution:** Downgrade to CsvHelper 12.3.2 which requires:
  - `Microsoft.Bcl.AsyncInterfaces` v1.0.0.0 ✓
  - `System.Threading.Tasks.Extensions` v4.2.0.0 ✓

## Final Working Configuration

### Package Versions
```xml
<PackageReference Include="CsvHelper" Version="12.3.2" />
<PackageReference Include="XrmToolBoxPackage" Version="1.2025.7.71" />
<PackageReference Include="MscrmTools.Xrm.Connection" Version="1.2025.7.63" />
```

### CsvHelper API Change
CsvHelper 12.x uses older API pattern:
```csharp
// OLD (v30):
using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))

// NEW (v12):
using (var csv = new CsvWriter(writer))
{
    csv.Configuration.CultureInfo = CultureInfo.InvariantCulture;
    csv.WriteRecords(records);
}
```

### Fody Configuration
Disabled Costura in `FodyWeavers.xml`:
```xml
<?xml version="1.0" encoding="utf-8"?>
<Weavers xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:noNamespaceSchemaLocation="FodyWeavers.xsd">
  <!-- Costura disabled - XrmToolBox needs separate DLLs -->
  <!-- <Costura /> -->
</Weavers>
```

### Deployed Files (4 files minimum)
Located in: `C:\Users\{username}\AppData\Roaming\MscrmTools\XrmToolBox\Plugins\`

1. **AttributeExporterXrmToolBoxPlugin.dll** (23KB)
2. **CsvHelper.dll** (146KB - v12.3.2)
3. **Microsoft.Bcl.AsyncInterfaces.dll** (21KB - v1.0.0.0)
4. **System.Threading.Tasks.Extensions.dll** (26KB - v4.2.0.1)

## Deployment Process

### Build Steps
```bash
# Clean previous builds
dotnet clean AttributeExporterXrmToolBoxPlugin.sln --configuration Release

# Restore packages
dotnet restore

# Build
dotnet build AttributeExporterXrmToolBoxPlugin.sln --configuration Release
```

### Deployment Steps
```powershell
# Set plugin path
$pluginsPath = "$env:APPDATA\MscrmTools\XrmToolBox\Plugins"

# Clean old versions
Remove-Item "$pluginsPath\AttributeExporterXrmToolBoxPlugin*" -Force
Remove-Item "$pluginsPath\CsvHelper.dll" -Force

# Copy plugin and dependency
Copy-Item "bin\Release\net48\AttributeExporterXrmToolBoxPlugin.dll" -Destination $pluginsPath -Force
Copy-Item "bin\Release\net48\CsvHelper.dll" -Destination $pluginsPath -Force

# Copy correct dependency versions from NuGet cache
Copy-Item "$env:USERPROFILE\.nuget\packages\microsoft.bcl.asyncinterfaces\1.0.0\lib\net461\Microsoft.Bcl.AsyncInterfaces.dll" -Destination $pluginsPath -Force
Copy-Item "$env:USERPROFILE\.nuget\packages\system.threading.tasks.extensions\4.5.4\lib\net461\System.Threading.Tasks.Extensions.dll" -Destination $pluginsPath -Force
```

## Testing Process

1. **Close XRM ToolBox completely**
   - End all `XrmToolBox.exe` processes in Task Manager
   - XRM ToolBox caches plugin metadata

2. **Delete cached manifest (optional)**
   ```powershell
   Remove-Item "$env:APPDATA\MscrmTools\XrmToolBox\Plugins\manifest.json" -Force
   ```

3. **Start XRM ToolBox**
   - It will rescan all plugins
   - Check Tools menu for "Attribute Metadata Exporter"

## Troubleshooting Tips

### Check Plugin is Scanned
Look in `manifest.json` for your plugin:
```powershell
Get-Content "$env:APPDATA\MscrmTools\XrmToolBox\Plugins\manifest.json" | Select-String "AttributeExporter"
```

### Verify Dependency Versions
```powershell
[System.Reflection.AssemblyName]::GetAssemblyName("$env:APPDATA\MscrmTools\XrmToolBox\Plugins\Microsoft.Bcl.AsyncInterfaces.dll").Version
# Should show: 1.0.0.0

[System.Reflection.AssemblyName]::GetAssemblyName("$env:APPDATA\MscrmTools\XrmToolBox\Plugins\System.Threading.Tasks.Extensions.dll").Version
# Should show: 4.2.0.1
```

### Check XRM ToolBox Logs
Error logs location:
```
C:\Users\{username}\AppData\Roaming\MscrmTools\XrmToolBox\1.2025.7.71\
```

### Common Errors and Solutions

#### "Could not load Microsoft.Bcl.AsyncInterfaces Version=9.0.0.7"
- CsvHelper version too new
- Downgrade CsvHelper to 12.3.2

#### "Could not load Microsoft.Bcl.AsyncInterfaces Version=1.0.0.0"
- Wrong version of dependency deployed
- Copy v1.0.0.0 from NuGet cache (see deployment steps)

#### "System.Reflection.ReflectionTypeLoadException"
- Missing dependencies
- Check all 4 required DLLs are in Plugins folder

#### Plugin doesn't appear in Tools menu
- Wrong deployment folder (use AppData, not OneDrive/Documents)
- Restart XRM ToolBox completely
- Delete manifest.json to force rescan

## Key Learnings

1. **XRM ToolBox MEF Loader Requirements:**
   - Cannot extract embedded dependencies (Costura doesn't work)
   - Requires exact dependency versions
   - Binding redirects don't work for MEF loading

2. **Modern NuGet Packages:**
   - Newer packages (CsvHelper 30+) require .NET Standard 2.1+ dependencies
   - .NET Framework 4.8 doesn't have compatible versions
   - Use older package versions designed for .NET Framework

3. **Deployment Location Matters:**
   - XRM ToolBox has multiple possible plugin locations
   - AppData\Roaming location takes precedence
   - OneDrive/Documents location may exist but be outdated

4. **Testing Best Practices:**
   - Always close XRM ToolBox completely before testing
   - Delete manifest.json to force fresh scan
   - Check Task Manager for lingering processes

## Project File Reference

Final working `AttributeExporterXrmToolBoxPlugin.csproj`:
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net48</TargetFramework>
    <OutputType>Library</OutputType>
    <UseWindowsForms>true</UseWindowsForms>
    <GenerateAssemblyInfo>false</GenerateAssemblyInfo>
    <GenerateResourceUsePreserializedResources>true</GenerateResourceUsePreserializedResources>
    <CopyLocalLockFileAssemblies>true</CopyLocalLockFileAssemblies>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="XrmToolBoxPackage" Version="1.2025.7.71" />
    <PackageReference Include="MscrmTools.Xrm.Connection" Version="1.2025.7.63" />
    <PackageReference Include="Microsoft.CrmSdk.CoreAssemblies" Version="9.0.2.59" />
    <PackageReference Include="System.Resources.Extensions" Version="4.7.1" />
    <PackageReference Include="CsvHelper" Version="12.3.2" />

    <!-- Fody packages - Costura disabled -->
    <PackageReference Include="Fody" Version="6.8.2" PrivateAssets="all" />
    <PackageReference Include="Costura.Fody" Version="6.0.0" PrivateAssets="all" />
  </ItemGroup>

  <ItemGroup>
    <Reference Include="System.ComponentModel.Composition" />
  </ItemGroup>
</Project>
```

## Automated Build & Deploy Script

Create `deploy.ps1` in project root:
```powershell
# Build
Write-Host "Building plugin..." -ForegroundColor Green
dotnet clean AttributeExporterXrmToolBoxPlugin.sln --configuration Release
dotnet build AttributeExporterXrmToolBoxPlugin.sln --configuration Release

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed!" -ForegroundColor Red
    exit 1
}

# Deploy
Write-Host "Deploying plugin..." -ForegroundColor Green
$pluginsPath = "$env:APPDATA\MscrmTools\XrmToolBox\Plugins"

# Clean old files
Remove-Item "$pluginsPath\AttributeExporterXrmToolBoxPlugin*" -Force -ErrorAction SilentlyContinue
Remove-Item "$pluginsPath\CsvHelper.dll" -Force -ErrorAction SilentlyContinue
Remove-Item "$pluginsPath\Microsoft.Bcl.AsyncInterfaces.dll" -Force -ErrorAction SilentlyContinue
Remove-Item "$pluginsPath\System.Threading.Tasks.Extensions.dll" -Force -ErrorAction SilentlyContinue

# Copy plugin files
Copy-Item "bin\Release\net48\AttributeExporterXrmToolBoxPlugin.dll" -Destination $pluginsPath -Force
Copy-Item "bin\Release\net48\CsvHelper.dll" -Destination $pluginsPath -Force

# Copy dependencies from NuGet cache
Copy-Item "$env:USERPROFILE\.nuget\packages\microsoft.bcl.asyncinterfaces\1.0.0\lib\net461\Microsoft.Bcl.AsyncInterfaces.dll" -Destination $pluginsPath -Force
Copy-Item "$env:USERPROFILE\.nuget\packages\system.threading.tasks.extensions\4.5.4\lib\net461\System.Threading.Tasks.Extensions.dll" -Destination $pluginsPath -Force

# Delete manifest to force rescan
Remove-Item "$pluginsPath\manifest.json" -Force -ErrorAction SilentlyContinue

Write-Host "Deployment complete!" -ForegroundColor Green
Write-Host "Please restart XRM ToolBox to load the plugin." -ForegroundColor Yellow
```

Usage:
```powershell
.\deploy.ps1
```

---

**Document Version:** 1.0
**Date:** 2025-09-30
**XRM ToolBox Version:** 1.2025.7.71
**Plugin Status:** Working ✓
