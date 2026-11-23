#requires -Version 7.0
[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'
$PSNativeCommandUseErrorActionPreference = $false
$repo = (Resolve-Path (Join-Path $PSScriptRoot '../..')).Path
$docfxRoot = Join-Path $repo '.docfx'
$work = Join-Path ([IO.Path]::GetTempPath()) "cuemon-docfx-poc-$([Guid]::NewGuid().ToString('N'))"
$frameworks = @('net10.0', 'net11.0', 'netstandard2.0')
$projects = @('Cuemon.Kernel/Cuemon.Kernel.csproj', 'Cuemon.IO/Cuemon.IO.csproj')
$legacyType = 'System.Runtime.CompilerServices.CallerArgumentExpressionAttribute'
$modernType = 'Cuemon.IO.BufferWriterOptions'

function Assert-True([bool] $Condition, [string] $Message) {
    if (-not $Condition) { throw $Message }
}

function Invoke-Logged([string] $Log, [string] $Command, [string[]] $Arguments, [string] $ExpectedFailure) {
    & $Command @Arguments *> $Log
    $code = $LASTEXITCODE
    if ($ExpectedFailure) {
        Assert-True ($code -ne 0 -and (Get-Content -LiteralPath $Log -Raw).Contains($ExpectedFailure)) "Expected failure '$ExpectedFailure'. See $Log"
    } elseif ($code -ne 0) {
        Get-Content -LiteralPath $Log -Tail 30 | Write-Host
        throw "$Command failed with exit code $code. See $Log"
    }
}

function Read-Manifest([string] $Directory) {
    Get-Content -LiteralPath (Join-Path $Directory '.manifest') -Raw | ConvertFrom-Json -AsHashtable
}

function Get-TreeFingerprint([string] $Directory) {
    $entries = Get-ChildItem -LiteralPath $Directory -Recurse -File -Force | ForEach-Object {
        $name = [IO.Path]::GetRelativePath($Directory, $_.FullName).Replace('\', '/')
        if ($name -ceq 'manifest.json') {
            # DocFX records the absolute build directory and emits file entries in parallel completion order.
            $manifest = Get-Content -LiteralPath $_.FullName -Raw | ConvertFrom-Json -AsHashtable
            $manifest.Remove('source_base_path')
            [string[]] $files = @($manifest.files | ForEach-Object { ConvertTo-Json -InputObject $_ -Depth 30 -Compress })
            [Array]::Sort($files, [StringComparer]::Ordinal)
            $manifest.files = $files
            $bytes = [Text.Encoding]::UTF8.GetBytes((ConvertTo-Json -InputObject $manifest -Depth 30 -Compress))
            "$name $([Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($bytes)))"
        } else {
            "$name $((Get-FileHash -LiteralPath $_.FullName).Hash)"
        }
    }
    [string[]] $entries = @($entries)
    [Array]::Sort($entries, [StringComparer]::Ordinal)
    $entries -join "`n"
}

function New-Metadata([string] $Directory, [string] $Name, [string] $Source, [string[]] $Files, [string] $Framework) {
    $job = @{ src = @(@{ src = $Source; files = $Files }); dest = $Name; memberLayout = 'separatePages'; filter = 'filterConfig.yml' }
    if ($Framework) { $job.properties = @{ TargetFramework = $Framework } }
    $config = Join-Path $Directory "$Name.json"
    @{ metadata = @($job) } | ConvertTo-Json -Depth 10 | Set-Content -LiteralPath $config
    $log = Join-Path $Directory "$Name.log"
    Invoke-Logged $log docfx @('metadata', $config, '--noRestore')
    foreach ($line in [IO.File]::ReadLines($log)) {
        if ($line -notmatch '^warning:') { continue }
        $known = $Framework -eq 'netstandard2.0' -and
            $line -match '^warning: \[Warning\] Found project reference without a matching metadata reference: .*[/\\]Cuemon\.(Kernel|Core|Threading)\.csproj$'
        Assert-True $known "Unexpected DocFX metadata warning in ${log}: $line"
    }
}

function Test-RenderedSite([string] $Site, $Inventory, [string[]] $ModernMembers) {
    $xrefPath = Join-Path $Site 'xrefmap.yml'
    $xref = [Collections.Generic.Dictionary[string,string]]::new([StringComparer]::Ordinal)
    # DocFX's XRefMap writes each UID and its href as top-level fields on the same entry.
    $uid = $null
    foreach ($line in [IO.File]::ReadLines($xrefPath)) {
        if ($line -cmatch '^- uid: (.+)$') { $uid = $Matches[1] }
        if ($line -cmatch '^  href: (.+)$' -and $uid) { $xref.Add($uid, $Matches[1]); $uid = $null }
    }

    $html = @{}
    foreach ($uid in @($Inventory.Keys) + @('Cuemon.IO.StreamFactory.Create*')) {
        Assert-True ($xref.ContainsKey($uid)) "Rendered xref missing: $uid"
        $page, $anchor = $xref[$uid].Split('#', 2)
        Assert-True ($page.StartsWith('api/dotnet/')) "Unexpected xref destination for $uid : $page"
        if (-not $html.ContainsKey($page)) { $html[$page] = Get-Content -LiteralPath (Join-Path $Site $page) -Raw }
        if ($anchor) {
            $id = [Net.WebUtility]::HtmlEncode([Uri]::UnescapeDataString($anchor))
            Assert-True ($html[$page].Contains("id=`"$id`"")) "Broken rendered anchor for $uid : $anchor"
        }
    }

    $legacy = $html[$xref[$legacyType]]
    $modern = $html[$xref[$modernType]]
    $modernLabel = '<strong>Available in documented targets:</strong> <code>net10.0</code>, <code>net11.0</code>.'
    Assert-True ($legacy.Contains('<strong>Available in documented targets:</strong> <code>netstandard2.0</code>.')) 'Incorrect rendered compatibility availability.'
    Assert-True ($modern.Contains($modernLabel)) 'Incorrect rendered modern type availability.'
    foreach ($uid in $ModernMembers) {
        $page, $anchor = $xref[$uid].Split('#', 2)
        $id = [Net.WebUtility]::HtmlEncode([Uri]::UnescapeDataString($anchor))
        $section = [regex]::Match($html[$page], '(?s)<h[1-6][^>]*id="' + [regex]::Escape($id) + '"[^>]*>.*?(?=<h[1-6][^>]*data-uid=|\z)').Value
        Assert-True ($section.Contains($modernLabel) -and $section.Contains('href="Cuemon.IO.BufferWriterOptions.html"')) "Modern overload missing its declaration, link or availability: $uid"
    }

    $toc = Get-Content -LiteralPath (Join-Path $Site 'api/dotnet/toc.html') -Raw
    foreach ($type in @($legacyType, $modernType)) {
        $namespace = $type.Substring(0, $type.LastIndexOf('.'))
        Assert-True ($toc.Contains("href=`"$type.html`"") -and $toc.Contains("href=`"$namespace.html`"")) "Missing rendered TOC entry for $type"
        Assert-True ($html[$xref[$namespace]].Contains("href=`"$type.html`"")) "Missing namespace link for $type"
        Assert-True ($html[$xref[$type]].Contains('.cs/#L')) "Missing Git source link for $type"
    }
    Assert-True ($legacy.Contains('CallerArgumentExpressionAttributeExample')) 'Compatibility overwrite example is missing.'
    Assert-True ($modern.Contains('BufferWriterOptionsExample')) 'Modern overwrite example is missing.'
    Assert-True ($html[$xref['Cuemon.IO.StreamFactory']].Contains('Create a stream by writing to a StreamWriter')) 'StreamFactory overwrite example is missing.'
    Assert-True ($html[$xref['System.Runtime.CompilerServices']].Contains('provides polyfill types')) 'Compatibility namespace overwrite is missing.'
    Assert-True ($html[$xref['Cuemon.ArgumentReservedKeywordException']].Contains('Inherited members in netstandard2.0')) 'Legacy inheritance annotation is missing.'
    $overwrites = Join-Path (Split-Path -Parent $Site) 'api/types'
    foreach ($file in Get-ChildItem -LiteralPath $overwrites -Filter '*.md') {
        $markdown = Get-Content -LiteralPath $file.FullName -Raw
        if ($markdown -notmatch '(?m)^example:') { continue }
        $page = $xref[$file.BaseName]
        Assert-True ($html[$page] -match '(?s)<h2 id="[^"]+_examples">Examples</h2>.*?<pre><code[^>]*>\s*\S') "Authored example did not render for $($file.BaseName)."
    }
}

New-Item -ItemType Directory -Path $work | Out-Null
Write-Host "Artifacts: $work"
$version = & docfx --version
Assert-True ($LASTEXITCODE -eq 0 -and $version -match '^2\.81\.0(?:\+|$)') "Expected DocFX 2.81.0, got $version"
foreach ($project in $projects) {
    $path = Join-Path "$repo/src" $project
    $targets = & dotnet msbuild $path -getProperty:TargetFrameworks
    Assert-True ($LASTEXITCODE -eq 0) "MSBuild target framework discovery failed for $project"
    foreach ($framework in $frameworks) {
        Assert-True ($framework -cin $targets.Split(';')) "$project does not declare $framework."
    }
    Write-Host "$project declares $targets"
}
Invoke-Logged "$work/restore.log" dotnet @('restore', (Join-Path "$repo/src" $projects[1]))
& "$docfxRoot/BuildRunActions.ps1" -OutputRoot "$work/run-actions"
& "$docfxRoot/TestRunActions.ps1" -ScriptsRoot "$work/run-actions"
Copy-Item -LiteralPath "$PSScriptRoot/Consolidate.cs" -Destination $work

foreach ($round in 1, 2) {
    $stage = Join-Path $work "round-$round"
    New-Item -ItemType Directory -Path "$stage/api" | Out-Null
    Copy-Item -LiteralPath "$docfxRoot/filterConfig.yml", "$docfxRoot/includes", "$docfxRoot/templates", "$docfxRoot/images", "$work/run-actions" -Destination $stage -Recurse
    $source = @{}
    $output = "$stage/api/dotnet"
    $arguments = @('run', '--file', "$work/Consolidate.cs", '--', $output)
    foreach ($framework in $frameworks) {
        New-Metadata $stage $framework "$repo/src" $projects $framework
        $source[$framework] = Read-Manifest "$stage/$framework"
        $arguments += @($framework, "$stage/$framework")
    }
    Invoke-Logged "$stage/consolidation.log" dotnet $arguments
    Get-Content -LiteralPath "$stage/consolidation.log" | Write-Host
    $actual = Read-Manifest $output
    $expected = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
    foreach ($framework in $frameworks) {
        $expected.UnionWith([string[]] @($source[$framework].Keys))
        Assert-True ($source[$framework].Contains('Cuemon.Validator') -and $source[$framework].Contains('Cuemon.IO.StreamFactory')) "Expected both projects in $framework metadata."
        Assert-True ($source[$framework].Contains($legacyType) -eq ($framework -eq 'netstandard2.0')) "Unexpected source availability for $legacyType in $framework."
        Assert-True ($source[$framework].Contains($modernType) -eq ($framework -ne 'netstandard2.0')) "Unexpected source availability for $modernType in $framework."
    }
    Assert-True ($expected.SetEquals([string[]] @($actual.Keys))) 'Consolidated inventory differs from the source union.'
    $modernMembers = @($actual.Keys.Where({ $_ -clike 'Cuemon.IO.StreamFactory.Create*' -and $_.Contains('System.Buffers.IBufferWriter{System.Byte}') }))
    Assert-True ($modernMembers.Count -eq 6) 'Expected six IBufferWriter<byte> overloads.'
    foreach ($uid in $modernMembers) {
        Assert-True ($source['net10.0'].Contains($uid) -and $source['net11.0'].Contains($uid) -and -not $source['netstandard2.0'].Contains($uid)) "Unexpected source availability for $uid"
    }

    foreach ($kind in 'types', 'namespaces') {
        New-Item -ItemType Directory -Path "$stage/api/$kind" | Out-Null
        Get-ChildItem -LiteralPath "$docfxRoot/api/$kind" -Filter '*.md' |
            Where-Object { $actual.Contains($_.BaseName) } | Copy-Item -Destination "$stage/api/$kind"
    }
    $config = Get-Content -LiteralPath "$docfxRoot/docfx.json" -Raw | ConvertFrom-Json -AsHashtable
    $config.Remove('metadata')
    $config.build.Remove('sitemap')
    $config.build.content = @(@{ files = @('api/dotnet/*.yml') })
    $config | ConvertTo-Json -Depth 30 | Set-Content -LiteralPath "$stage/docfx.json"
    Invoke-Logged "$stage/build.log" docfx @('build', "$stage/docfx.json", '--warningsAsErrors')
    Test-RenderedSite "$stage/wwwroot" $actual $modernMembers
}

foreach ($directory in 'api/dotnet', 'wwwroot') {
    $first = Get-TreeFingerprint "$work/round-1/$directory"
    $second = Get-TreeFingerprint "$work/round-2/$directory"
    if ($first -cne $second) {
        Compare-Object ($first -split "`n") ($second -split "`n") | Write-Host
        throw "Fresh runs differ in $directory."
    }
    $first | Set-Content -LiteralPath (Join-Path $work ($directory.Replace('/', '-') + '.sha256'))
}

$single = "$work/single"
New-Item -ItemType Directory -Path $single | Out-Null
Copy-Item -LiteralPath "$docfxRoot/filterConfig.yml" -Destination $single
'<Project Sdk="Microsoft.NET.Sdk"><PropertyGroup><TargetFramework>net10.0</TargetFramework></PropertyGroup></Project>' |
    Set-Content -LiteralPath "$single/Single.csproj"
'namespace Fixture; public class Single { public int GetValue() { return 42; } }' | Set-Content -LiteralPath "$single/Single.cs"
Invoke-Logged "$single/restore.log" dotnet @('restore', "$single/Single.csproj")
New-Metadata $single 'normal' $single @('Single.csproj')
Invoke-Logged "$single/copy.log" dotnet @('run', '--file', "$work/Consolidate.cs", '--', "$single/copied", 'net10.0', "$single/normal")
Assert-True ((Get-TreeFingerprint "$single/normal") -ceq (Get-TreeFingerprint "$single/copied")) 'Single-TFM metadata changed.'

$first = "$work/round-1"
$bad = "$work/negative"
Copy-Item -LiteralPath "$first/net11.0" -Destination $bad -Recurse
$page = "$bad/Cuemon.IO.StreamFactory.yml"
$original = Get-Content -LiteralPath $page -Raw
$modified = $original.Replace('content: public static class StreamFactory', 'content: public sealed class StreamFactory')
Assert-True ($original -cne $modified) 'Signature conflict fixture did not change the syntax.'
$modified | Set-Content -LiteralPath $page -NoNewline
$negative = @('run', '--file', "$work/Consolidate.cs", '--', "$work/rejected", 'net10.0', "$first/net10.0", 'net11.0', $bad)
Invoke-Logged "$work/signature-conflict.log" dotnet $negative "Cuemon.IO.StreamFactory (net11.0): conflicting 'syntax'"
$original | Set-Content -LiteralPath $page -NoNewline
Remove-Item -LiteralPath $page
Invoke-Logged "$work/missing-api.log" dotnet $negative 'generated pages disagree with the DocFX manifest'
Assert-True (-not (Test-Path -LiteralPath "$work/rejected")) 'Unsafe input produced an output directory.'
Write-Host 'Passed availability, UID coverage, links, overwrites, reproducibility, single-TFM passthrough and failure checks.'
