#Requires -Version 5.1

param(
    # Path to the Named Parametrizer benchmark report file
    [Parameter(Mandatory=$false)]
    [string]$NamedBenchmarkReportPath = "tests/Basmus.Parametrization.Benchmarks/artifacts/results/Basmus.Parametrization.Benchmarks.NamedParametrizerBenchmarks-report-github.md",
    
    # Path to the Positional Parametrizer benchmark report file
    [Parameter(Mandatory=$false)]
    [string]$PositionalBenchmarkReportPath = "tests/Basmus.Parametrization.Benchmarks/artifacts/results/Basmus.Parametrization.Benchmarks.PositionalParametrizerBenchmarks-report-github.md",
    
    # Path to the README file
    [Parameter(Mandatory=$false)]
    [string]$ReadmePath = "README.md"
)

function Convert-HtmlEntities {
    param([string]$text)
    return $text -replace '&#39;', "'" -replace '&quot;', '"' -replace '&amp;', '&' -replace '&lt;', '<' -replace '&gt;', '>'
}

function Convert-MarkdownTableToArray {
    param(
        [Parameter(Mandatory=$true)]
        [string]$MarkdownTable,
        
        [Parameter(Mandatory=$false)]
        [string[]]$SelectedColumns = @()
    )

    # Split the content into lines and filter out empty lines
    $lines = $MarkdownTable -split "`n" | Where-Object { $_ -match '\S' }
    
    # Find the table content
    $tableLines = $lines | Where-Object { $_ -match '^\s*\|' }
    if (-not $tableLines) {
        Write-Error "No table found in the content"
        return @()
    }
    
    # Initialize the result array
    $result = @()
    $headerRow = $null
    $columnIndices = @()
    
    foreach ($line in $tableLines) {
        # Skip the separator line (the one with dashes)
        if ($line -match '^\s*\|[\s-:]+\|') {
            continue
        }
        
        # Split the line by | and trim whitespace
        $cells = $line -split '\|' | ForEach-Object { $_.Trim() }
        
        # Remove empty elements at the start and end
        $cells = $cells | Where-Object { $_ -ne '' }
        
        # Convert HTML entities in each cell
        $cells = $cells | ForEach-Object { Convert-HtmlEntities $_ }
        
        # If this is the header row, process column selection
        if (-not $headerRow) {
            $headerRow = $cells
            
            # If no columns are selected, use all columns
            if ($SelectedColumns.Count -eq 0) {
                $columnIndices = 0..($cells.Count - 1)
            } else {
                # Find indices of selected columns
                $columnIndices = $SelectedColumns | ForEach-Object {
                    $index = $headerRow.IndexOf($_)
                    if ($index -eq -1) {
                        Write-Warning "Column '$_' not found in table"
                    }
                    $index
                } | Where-Object { $_ -ne -1 }
                
                if ($columnIndices.Count -eq 0) {
                    Write-Error "None of the selected columns were found in the table"
                    return @()
                }
            }
        }
        
        # Filter cells based on selected columns
        $filteredCells = $columnIndices | ForEach-Object { $cells[$_] }
        
        # Add the row to the result
        if ($filteredCells.Count -gt 0) {
            $result += ,@($filteredCells)
        }
    }
    
    return $result
}

function Format-Table {
    param(
        [Parameter(Mandatory=$true)]
        [object[]]$TableArray
    )
    
    if ($TableArray.Count -eq 0) {
        return "No data to display"
    }
    
    # Calculate column widths
    $columnWidths = @()
    for ($i = 0; $i -lt $TableArray[0].Count; $i++) {
        $maxWidth = ($TableArray | ForEach-Object { $_[$i].ToString().Length } | Measure-Object -Maximum).Maximum
        $columnWidths += $maxWidth
    }
    
    $output = @()
    
    # Add header
    $headerRow = $TableArray[0]
    $formattedHeader = @()
    for ($i = 0; $i -lt $headerRow.Count; $i++) {
        $formattedHeader += $headerRow[$i].ToString().PadRight($columnWidths[$i])
    }
    $output += ($formattedHeader -join " | ")
    
    # Add separator
    $separator = $columnWidths | ForEach-Object { "-" * $_ }
    $output += ($separator -join " | ")
    
    # Add data rows
    for ($rowIndex = 1; $rowIndex -lt $TableArray.Count; $rowIndex++) {
        $row = $TableArray[$rowIndex]
        $formattedRow = @()
        for ($i = 0; $i -lt $row.Count; $i++) {
            $formattedRow += $row[$i].ToString().PadRight($columnWidths[$i])
        }
        $output += ($formattedRow -join " | ")
    }
    
    return $output -join "`n"
}

function Update-ReadmeSubsection {
    param(
        [string]$ReadmePath,
        [string]$SubsectionName,
        [string]$NewContent
    )
    
    Write-Host "Replacing subsection '$SubsectionName' in README"
    $readmeContent = Get-Content -Path $ReadmePath -Raw
    
    # Create pattern to match the subsection and its content until next section (### or ##)
    $pattern = "(?s)### $SubsectionName.*?(?=### |## |\z)"
    
    # Replace the subsection content while preserving the header
    $newReadmeContent = [regex]::Replace($readmeContent, $pattern, "### $SubsectionName`n`n$NewContent`n")
    
    # Write the updated content back to the file
    Set-Content -Path $ReadmePath -Value $newReadmeContent -NoNewline
}

function Format-SectionContent {
    param(
        [Parameter(Mandatory=$true)]
        [array]$Table,
        [Parameter(Mandatory=$true)]
        [string]$DetailsLink
    )

    $content = $Table
    $content = $content.Trim() + "`n`n[Details]($DetailsLink)`n"
    return $content
}

function Update-ReadmeSubsection {
    param(
        [string]$ReadmePath,
        [string]$SubsectionName,
        [string]$NewContent
    )
    
    Write-Host "Replacing subsection '$SubsectionName' in README"
    $readmeContent = Get-Content -Path $ReadmePath -Raw
    
    # Create pattern to match the subsection and its content until next section (### or ##)
    $pattern = "(?s)### $SubsectionName.*?(?=### |## |\z)"
    
    # Replace the subsection content while preserving the header
    $newReadmeContent = [regex]::Replace($readmeContent, $pattern, "### $SubsectionName`n`n$NewContent`n")
    
    # Write the updated content back to the file
    Set-Content -Path $ReadmePath -Value $newReadmeContent -NoNewline
}

$namedReportContent = Get-Content -Path $NamedBenchmarkReportPath -Raw
$positionalReportContent = Get-Content -Path $PositionalBenchmarkReportPath -Raw

# Convert the Markdown tables to arrays
$namedResults = Convert-MarkdownTableToArray -MarkdownTable $namedReportContent -SelectedColumns @("Method", "Mean", "Allocated")
$positionalResults = Convert-MarkdownTableToArray -MarkdownTable $positionalReportContent -SelectedColumns @("Method", "Mean", "Allocated")

# Build the tables
$namedTable = Format-Table -TableArray $namedResults
$positionalTable = Format-Table -TableArray $positionalResults

# Build the section content
$namedContent = Format-SectionContent -Table $namedTable -DetailsLink $NamedBenchmarkReportPath
$positionalContent = Format-SectionContent -Table $positionalTable -DetailsLink $PositionalBenchmarkReportPath

#replace the content in the README file
Update-ReadmeSubsection -ReadmePath $ReadmePath -SubsectionName "Named Parametrizer" -NewContent $namedContent
Update-ReadmeSubsection -ReadmePath $ReadmePath -SubsectionName "Positional Parametrizer" -NewContent $positionalContent