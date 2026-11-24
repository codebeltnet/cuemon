#requires -Version 7.0
<#
.SYNOPSIS
Validates Cuemon DocFX run-action manifest integrity, Markdown references,
generated script completeness, .dockerignore rules, determinism, rendered
DocFX site output, and exported OCI image archives.
#>
[CmdletBinding()]
param(
    [string] $DocfxRoot = $PSScriptRoot,
    [string] $ManifestPath = (Join-Path $DocfxRoot 'run-actions.manifest.json'),
    [string] $ScriptsRoot = (Join-Path $DocfxRoot 'run-actions'),
    [string] $SiteRoot = '',
    [string] $OciArchivePath = '',
    [switch] $CheckDeterminism
)

$ErrorActionPreference = 'Stop'
$PSNativeCommandUseErrorActionPreference = $false
Set-StrictMode -Version Latest

function Add-Failure([System.Collections.Generic.List[string]] $Errors, [string] $Message) {
    $Errors.Add($Message)
}

function Test-NetFrameworkTfm([string] $Framework) {
    return (-not [string]::IsNullOrWhiteSpace($Framework) -and $Framework -match '^net4\d+$')
}

function Get-ExpectedRunActionMap([string] $Path, [System.Collections.Generic.List[string]] $Errors) {
    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) {
        throw "Run-actions manifest not found: $Path"
    }

    $doc = Get-Content -LiteralPath $Path -Raw | ConvertFrom-Json -AsHashtable
    if (-not ($doc -is [System.Collections.IDictionary])) {
        throw "Malformed run-actions manifest at ${Path}: root JSON value must be an object."
    }
    if (-not $doc.ContainsKey('schemaVersion') -or $doc['schemaVersion'] -ne 1) {
        throw "Unsupported run-actions manifest schemaVersion: $($doc['schemaVersion'])"
    }
    if (-not $doc.ContainsKey('examples') -or $null -eq $doc['examples'] -or ($doc['examples'] -is [string]) -or ($doc['examples'] -is [System.Collections.IDictionary])) {
        throw "Malformed run-actions manifest at ${Path}: 'examples' must be an array."
    }

    $examples = @($doc['examples'])
    if ($examples.Count -eq 0) {
        throw "Run-actions manifest at $Path contains no examples."
    }

    $map = @{}
    foreach ($example in $examples) {
        if (-not ($example -is [System.Collections.IDictionary])) {
            Add-Failure $Errors 'Manifest contains a non-object example entry.'
            continue
        }
        $id = [string] $example['id']
        if ($id -cnotmatch '^[a-f0-9]{24}$') {
            Add-Failure $Errors "Invalid run-action id in manifest: '$id'"
            continue
        }
        if ($map.ContainsKey($id)) {
            Add-Failure $Errors "Duplicate run-action id in manifest: '$id'"
            continue
        }

        $strategy = [string] $example['strategy']
        if ($strategy -cnotin @('standalone', 'repository', 'host')) {
            Add-Failure $Errors "Unsupported strategy '$strategy' for manifest entry '$id'."
            continue
        }

        if ($strategy -ceq 'standalone') {
            if (-not $example.ContainsKey('manifest') -or -not ($example['manifest'] -is [System.Collections.IDictionary]) -or
                -not $example['manifest'].ContainsKey('mode') -or [string]::IsNullOrWhiteSpace([string]$example['manifest']['mode']) -or
                -not $example['manifest'].ContainsKey('source') -or [string]::IsNullOrWhiteSpace([string]$example['manifest']['source'])) {
                Add-Failure $Errors "Malformed standalone manifest payload for '$id'."
                continue
            }
        } elseif ($strategy -ceq 'repository') {
            foreach ($field in @('repository', 'commit', 'project', 'test', 'framework', 'image')) {
                if (-not $example.ContainsKey($field) -or [string]::IsNullOrWhiteSpace([string]$example[$field])) {
                    Add-Failure $Errors "Missing '$field' in repository manifest entry '$id'."
                }
            }
        } elseif ($strategy -ceq 'host') {
            if (-not $example.ContainsKey('source') -or [string]::IsNullOrWhiteSpace([string]$example['source'])) {
                Add-Failure $Errors "Missing 'source' in host manifest entry '$id'."
                continue
            }
        }

        $effectiveFramework = if ($strategy -ceq 'standalone' -and $example.ContainsKey('manifest') -and $example['manifest'] -and $example['manifest'].ContainsKey('framework') -and $example['manifest']['framework']) {
            [string] $example['manifest']['framework']
        } elseif ($strategy -ceq 'host' -and $example.ContainsKey('hostHarness') -and $example['hostHarness'] -and $example['hostHarness'].ContainsKey('framework') -and $example['hostHarness']['framework']) {
            [string] $example['hostHarness']['framework']
        } elseif ($example.ContainsKey('framework') -and $example['framework']) {
            [string] $example['framework']
        } else {
            ''
        }
        $isNetFramework = Test-NetFrameworkTfm $effectiveFramework

        if (-not $example.ContainsKey('transports') -or $null -eq $example['transports'] -or ($example['transports'] -is [string])) {
            Add-Failure $Errors "Missing or invalid 'transports' array for '$id'."
            continue
        }

        $expectedTransports = [System.Collections.Generic.List[string]]::new()
        $seenTransports = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::Ordinal)
        foreach ($tVal in @($example['transports'])) {
            $t = [string] $tVal
            if ($t -cnotin @('native', 'docker')) {
                Add-Failure $Errors "Unsupported transport '$t' for '$id'."
                continue
            }
            if (-not $seenTransports.Add($t)) {
                Add-Failure $Errors "Duplicate transport '$t' for '$id'."
                continue
            }
            if ($isNetFramework -and $t -ceq 'docker') { continue }
            $expectedTransports.Add($t)
        }

        if ($expectedTransports.Count -eq 0) {
            Add-Failure $Errors "No active transports remain for '$id'."
            continue
        }

        $map[$id] = @($expectedTransports)
    }

    return $map
}

