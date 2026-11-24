#requires -Version 7.0
[CmdletBinding()]
param(
    [string] $DefaultFramework = 'net11.0',
    [string[]] $Framework = @('net11.0', 'net10.0', 'netstandard2.1', 'netstandard2.0'),
    [string] $BuilderImage = 'codebeltnet/docfx:2.81.0@sha256:9645aaf2a361f1644585109bdaaa1fc65480b2c4d5d33a4777583eb07f9831af',
    [string] $OutputRoot = (Join-Path ([IO.Path]::GetTempPath()) "cuemon-docfx-native-$([guid]::NewGuid().ToString('N'))"),
    [string] $MetadataCache,
    [string] $MetadataCacheKey,
    [switch] $ForceMetadata,
    [switch] $BuildInContainer,
    [switch] $TestSharedImplementation
)

$ErrorActionPreference = 'Stop'
$repo = (Resolve-Path -LiteralPath "$PSScriptRoot/../..").Path
$source = "$repo/.docfx"

function Expand-PinnedArchive([string] $Repository, [string] $Revision, [string] $Workspace) {
    $archive = Join-Path $Workspace "$Repository.zip"
    Invoke-WebRequest -Uri "https://codeload.github.com/codebeltnet/$Repository/zip/$Revision" -OutFile $archive
    Expand-Archive -LiteralPath $archive -DestinationPath $Workspace
    Join-Path $Workspace "$Repository-$Revision"
}

# Use the authored inputs for the two-project fixture. The shared script owns discovery and generation.
$config = Get-Content -LiteralPath "$source/docfx.json" -Raw | ConvertFrom-Json -AsHashtable
$group = @($config.metadata | Where-Object dest -CEQ 'api/dotnet')
if ($group.Count -ne 1 -or $group[0].src.Count -ne 1) { throw 'Expected one api/dotnet metadata group with one source mapping.' }
$config.metadata = $group
$mapping = $group[0].src[0]
$mapping.src = [IO.Path]::GetFullPath($mapping.src, $source)
$mapping.files = @($mapping.files | Where-Object { $_ -cmatch '^Cuemon\.(Kernel|IO)/' })
if ($mapping.files.Count -ne 2) { throw 'Expected the authored Cuemon.Kernel and Cuemon.IO project mappings.' }
if (!(Test-Path -LiteralPath "$repo/cuemon.snk" -PathType Leaf)) {
    $group[0].properties = @{ SkipSignAssembly = 'true' }
}
$config.build.Remove('sitemap')
$config.build.Remove('xref')
$config.build.globalMetadata.Remove('_googleAnalyticsTagId')
$config.build.content = @(@{ files = @('api/dotnet/**/*.yml', 'index.md', 'toc.yml') })

$temporary = [IO.Path]::GetFullPath([IO.Path]::GetTempPath())
$workspace = Join-Path $temporary "cuemon-docfx-input-$([guid]::NewGuid().ToString('N'))"
$inputRoot = Join-Path $workspace 'input'
New-Item -ItemType Directory -Path "$inputRoot/api" -Force | Out-Null
try {
    $shared = Expand-PinnedArchive 'docfx-oci-build' 'fc1d3f50550499691b1c0efa35e8795a44028fcb' $workspace
    # The published builder has no framework selector yet. Use this override only for the fixture.
    $templates = Expand-PinnedArchive 'docfx' '06e35071e26f0a35a876ea963767c6216449532f' $workspace
    $sharedTemplatePath = Join-Path $templates 'templates/ms-style'
    if ($TestSharedImplementation) {
        & "$shared/tests/Test-FrameworkViews.ps1" -SharedTemplatePath $sharedTemplatePath
    }

    Copy-Item -LiteralPath "$source/includes", "$source/images", "$source/templates", "$source/index.md", "$source/$($group[0].filter)" -Destination $inputRoot -Recurse
    Copy-Item -LiteralPath "$source/api/namespaces", "$source/api/types" -Destination "$inputRoot/api" -Recurse
    @'
- name: Concept Reference
  href: index.md
- name: .NET API
  href: api/dotnet/Cuemon.IO.yml
'@ | Set-Content -LiteralPath "$inputRoot/toc.yml"
    ConvertTo-Json -InputObject $config -Depth 100 | Set-Content -LiteralPath "$inputRoot/docfx.json"
    & "$source/BuildRunActions.ps1" -OutputRoot "$inputRoot/run-actions"
    & "$shared/scripts/Build-DocfxFrameworkViews.ps1" -DocfxConfig "$inputRoot/docfx.json" -OutputRoot $OutputRoot -ApiPath $group[0].dest `
        -DefaultFramework $DefaultFramework -Framework $Framework -AlignAssemblyVersion `
        -SharedTemplatePath $sharedTemplatePath -BuilderImage $BuilderImage -BuildInContainer:$BuildInContainer `
        -MetadataCache $MetadataCache -MetadataCacheKey $MetadataCacheKey -ForceMetadata:$ForceMetadata
    & "$source/TestRunActions.ps1" -ScriptsRoot "$inputRoot/run-actions" -SiteRoot (Join-Path $OutputRoot 'site')
} finally {
    $resolvedWorkspace = (Resolve-Path -LiteralPath $workspace).Path
    if ([IO.Path]::GetRelativePath($temporary, $resolvedWorkspace) -match '^cuemon-docfx-input-[a-f0-9]{32}$') {
        Remove-Item -LiteralPath $resolvedWorkspace -Recurse -Force
    }
}
