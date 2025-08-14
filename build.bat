@echo off
echo Building Attribute Metadata Exporter Plugin...
echo.

echo Restoring NuGet packages...
dotnet restore

echo.
echo Building project...
dotnet build --configuration Release

echo.
echo Build complete!
echo.
echo To install the plugin:
echo 1. Copy the DLL from bin\Release\net48\ to your XRM ToolBox plugins folder
echo 2. Restart XRM ToolBox
echo 3. Look for "Attribute Metadata Exporter" in the plugins list
echo.
pause
