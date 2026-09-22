[CmdletBinding()]
param(
    # Regenerate metadata even when the inputs and generated files are unchanged.
    [switch] $ForceMetadata
)

$ErrorActionPreference = 'Stop'

function Assert-CommandSucceeded([string] $Command) {
    if ($LASTEXITCODE -ne 0) { throw "$Command failed with exit code $LASTEXITCODE." }
}

function Get-SourceFiles([string] $Directory) {
    foreach ($item in Get-ChildItem -LiteralPath $Directory -Force) {
        if ($item.PSIsContainer) {
            if ($item.Name -notin @('bin', 'obj', '.git')) { Get-SourceFiles $item.FullName }
        } else {
            $item.FullName
        }
    }
}

function Get-Fingerprint([string[]] $Paths, [string[]] $Values = @()) {
    $entries = @(
        $Values
        foreach ($path in $Paths | Sort-Object -Unique) {
            if (Test-Path -LiteralPath $path -PathType Leaf) {
                "$path=$((Get-FileHash -LiteralPath $path -Algorithm SHA256).Hash)"
            } else {
                "$path=<missing>"
            }
        }
    )
    $sha = [System.Security.Cryptography.SHA256]::Create()
    try {
        [BitConverter]::ToString($sha.ComputeHash([Text.Encoding]::UTF8.GetBytes(($entries -join [Environment]::NewLine))))
    } finally {
        $sha.Dispose()
    }
}

function Get-MetadataFiles {
    foreach ($directory in $metadataDirectories) {
        if (Test-Path -LiteralPath $directory) {
            Get-ChildItem -LiteralPath $directory -Recurse -Force -File |
                Where-Object { $_.Extension -eq '.yml' -or $_.Name -eq '.manifest' } |
                Select-Object -ExpandProperty FullName
        }
    }
}

