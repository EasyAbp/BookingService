param (
    [string]$folder,
    [string]$solutionFolder,
    [string]$slnFile,
    [switch]$includeTest,
    [switch]$includeHost,
    [switch]$includeBlazor,
    [switch]$includeMongoDb
)
if ([String]::IsNullOrWhiteSpace($slnFile)) {
    $slnFile = "./build/build-tmp.sln"
}

$scriptpath = Split-Path $MyInvocation.MyCommand.Path
$currentPath = [System.IO.Path]::GetDirectoryName($scriptpath)
Write-Output "working directory: $currentPath"
Push-Location $currentPath

$projects = (Get-ChildItem -r ("./" + $folder + "/*.csproj") | ForEach-Object { $_.FullName } | Resolve-Path -Relative) 
$testProjects = New-Object  System.Collections.ArrayList
$hostProjects = New-Object  System.Collections.ArrayList
$normalProjects = New-Object  System.Collections.ArrayList

for ($i = 0; $i -lt $projects.length; $i ++) {
    Write-Output $projects[$i]

    if (!$includeBlazor -and $projects[$i].Contains("Blazor")) {
        continue
    }

    if (!$includeMongoDb -and $projects[$i].Contains("MongoDB")) {
        continue
    }

    if ($projects[$i].Contains("\test\")) {
        $testProjects.Add($projects[$i])
    }
    elseif ($projects[$i].Contains("\host\")) {
        $hostProjects.Add($projects[$i])
    }
    else {
        $normalProjects.Add($projects[$i])
    }
} 

dotnet sln $slnFile add $normalProjects -s ($solutionFolder + "\src")

if ($includeTest -and ($testProjects.Count -gt 0)) {
    dotnet sln $slnFile add $testProjects -s ($solutionFolder + "\tests")
}

if ($includeHost -and ($hostProjects.Count -gt 0)) {
    dotnet sln $slnFile add $hostProjects -s ($solutionFolder + "\host")
}
