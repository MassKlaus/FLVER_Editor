# Enable strict mode for better error handling
Set-StrictMode -Version Latest

# Define the source and target directories
$source_folder = ".\FLVER_Editor\Actions"
$target_folder = ".\FLVER.Tests\FLVER2Tests\ActionTests_Try"

# Ensure the target folder exists
if (-not (Test-Path $target_folder)) {
    New-Item -ItemType Directory -Path $target_folder | Out-Null
}

# Get all files in the source folder
$files = Get-ChildItem -Path $source_folder -File

foreach ($file in $files) {
    Write-Output "Processing file: $($file.Name)"

    # Get the file name without extension
    $filename = $file.BaseName
    $class_name = "FLVER2${filename}Tests"
    
    # Define the output file path
    $output_file = "$target_folder\$class_name.cs"

    # Write boilerplate content to the new file
    @"
using System.Numerics;
using FLVER_Editor.Actions;
using SoulsFormats;
using Xunit;

namespace FLVER.Tests.FLVER2Tests.ActionTests;

public class $class_name : IClassFixture<DataFixture>
{
    private readonly DataFixture dataFixture;

    public $class_name(DataFixture dataFixture)
    {
        this.dataFixture = dataFixture;
    }

    [Fact]
    public void UnWrittenTest()
    {
        // This is a placeholder for future implementation.
        // Ensure that the actual logic is implemented later.
        Assert.Fail("This test needs to be implemented.");
    }
}
"@ | Out-File -FilePath $output_file -Encoding utf8

    Write-Output "Created file: $output_file"
}

Write-Output "File creation process completed."