function Get-DirectoryFileHashes([string] $Root) {
    $hashes = [ordered]@{}
    $fullRoot = [IO.Path]::GetFullPath($Root).TrimEnd([IO.Path]::DirectorySeparatorChar, [IO.Path]::AltDirectorySeparatorChar)
    foreach ($file in (Get-ChildItem -LiteralPath $fullRoot -Recurse -File | Sort-Object FullName)) {
        $rel = $file.FullName.Substring($fullRoot.Length + 1).Replace('\', '/')
        $hashes[$rel] = (Get-FileHash -LiteralPath $file.FullName -Algorithm SHA256).Hash
    }
    return $hashes
}

function Resolve-RelativeWebPath([string] $BaseRelDir, [string] $Href) {
    $combined = if ($Href.StartsWith('/')) {
        $Href.TrimStart('/')
    } elseif ([string]::IsNullOrEmpty($BaseRelDir)) {
        $Href
    } else {
        "$BaseRelDir/$Href"
    }

    $stack = [System.Collections.Generic.List[string]]::new()
    foreach ($segment in $combined.Split('/')) {
        if ([string]::IsNullOrEmpty($segment) -or $segment -eq '.') { continue }
        if ($segment -eq '..') {
            if ($stack.Count -eq 0) { return $null }
            $stack.RemoveAt($stack.Count - 1)
        } else {
            $stack.Add($segment)
        }
    }
    return ($stack -join '/')
}

function Test-OciArchiveAssets([string] $ArchivePath, [System.Collections.Generic.HashSet[string]] $ExpectedScripts, [System.Collections.Generic.List[string]] $Errors) {
    if (-not (Test-Path -LiteralPath $ArchivePath -PathType Leaf)) {
        Add-Failure $Errors "OCI archive not found: $ArchivePath"
        return
    }

    $blobs = @{}
    $archiveStream = [IO.File]::OpenRead($ArchivePath)
    try {
        $tarReader = [System.Formats.Tar.TarReader]::new($archiveStream, $true)
        try {
            while ($null -ne ($entry = $tarReader.GetNextEntry())) {
                if ($entry.EntryType -ne [System.Formats.Tar.TarEntryType]::RegularFile -and
                    $entry.EntryType -ne [System.Formats.Tar.TarEntryType]::V7RegularFile) {
                    continue
                }
                $name = $entry.Name.Replace('\', '/').TrimStart('./')
                if ($name -ceq 'index.json' -or $name.StartsWith('blobs/sha256/', [StringComparison]::Ordinal)) {
                    $ms = [IO.MemoryStream]::new()
                    if ($null -ne $entry.DataStream) {
                        $entry.DataStream.CopyTo($ms)
                    }
                    $blobs[$name] = $ms.ToArray()
                    $ms.Dispose()
                }
            }
        } finally {
            $tarReader.Dispose()
        }
    } finally {
        $archiveStream.Dispose()
    }

    if (-not $blobs.ContainsKey('index.json')) {
        Add-Failure $Errors "OCI archive '$ArchivePath' is missing index.json."
        return
    }

    $indexDoc = [Text.Encoding]::UTF8.GetString($blobs['index.json']) | ConvertFrom-Json -AsHashtable
    $manifestRefs = @($indexDoc['manifests'])
    if ($manifestRefs.Count -eq 0) {
        Add-Failure $Errors "OCI archive '$ArchivePath' index.json contains no manifests."
        return
    }

    $platformManifestDigests = [System.Collections.Generic.List[string]]::new()
    foreach ($mRef in $manifestRefs) {
        $digest = [string] $mRef['digest']
        if ($digest -cnotmatch '^sha256:([a-f0-9]{64})$') { continue }
        $blobKey = "blobs/sha256/$($Matches[1])"
        if (-not $blobs.ContainsKey($blobKey)) {
            Add-Failure $Errors "OCI archive '$ArchivePath' is missing manifest blob '$blobKey'."
            continue
        }
        $mObj = [Text.Encoding]::UTF8.GetString($blobs[$blobKey]) | ConvertFrom-Json -AsHashtable
        if ($mObj.ContainsKey('manifests')) {
            foreach ($nested in @($mObj['manifests'])) {
                $p = $nested['platform']
                if ($p -and $p['os'] -ceq 'unknown' -and $p['architecture'] -ceq 'unknown') { continue }
                if ([string]$nested['digest'] -cmatch '^sha256:([a-f0-9]{64})$') {
                    $platformManifestDigests.Add($Matches[1])
                }
            }
        } elseif ($mObj.ContainsKey('layers')) {
            $p = $mRef['platform']
            if ($p -and $p['os'] -ceq 'unknown' -and $p['architecture'] -ceq 'unknown') { continue }
            $platformManifestDigests.Add($digest.Substring(7))
        }
    }

    if ($platformManifestDigests.Count -eq 0) {
        Add-Failure $Errors "OCI archive '$ArchivePath' contains no runnable platform manifests."
        return
    }

    foreach ($manifestHex in ($platformManifestDigests | Select-Object -Unique)) {
        $mBlobKey = "blobs/sha256/$manifestHex"
        $manifestDoc = [Text.Encoding]::UTF8.GetString($blobs[$mBlobKey]) | ConvertFrom-Json -AsHashtable
        $files = @{}

        foreach ($layer in @($manifestDoc['layers'])) {
            $layerDigest = [string] $layer['digest']
            if ($layerDigest -cnotmatch '^sha256:([a-f0-9]{64})$') { continue }
            $layerKey = "blobs/sha256/$($Matches[1])"
            if (-not $blobs.ContainsKey($layerKey)) {
                Add-Failure $Errors "OCI archive '$ArchivePath' is missing layer blob '$layerKey' for manifest '$manifestHex'."
                continue
            }

            $layerBytes = $blobs[$layerKey]
            $mediaType = [string] $layer['mediaType']
            $isGzip = $mediaType -like '*gzip*' -or ($layerBytes.Length -ge 2 -and $layerBytes[0] -eq 0x1F -and $layerBytes[1] -eq 0x8B)
            $isZstd = $mediaType -like '*zstd*' -or ($layerBytes.Length -ge 4 -and $layerBytes[0] -eq 0x28 -and $layerBytes[1] -eq 0xB5 -and $layerBytes[2] -eq 0x2F -and $layerBytes[3] -eq 0xFD)

            $layerStream = [IO.MemoryStream]::new($layerBytes, $false)
            $decompressedTemp = $null
            $tarInput = $null
            if ($isGzip) {
                $tarInput = [IO.Compression.GzipStream]::new($layerStream, [IO.Compression.CompressionMode]::Decompress)
            } elseif ($isZstd) {
                $zstdType = 'System.IO.Compression.ZstandardStream' -as [type]
                if ($null -ne $zstdType) {
                    $tarInput = $zstdType::new($layerStream, [IO.Compression.CompressionMode]::Decompress)
                } elseif (Get-Command zstd -ErrorAction SilentlyContinue) {
                    $tmpIn = [IO.Path]::GetTempFileName()
                    $decompressedTemp = [IO.Path]::GetTempFileName()
                    try {
                        [IO.File]::WriteAllBytes($tmpIn, $layerBytes)
                        & zstd -d -f -q $tmpIn -o $decompressedTemp
                        if ($LASTEXITCODE -ne 0) { throw "zstd decompression failed for layer '$layerKey'." }
                        $tarInput = [IO.File]::OpenRead($decompressedTemp)
                    } finally {
                        if (Test-Path -LiteralPath $tmpIn) { Remove-Item -LiteralPath $tmpIn -Force }
                    }
                } else {
                    $tmpTar = [IO.Path]::GetTempFileName()
                    try {
                        [IO.File]::WriteAllBytes($tmpTar, $layerBytes)
                        $tarEntries = @(& tar -tvf $tmpTar 2>&1)
                        if ($LASTEXITCODE -ne 0) { throw "Failed to inspect zstd layer '$layerKey' with tar: $($tarEntries -join ' ')" }
                        foreach ($line in $tarEntries) {
                            if ($line -match '^[^d]\S*\s+\S+\s+(?<size>\d+)\s+.+?\s+(?<path>\S+)$') {
                                $entryPath = $Matches['path'].Replace('\', '/').TrimStart('./')
                                $files[$entryPath] = [long]$Matches['size']
                            }
                        }
                    } finally {
                        if (Test-Path -LiteralPath $tmpTar) { Remove-Item -LiteralPath $tmpTar -Force }
                    }
                    $layerStream.Dispose()
                    continue
                }
            } else {
                $tarInput = $layerStream
            }
            try {
                $layerReader = [System.Formats.Tar.TarReader]::new($tarInput, $true)
                try {
                    while ($null -ne ($entry = $layerReader.GetNextEntry())) {
                        $entryPath = $entry.Name.Replace('\', '/').TrimStart('./')
                        $entryDir = [IO.Path]::GetDirectoryName($entryPath)?.Replace('\', '/')
                        $entryLeaf = [IO.Path]::GetFileName($entryPath)
                        if ($entryLeaf -ceq '.wh..wh..opq' -and $entryDir) {
                            $prefix = "$entryDir/"
                            foreach ($existing in @($files.Keys | Where-Object { $_.StartsWith($prefix, [StringComparison]::Ordinal) })) {
                                $files.Remove($existing)
                            }
                            continue
                        }
                        if ($entryLeaf.StartsWith('.wh.', [StringComparison]::Ordinal)) {
                            $targetName = $entryLeaf.Substring(4)
                            $targetPath = if ($entryDir) { "$entryDir/$targetName" } else { $targetName }
                            $files.Remove($targetPath)
                            $dirPrefix = "$targetPath/"
                            foreach ($existing in @($files.Keys | Where-Object { $_.StartsWith($dirPrefix, [StringComparison]::Ordinal) })) {
                                $files.Remove($existing)
                            }
                            continue
                        }
                        if ($entry.EntryType -eq [System.Formats.Tar.TarEntryType]::RegularFile -or
                            $entry.EntryType -eq [System.Formats.Tar.TarEntryType]::V7RegularFile) {
                            $files[$entryPath] = $entry.Length
                        }
                    }
                } finally {
                    $layerReader.Dispose()
                }
            } finally {
                if ($null -ne $tarInput -and $tarInput -ne $layerStream) { $tarInput.Dispose() }
                $layerStream.Dispose()
                if ($decompressedTemp -and (Test-Path -LiteralPath $decompressedTemp)) {
                    Remove-Item -LiteralPath $decompressedTemp -Force
                }
            }
        }

        $jsPath = 'usr/share/nginx/html/public/run-actions.js'
        if (-not $files.ContainsKey($jsPath) -or $files[$jsPath] -le 0) {
            Add-Failure $Errors "OCI manifest '$manifestHex' is missing non-empty '$jsPath'."
        }

        $missingInLayer = 0
        foreach ($relScript in $ExpectedScripts) {
            $ociPath = "usr/share/nginx/html/run-actions/$relScript"
            if (-not $files.ContainsKey($ociPath) -or $files[$ociPath] -le 0) {
                $missingInLayer++
                if ($missingInLayer -le 10) {
                    Add-Failure $Errors "OCI manifest '$manifestHex' is missing '$ociPath'."
                }
            }
        }
        if ($missingInLayer -gt 10) {
            Add-Failure $Errors "OCI manifest '$manifestHex' is missing $missingInLayer total run-action script(s)."
        }
    }
}

$errors = [System.Collections.Generic.List[string]]::new()
$docfxFullRoot = [IO.Path]::GetFullPath($DocfxRoot)
$expectedById = Get-ExpectedRunActionMap -Path $ManifestPath -Errors $errors

$expectedScripts = [System.Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
foreach ($id in $expectedById.Keys) {
    foreach ($transport in $expectedById[$id]) {
        $null = $expectedScripts.Add("$id/$transport.ps1")
    }
}

# Validate docfx.json and template files.
$docfxConfigPath = Join-Path $docfxFullRoot 'docfx.json'
if (-not (Test-Path -LiteralPath $docfxConfigPath -PathType Leaf)) {
    Add-Failure $errors "DocFX configuration not found: $docfxConfigPath"
} else {
    $docfxConfig = Get-Content -LiteralPath $docfxConfigPath -Raw | ConvertFrom-Json -AsHashtable
    $resourceFiles = @()
    if ($docfxConfig.ContainsKey('build') -and $docfxConfig['build'] -and $docfxConfig['build'].ContainsKey('resource')) {
        foreach ($res in @($docfxConfig['build']['resource'])) {
            $resourceFiles += @($res['files'])
        }
    }
    if ($resourceFiles -notcontains 'run-actions/**') {
        Add-Failure $errors "docfx.json build.resource must include 'run-actions/**'."
    }
}

$runActionsJsPath = Join-Path $docfxFullRoot 'templates/cuemon/public/run-actions.js'
if (-not (Test-Path -LiteralPath $runActionsJsPath -PathType Leaf) -or (Get-Item -LiteralPath $runActionsJsPath).Length -eq 0) {
    Add-Failure $errors "Missing or empty template asset: $runActionsJsPath"
}
$mainJsPath = Join-Path $docfxFullRoot 'templates/cuemon/public/main.js'
if (-not (Test-Path -LiteralPath $mainJsPath -PathType Leaf) -or
    ([IO.File]::ReadAllText($mainJsPath) -notmatch "import\s+'\./run-actions\.js'")) {
    Add-Failure $errors "Template entry script templates/cuemon/public/main.js must import './run-actions.js'."
}

# Validate .dockerignore does not exclude nested run-actions/**/*.ps1 scripts.
$dockerignorePath = Join-Path $docfxFullRoot '.dockerignore'
if (Test-Path -LiteralPath $dockerignorePath -PathType Leaf) {
    $ignoreLines = @([IO.File]::ReadAllLines($dockerignorePath) | ForEach-Object { $_.Trim() } | Where-Object { $_ -and -not $_.StartsWith('#') })
    $excludesRunActionsDir = $ignoreLines | Where-Object { $_ -ceq 'run-actions/' -or $_ -ceq 'run-actions/**' -or $_ -ceq 'run-actions' }
    if ($excludesRunActionsDir) {
        Add-Failure $errors ".dockerignore must not exclude 'run-actions/'."
    }
    $hasWildcardPs1 = ($ignoreLines -contains '*.ps1') -or ($ignoreLines -contains '**/*.ps1')
    $hasNegation = $ignoreLines -contains '!run-actions/**/*.ps1'
    if ($hasWildcardPs1 -and -not $hasNegation) {
        Add-Failure $errors ".dockerignore ignores .ps1 files without explicitly preserving '!run-actions/**/*.ps1'."
    }
}

# Validate all Markdown run-action references against the manifest.
$excludedSegments = @('obj', 'wwwroot', 'packages', 'run-actions')
$mdFiles = @(
    Get-ChildItem -LiteralPath $docfxFullRoot -Recurse -File -Filter '*.md' |
        Where-Object {
            $rel = $_.FullName.Substring($docfxFullRoot.Length).TrimStart('\', '/').Replace('\', '/')
            $firstSegment = ($rel -split '/', 2)[0]
            $firstSegment -notin $excludedSegments
        }
)

$divRegex = [regex]'<div\s+class="docfx-run-actions"(?<attrs>[^>]*)>(?<inner>.*?)</div>'
$idAttrRegex = [regex]'data-example-id="(?<id>[^"]*)"'
$linkRegex = [regex]'<a\s+(?<attrs>[^>]*?)>'
$hrefAttrRegex = [regex]'href="(?<href>[^"]*)"'
$transportAttrRegex = [regex]'data-transport="(?<transport>[^"]*)"'
$referencedScripts = [System.Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)

foreach ($mdFile in $mdFiles) {
    $relMd = $mdFile.FullName.Substring($docfxFullRoot.Length).TrimStart('\', '/').Replace('\', '/')
    $content = [IO.File]::ReadAllText($mdFile.FullName)
    $cleaned = $divRegex.Replace($content, {
        param($match)
        $attrs = $match.Groups['attrs'].Value
        $inner = $match.Groups['inner'].Value
        $idMatch = $idAttrRegex.Match($attrs)
        if (-not $idMatch.Success) {
            Add-Failure $errors "${relMd}: docfx-run-actions block is missing data-example-id."
            return ''
        }
        $id = $idMatch.Groups['id'].Value
        if ($id -cnotmatch '^[a-f0-9]{24}$') {
            Add-Failure $errors "${relMd}: invalid data-example-id '$id'."
            return ''
        }
        if (-not $expectedById.ContainsKey($id)) {
            Add-Failure $errors "${relMd}: referenced run-action id '$id' is missing from $ManifestPath."
            return ''
        }

        $foundTransports = [System.Collections.Generic.List[string]]::new()
        foreach ($linkMatch in $linkRegex.Matches($inner)) {
            $linkAttrs = $linkMatch.Groups['attrs'].Value
            $hrefMatch = $hrefAttrRegex.Match($linkAttrs)
            $trMatch = $transportAttrRegex.Match($linkAttrs)
            if (-not $hrefMatch.Success -or -not $trMatch.Success) {
                Add-Failure $errors "${relMd} ($id): run-action anchor missing href or data-transport."
                continue
            }
            $href = $hrefMatch.Groups['href'].Value
            $tr = $trMatch.Groups['transport'].Value
            if ($tr -cnotin @('native', 'docker')) {
                Add-Failure $errors "${relMd} ($id): unsupported data-transport '$tr'."
                continue
            }
            $expectedHref = "~/run-actions/$id/$tr.ps1"
            if ($href -cne $expectedHref) {
                Add-Failure $errors "${relMd} ($id): expected href '$expectedHref' but found '$href'."
            }
            if ($foundTransports.Contains($tr)) {
                Add-Failure $errors "${relMd} ($id): duplicate data-transport '$tr'."
            } else {
                $foundTransports.Add($tr)
            }
            $null = $referencedScripts.Add("$id/$tr.ps1")
        }

        $expectedList = ($expectedById[$id] | Sort-Object) -join ','
        $foundList = ($foundTransports | Sort-Object) -join ','
        if ($expectedList -cne $foundList) {
            Add-Failure $errors "${relMd} ($id): transports [$foundList] do not match manifest [$expectedList]."
        }
        return ''
    })

    if ($cleaned -replace '<!--\s*docfx-run-actions:(?:start|end)\s*-->', '' -match 'docfx-run-actions|(?:~/|/)?run-actions/') {
        Add-Failure $errors "${relMd}: contains malformed docfx-run-actions markup or stray run-actions reference."
    }
}

if ($referencedScripts.Count -eq 0) {
    Add-Failure $errors "No run-action references were discovered in Markdown under $docfxFullRoot."
}

# Validate generated scripts under $ScriptsRoot.
$executionScriptRegex = [regex]'^& \{\r?\n#requires -Version 7\.0\r?\n[\s\S]*\r?\n\}\r?\n?$'
if (-not (Test-Path -LiteralPath $ScriptsRoot -PathType Container)) {
    Add-Failure $errors "Generated run-actions directory not found: $ScriptsRoot"
} else {
    $allRequired = [System.Collections.Generic.HashSet[string]]::new($expectedScripts, [StringComparer]::Ordinal)
    $allRequired.UnionWith($referencedScripts)

    foreach ($relScript in ($allRequired | Sort-Object)) {
        $scriptPath = Join-Path $ScriptsRoot $relScript
        if (-not (Test-Path -LiteralPath $scriptPath -PathType Leaf)) {
            Add-Failure $errors "Missing generated run-action script: $scriptPath"
            continue
        }
        $info = Get-Item -LiteralPath $scriptPath
        if ($info.Length -eq 0) {
            Add-Failure $errors "Generated run-action script is empty: $scriptPath"
            continue
        }
        $scriptText = [IO.File]::ReadAllText($scriptPath)
        if (-not $executionScriptRegex.IsMatch($scriptText)) {
            Add-Failure $errors "Generated run-action script does not match run-actions.js contract: $scriptPath"
        }
        $tokens = $null; $parseErrors = $null
        $null = [Management.Automation.Language.Parser]::ParseInput($scriptText, [ref]$tokens, [ref]$parseErrors)
        if ($parseErrors.Count -gt 0) {
            Add-Failure $errors "Generated run-action script has PowerShell syntax errors: $scriptPath"
        }
    }

    $scriptsFullRoot = [IO.Path]::GetFullPath($ScriptsRoot).TrimEnd([IO.Path]::DirectorySeparatorChar, [IO.Path]::AltDirectorySeparatorChar)
    foreach ($diskScript in (Get-ChildItem -LiteralPath $scriptsFullRoot -Recurse -File -Filter '*.ps1')) {
        $relDisk = $diskScript.FullName.Substring($scriptsFullRoot.Length + 1).Replace('\', '/')
        if (-not $expectedScripts.Contains($relDisk)) {
            Add-Failure $errors "Unexpected or stale generated script in ${ScriptsRoot}: $relDisk"
        }
    }
}

# Determinism check: generate twice from scratch into temp directories and compare hashes.
if ($CheckDeterminism) {
    $builderScript = Join-Path $docfxFullRoot 'BuildRunActions.ps1'
    if (-not (Test-Path -LiteralPath $builderScript -PathType Leaf)) {
        Add-Failure $errors "Generator script not found for determinism check: $builderScript"
    } else {
        $tempBase = Join-Path ([IO.Path]::GetTempPath()) ('dx-ra-det-' + [guid]::NewGuid().ToString('N'))
        $run1 = Join-Path $tempBase 'run1'
        $run2 = Join-Path $tempBase 'run2'
        try {
            & $builderScript -ManifestPath $ManifestPath -OutputRoot $run1 | Out-Null
            & $builderScript -ManifestPath $ManifestPath -OutputRoot $run2 | Out-Null

            $hashes1 = Get-DirectoryFileHashes $run1
            $hashes2 = Get-DirectoryFileHashes $run2

            if ($hashes1.Count -ne $expectedScripts.Count) {
                Add-Failure $errors "Determinism run1 produced $($hashes1.Count) files, expected $($expectedScripts.Count)."
            }
            if (($hashes1.Keys -join '|') -cne ($hashes2.Keys -join '|')) {
                Add-Failure $errors 'Determinism check failed: repeated runs produced different file sets.'
            } else {
                foreach ($key in $hashes1.Keys) {
                    if ($hashes1[$key] -cne $hashes2[$key]) {
                        Add-Failure $errors "Determinism check failed: non-deterministic content for '$key'."
                        break
                    }
                }
            }

            if (Test-Path -LiteralPath $ScriptsRoot -PathType Container) {
                $hashesLive = Get-DirectoryFileHashes $ScriptsRoot
                if (($hashes1.Keys -join '|') -cne ($hashesLive.Keys -join '|')) {
                    Add-Failure $errors "Generated directory '$ScriptsRoot' file set differs from clean generation."
                } else {
                    foreach ($key in $hashes1.Keys) {
                        if ($hashes1[$key] -cne $hashesLive[$key]) {
                            Add-Failure $errors "Generated file '$key' in '$ScriptsRoot' differs from clean generation."
                            break
                        }
                    }
                }
            }
        } finally {
            if (Test-Path -LiteralPath $tempBase) {
                Remove-Item -LiteralPath $tempBase -Recurse -Force
            }
        }
    }
}

# Validate published DocFX HTML output directory if provided.
if (-not [string]::IsNullOrWhiteSpace($SiteRoot)) {
    if (-not (Test-Path -LiteralPath $SiteRoot -PathType Container)) {
        Add-Failure $errors "Published site directory not found: $SiteRoot"
    } else {
        $siteFullRoot = [IO.Path]::GetFullPath($SiteRoot).TrimEnd([IO.Path]::DirectorySeparatorChar, [IO.Path]::AltDirectorySeparatorChar)
        $siteJs = Join-Path $siteFullRoot 'public/run-actions.js'
        if (-not (Test-Path -LiteralPath $siteJs -PathType Leaf) -or (Get-Item -LiteralPath $siteJs).Length -eq 0) {
            Add-Failure $errors "Published site is missing non-empty public/run-actions.js: $siteJs"
        }

        $siteScriptsRoot = Join-Path $siteFullRoot 'run-actions'
        if (-not (Test-Path -LiteralPath $siteScriptsRoot -PathType Container)) {
            Add-Failure $errors "Published site is missing run-actions directory: $siteScriptsRoot"
        } else {
            foreach ($relScript in ($expectedScripts | Sort-Object)) {
                $siteScriptPath = Join-Path $siteScriptsRoot $relScript
                if (-not (Test-Path -LiteralPath $siteScriptPath -PathType Leaf) -or (Get-Item -LiteralPath $siteScriptPath).Length -eq 0) {
                    Add-Failure $errors "Published site is missing generated script: run-actions/$relScript"
                } elseif (Test-Path -LiteralPath (Join-Path $ScriptsRoot $relScript) -PathType Leaf) {
                    $srcBytes = [IO.File]::ReadAllBytes((Join-Path $ScriptsRoot $relScript))
                    $siteBytes = [IO.File]::ReadAllBytes($siteScriptPath)
                    if (-not [System.Linq.Enumerable]::SequenceEqual($srcBytes, $siteBytes)) {
                        Add-Failure $errors "Published script differs from generated source script: run-actions/$relScript"
                    }
                }
            }
        }

        $htmlFiles = @(Get-ChildItem -LiteralPath $siteFullRoot -Recurse -File -Filter '*.html')
        if ($htmlFiles.Count -eq 0) {
            Add-Failure $errors "No HTML files found in published site: $siteFullRoot"
        }

        $frameworkManifest = Join-Path $siteFullRoot 'framework-views.json'
        $frameworkPrefixes = @()
        if (Test-Path -LiteralPath $frameworkManifest -PathType Leaf) {
            $framework = Get-Content -LiteralPath $frameworkManifest -Raw | ConvertFrom-Json -AsHashtable
            $frameworkPrefixes = @(foreach ($entry in $framework.views.GetEnumerator()) {
                if ($entry.Key -cne $framework.defaultView) { "_frameworks/$($entry.Value.directory)" }
            })
        }
        $htmlLinksChecked = 0
        foreach ($htmlFile in $htmlFiles) {
            $relHtml = $htmlFile.FullName.Substring($siteFullRoot.Length + 1).Replace('\', '/')
            $publicHtml = $relHtml
            foreach ($prefix in $frameworkPrefixes) {
                if ($relHtml.StartsWith("$prefix$($framework.apiPath)", [StringComparison]::Ordinal)) {
                    $publicHtml = $relHtml.Substring($prefix.Length + 1)
                    break
                }
            }
            $baseRelDir = [IO.Path]::GetDirectoryName($publicHtml)?.Replace('\', '/')
            $htmlText = [IO.File]::ReadAllText($htmlFile.FullName)
            foreach ($match in $divRegex.Matches($htmlText)) {
                $attrs = $match.Groups['attrs'].Value
                $inner = $match.Groups['inner'].Value
                $idMatch = $idAttrRegex.Match($attrs)
                if (-not $idMatch.Success) {
                    Add-Failure $errors "${relHtml}: docfx-run-actions block missing data-example-id."
                    continue
                }
                $id = $idMatch.Groups['id'].Value
                if (-not $expectedById.ContainsKey($id)) {
                    Add-Failure $errors "${relHtml}: referenced id '$id' missing from manifest."
                    continue
                }
                foreach ($linkMatch in $linkRegex.Matches($inner)) {
                    $linkAttrs = $linkMatch.Groups['attrs'].Value
                    $hrefMatch = $hrefAttrRegex.Match($linkAttrs)
                    $trMatch = $transportAttrRegex.Match($linkAttrs)
                    if (-not $hrefMatch.Success -or -not $trMatch.Success) {
                        Add-Failure $errors "${relHtml} ($id): anchor missing href or data-transport."
                        continue
                    }
                    $href = $hrefMatch.Groups['href'].Value
                    $tr = $trMatch.Groups['transport'].Value
                    $resolved = Resolve-RelativeWebPath -BaseRelDir $baseRelDir -Href $href
                    if ($null -eq $resolved -or $resolved -cne "run-actions/$id/$tr.ps1") {
                        Add-Failure $errors "${relHtml} ($id): href '$href' resolved to '$resolved' instead of 'run-actions/$id/$tr.ps1'."
                        continue
                    }
                    $targetFile = Join-Path $siteFullRoot $resolved
                    if (-not (Test-Path -LiteralPath $targetFile -PathType Leaf) -or (Get-Item -LiteralPath $targetFile).Length -eq 0) {
                        Add-Failure $errors "${relHtml} ($id): published target '$resolved' is missing or empty."
                    }
                    $htmlLinksChecked++
                }
            }
            $cleanedHtml = $divRegex.Replace($htmlText, '')
            if ($cleanedHtml -match 'href="[^"]*run-actions/[^"]*"') {
                Add-Failure $errors "${relHtml}: contains stray run-actions href outside docfx-run-actions block."
            }
        }
        if ($htmlLinksChecked -eq 0) {
            Add-Failure $errors "No docfx-run-actions links were found in HTML under $siteFullRoot."
        }
    }
}

if (-not [string]::IsNullOrWhiteSpace($OciArchivePath)) {
    Test-OciArchiveAssets -ArchivePath $OciArchivePath -ExpectedScripts $expectedScripts -Errors $errors
}

if ($errors.Count -gt 0) {
    foreach ($err in $errors) {
        Write-Error $err -ErrorAction Continue
    }
    throw "DocFX run-action validation failed with $($errors.Count) error(s)."
}

Write-Host "Validated $($expectedById.Count) manifest example(s), $($referencedScripts.Count) Markdown reference(s), and $($expectedScripts.Count) generated script(s)."
