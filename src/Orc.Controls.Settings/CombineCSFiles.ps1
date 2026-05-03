# Get the directory where this script is located
$sourceDirectory = $PSScriptRoot
$outputFile = Join-Path $sourceDirectory "Combined.__cs"

# Get all .cs files recursively
$files = Get-ChildItem -Path $sourceDirectory -Filter "*.cs" -Recurse |
         Where-Object { $_.Name -ne "Combined.__cs" }  # Exclude the output file itself

# Create or clear the output file
"" | Set-Content $outputFile

# Add a header comment
"// Combined C# files - Generated on $(Get-Date)`n" | Add-Content $outputFile

foreach ($file in $files) {
    # Add a file separator comment
    "`n// ===============================`n" | Add-Content $outputFile
    "// File: $($file.FullName)`n" | Add-Content $outputFile
    
    # Get and append the content of each file
    Get-Content $file.FullName | Add-Content $outputFile
}

Write-Host "Files have been combined into $outputFile"
Write-Host "Source directory: $sourceDirectory"