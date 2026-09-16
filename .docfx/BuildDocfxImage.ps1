$version = minver -i -t v -v w
$docfxRoot = (Get-Location).Path
$sourceRoot = [System.IO.Path]::GetFullPath((Join-Path $docfxRoot '..\src'))
$docfxConfig = Get-Content -Raw 'docfx.json' | ConvertFrom-Json
$metadataProjectPatterns = @(
    foreach ($metadata in $docfxConfig.metadata) {
        foreach ($source in $metadata.src) {
            $metadataSourceRoot = if ($source.src) {
                [System.IO.Path]::GetFullPath((Join-Path $docfxRoot $source.src))
            } else {
                $docfxRoot
            }

            foreach ($file in $source.files) {
                (Join-Path $metadataSourceRoot $file).Replace('/', [System.IO.Path]::DirectorySeparatorChar)
            }
        }
    }
)
$sourceProjects = @(Get-ChildItem -LiteralPath $sourceRoot -Recurse -File -Filter '*.csproj')
$metadataProjects = @(
    $sourceProjects |
        Where-Object {
            $projectPath = $_.FullName
            foreach ($pattern in $metadataProjectPatterns) {
                if ($projectPath -like $pattern) {
                    return $true
                }
            }

            return $false
        }
)
$sourceProjectsHaveRestoreAssets = $sourceProjects.Count -gt 0
foreach ($project in $sourceProjects) {
    $restoreAssetsPath = Join-Path $project.DirectoryName 'obj\project.assets.json'
    if (-not (Test-Path -LiteralPath $restoreAssetsPath -PathType Leaf)) {
        $sourceProjectsHaveRestoreAssets = $false
        break
    }
}

$useNoRestore = $metadataProjects.Count -gt 0 -and $sourceProjectsHaveRestoreAssets
$restoreInputNames = @('Directory.Build.props', 'Directory.Build.targets', 'Directory.Packages.props', 'NuGet.Config', 'nuget.config', 'global.json')

foreach ($project in $metadataProjects) {
    $restoreAssetsPath = Join-Path $project.DirectoryName 'obj\project.assets.json'
    if (-not (Test-Path -LiteralPath $restoreAssetsPath -PathType Leaf)) {
        $useNoRestore = $false
        break
    }

    $restoreAssetsLastWriteTime = (Get-Item -LiteralPath $restoreAssetsPath).LastWriteTimeUtc
    $restoreInputPaths = [System.Collections.Generic.List[string]]::new()
    $restoreInputPaths.Add($project.FullName)

    $inputDirectory = $project.DirectoryName
    while ($inputDirectory) {
        foreach ($name in $restoreInputNames) {
            $restoreInputPath = Join-Path $inputDirectory $name
            if (Test-Path -LiteralPath $restoreInputPath -PathType Leaf) {
                $restoreInputPaths.Add($restoreInputPath)
            }
        }

        $parentDirectory = Split-Path -Parent $inputDirectory
        if (-not $parentDirectory -or $parentDirectory -eq $inputDirectory) {
            break
        }

        $inputDirectory = $parentDirectory
    }

    $lockFilePath = Join-Path $project.DirectoryName 'packages.lock.json'
    if (Test-Path -LiteralPath $lockFilePath -PathType Leaf) {
        $restoreInputPaths.Add($lockFilePath)
    }

    if ($env:APPDATA) {
        $userNuGetConfigPath = Join-Path $env:APPDATA 'NuGet\NuGet.Config'
        if (Test-Path -LiteralPath $userNuGetConfigPath -PathType Leaf) {
            $restoreInputPaths.Add($userNuGetConfigPath)
        }
    }

    foreach ($restoreInputPath in $restoreInputPaths | Sort-Object -Unique) {
        if ((Get-Item -LiteralPath $restoreInputPath).LastWriteTimeUtc -gt $restoreAssetsLastWriteTime) {
            $useNoRestore = $false
            break
        }
    }

    if (-not $useNoRestore) {
        break
    }
}

# Keep the metadata groups in one process; DocFX carries resolver state across groups.
if ($useNoRestore) {
    docfx metadata docfx.json --noRestore
} else {
    docfx metadata docfx.json
}
docker buildx build -t cuemon-docfx:$version --platform linux/arm64,linux/amd64 --load -f Dockerfile.docfx .
get-childItem -recurse -path api -include *.yml, .manifest | remove-item