$totalTimer = [Diagnostics.Stopwatch]::StartNew()
Push-Location $PSScriptRoot
try {
    $docfxRoot = $PSScriptRoot
    $repoRoot = Split-Path -Parent $docfxRoot
    $sourceRoot = Join-Path $repoRoot 'src'
    $docfxConfig = Get-Content -LiteralPath (Join-Path $docfxRoot 'docfx.json') -Raw | ConvertFrom-Json
    $version = minver -i -t v -v w
    Assert-CommandSucceeded 'minver'
    $docfxVersion = docfx --version
    Assert-CommandSucceeded 'docfx --version'
    $sdkVersion = dotnet --version
    Assert-CommandSucceeded 'dotnet --version'
    $revision = git rev-parse HEAD
    Assert-CommandSucceeded 'git rev-parse HEAD'
    $branch = git rev-parse --abbrev-ref HEAD
    Assert-CommandSucceeded 'git rev-parse --abbrev-ref HEAD'

    # Only these generated destinations may be cleaned; preserve authored Markdown.
    $apiRoot = [IO.Path]::GetFullPath((Join-Path $docfxRoot 'api')) + [IO.Path]::DirectorySeparatorChar
    $metadataDirectories = @(
        foreach ($metadata in $docfxConfig.metadata) {
            $destination = [IO.Path]::GetFullPath((Join-Path $docfxRoot $metadata.dest))
            if (-not $destination.StartsWith($apiRoot, [StringComparison]::OrdinalIgnoreCase)) {
                throw "Metadata destination must be beneath $apiRoot : $destination"
            }
            $destination
        }
    )
    $sourceFiles = @(Get-SourceFiles $sourceRoot)
    $sourceProjects = @($sourceFiles | Where-Object { [IO.Path]::GetExtension($_) -eq '.csproj' })
    $inputPaths = @(
        $PSCommandPath
        $sourceFiles
        Get-ChildItem -LiteralPath $repoRoot -Force -File | Select-Object -ExpandProperty FullName
        foreach ($metadata in $docfxConfig.metadata) {
            if ($metadata.filter) { Join-Path $docfxRoot $metadata.filter }
        }
        $inputDirectory = $repoRoot
        while ($inputDirectory) {
            foreach ($name in @('Directory.Build.props', 'Directory.Build.targets', 'Directory.Packages.props', 'NuGet.Config', 'global.json')) {
                Join-Path $inputDirectory $name
            }
            $inputDirectory = Split-Path -Parent $inputDirectory
        }
        if ($env:APPDATA) { Join-Path $env:APPDATA 'NuGet/NuGet.Config' }
        foreach ($project in $sourceProjects) {
            $objDirectory = Join-Path (Split-Path -Parent $project) 'obj'
            Join-Path $objDirectory 'project.assets.json'
            Join-Path $objDirectory ((Split-Path -Leaf $project) + '.nuget.g.props')
            Join-Path $objDirectory ((Split-Path -Leaf $project) + '.nuget.g.targets')
        }
    )
    $inputValues = @(
        $version; $docfxVersion; $sdkVersion; $revision; $branch
        $docfxConfig.metadata | ConvertTo-Json -Depth 100 -Compress
        foreach ($name in @('CI', 'EMAIL', 'Configuration', 'MSBuildSDKsPath', 'DOTNET_ROOT', 'NUGET_PACKAGES', 'GITHUB_RUN_NUMBER')) {
            "$name=$([Environment]::GetEnvironmentVariable($name))"
        }
    )
    $cachePath = Join-Path $docfxRoot 'obj/metadata-cache.json'
    $inputFingerprint = Get-Fingerprint $inputPaths $inputValues
    $metadataFiles = @(Get-MetadataFiles)
    $cache = $null
    if (Test-Path -LiteralPath $cachePath) {
        try {
            $cache = Get-Content -LiteralPath $cachePath -Raw | ConvertFrom-Json
        } catch {
            Write-Warning 'Metadata cache could not be read; regenerating it.'
        }
    }
    $reuseMetadata = -not $ForceMetadata -and $cache -and
        $cache.inputs -eq $inputFingerprint -and $metadataFiles.Count -gt 0 -and
        $cache.outputs -eq (Get-Fingerprint $metadataFiles)

    if ($reuseMetadata) {
        Write-Host 'Metadata unchanged; reusing verified generated files.'
    } else {
        $metadataTimer = [Diagnostics.Stopwatch]::StartNew()
        # One restore graph replaces DocFX's separate restore for each project.
        $restoreSolution = Join-Path ([IO.Path]::GetTempPath()) ("cuemon-docfx-{0}.slnx" -f [Guid]::NewGuid())
        try {
            $projectsXml = foreach ($project in $sourceProjects) {
                '  <Project Path="{0}" />' -f [Security.SecurityElement]::Escape($project)
            }
            @('<Solution>') + $projectsXml + @('</Solution>') | Set-Content -LiteralPath $restoreSolution -Encoding utf8
            dotnet restore $restoreSolution --verbosity quiet
            Assert-CommandSucceeded 'dotnet restore'
        } finally {
            if (Test-Path -LiteralPath $restoreSolution) { Remove-Item -LiteralPath $restoreSolution }
        }

        # Invalidate before generation so an interrupted or failed run cannot be reused.
        if (Test-Path -LiteralPath $cachePath) { Remove-Item -LiteralPath $cachePath }
        foreach ($file in $metadataFiles) { Remove-Item -LiteralPath $file }
        $generationFingerprint = Get-Fingerprint $inputPaths $inputValues
        # Keep all groups in one process; DocFX carries resolver state across groups.
        docfx metadata docfx.json --noRestore
        Assert-CommandSucceeded 'docfx metadata'
        $metadataFiles = @(Get-MetadataFiles)
        if ($metadataFiles.Count -eq 0) { throw 'DocFX did not generate metadata.' }
        if ((Get-Fingerprint $inputPaths $inputValues) -ne $generationFingerprint) {
            throw 'Metadata inputs changed during generation; rerun the build.'
        }
        $cache = @{
            inputs = $generationFingerprint
            outputs = Get-Fingerprint $metadataFiles
        }
        New-Item -ItemType Directory -Path (Split-Path -Parent $cachePath) -Force | Out-Null
        $cache | ConvertTo-Json | Set-Content -LiteralPath $cachePath -Encoding utf8
        Write-Host ('Metadata and restore: {0:N2}s' -f $metadataTimer.Elapsed.TotalSeconds)
    }

    $imageTimer = [Diagnostics.Stopwatch]::StartNew()
    docker buildx build -t cuemon-docfx:$version --platform linux/arm64,linux/amd64 --load -f Dockerfile.docfx .
    Assert-CommandSucceeded 'docker buildx build'
    Write-Host ('Docker image: {0:N2}s; total: {1:N2}s' -f $imageTimer.Elapsed.TotalSeconds, $totalTimer.Elapsed.TotalSeconds)
} finally {
    Pop-Location
}
