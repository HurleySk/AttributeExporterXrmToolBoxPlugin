Write-Host "Building Attribute Metadata Exporter Plugin..." -ForegroundColor Green
Write-Host ""

Write-Host "Restoring NuGet packages..." -ForegroundColor Yellow
dotnet restore

Write-Host ""
Write-Host "Building project..." -ForegroundColor Yellow
dotnet build --configuration Release

Write-Host ""
Write-Host "Build complete!" -ForegroundColor Green
Write-Host ""
Write-Host "To install the plugin:" -ForegroundColor Cyan
Write-Host "1. Copy the DLL from bin\Release\net6.0-windows\ to your XRM ToolBox plugins folder" -ForegroundColor White
Write-Host "2. Restart XRM ToolBox" -ForegroundColor White
Write-Host "3. Look for 'Attribute Metadata Exporter' in the plugins list" -ForegroundColor White
Write-Host ""
Write-Host "Press any key to continue..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
