#!/usr/bin/env pwsh
# PowerShell pre-commit hook. Make executable by setting core.hooksPath to .githooks and ensure script execution policy allows running this script.
$solution = "CloudOrder.sln"
Write-Host "Running pre-commit checks: restore, build, and tests..."
dotnet restore $solution
if ($LASTEXITCODE -ne 0) { Write-Error "dotnet restore failed"; exit 1 }
dotnet build $solution --no-restore
if ($LASTEXITCODE -ne 0) { Write-Error "Build failed"; exit 1 }
dotnet test $solution --no-build
if ($LASTEXITCODE -ne 0) { Write-Error "Tests failed"; exit 1 }
Write-Host "Pre-commit checks passed."
exit 0
