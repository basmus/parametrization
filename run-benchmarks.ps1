# Run the benchmarks
Write-Host "Starting benchmark execution..."
Write-Host "Configuration: Release"
Write-Host "Project: tests/Basmus.Parametrization.Benchmarks"
Write-Host "----------------------------------------"

# Change to the benchmarks directory
Set-Location tests/Basmus.Parametrization.Benchmarks

# Run the benchmarks
dotnet run --configuration Release

# Change back to the original directory
Set-Location ../..

Write-Host "----------------------------------------"
Write-Host "Benchmark execution completed"