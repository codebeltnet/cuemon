#requires -Version 7.0
[CmdletBinding()]
param(
    [Parameter(Mandatory)][string] $Workspace,
    [string] $CompareWorkspace,
    [string] $Origin,
    [string] $NginxImage = 'dhi.io/nginx:1.31-alpine',
    [switch] $Browser
)

$ErrorActionPreference = 'Stop'
$PSNativeCommandUseErrorActionPreference = $false
$workspace = (Resolve-Path -LiteralPath $Workspace).Path
$site = "$workspace/site"
$legacy = 'System.Runtime.CompilerServices.CallerArgumentExpressionAttribute'
$buffer = 'Cuemon.IO.BufferWriterOptions'
$factory = 'Cuemon.IO.StreamFactory'
$baselineUids = @{ 'net11.0' = 557; 'net10.0' = 557; 'netstandard2.1' = 76; 'netstandard2.0' = 548 }

function Assert-True([bool] $Condition, [string] $Message) {
    if (!$Condition) { throw $Message }
}

function Read-Json([string] $Path) {
    Get-Content -LiteralPath $Path -Raw | ConvertFrom-Json -AsHashtable
}

function Get-Fingerprint([string] $Root) {
    $entries = [Collections.Generic.Dictionary[string,string]]::new([StringComparer]::Ordinal)
    foreach ($file in Get-ChildItem -LiteralPath $Root -Recurse -File -Force) {
        $name = [IO.Path]::GetRelativePath($Root, $file.FullName).Replace('\', '/')
        if ($name -ceq 'manifest.json') {
            $value = Read-Json $file.FullName
            $value.Remove('source_base_path')
            $value.files = @($value.files | ForEach-Object { ConvertTo-Json -InputObject $_ -Depth 30 -Compress } | Sort-Object -CaseSensitive)
            $content = ConvertTo-Json -InputObject $value -Depth 30 -Compress
        } elseif ($file.Name -ceq 'index.json') {
            $index = Read-Json $file.FullName
            $content = ($index.Keys | Sort-Object -CaseSensitive | ForEach-Object {
                "$_ " + (ConvertTo-Json -InputObject $index[$_] -Depth 20 -Compress)
            }) -join [char]10
        } elseif ($file.Extension -cin '.html', '.yml', '.json', '.js', '.css', '.ps1', '.svg', '.map' -or $file.Name -ceq '.manifest') {
            $content = [IO.File]::ReadAllText($file.FullName).Replace([string]([char]13) + [char]10, [string][char]10)
            if ($content.StartsWith('### YamlMime:ManagedReference')) {
                # Match the repository-relative source path DocFX uses to render the source link.
                $content = $content -creplace '(?m)(^  source:\n    remote:\n      path: (\S+)\n      branch: [^\n]+\n      repo: [^\n]+\n    id: [^\n]+\n    path: )(?:[^\n]*/)?\2$', '$1$2'
            }
        } else {
            $entries.Add($name, (Get-FileHash -LiteralPath $file.FullName).Hash)
            continue
        }
        $entries.Add($name, [Convert]::ToHexString([Security.Cryptography.SHA256]::HashData([Text.Encoding]::UTF8.GetBytes($content))))
    }
    $entries
}

function Compare-Trees([string] $Left, [string] $Right) {
    $leftHashes = Get-Fingerprint $Left
    $rightHashes = Get-Fingerprint $Right
    $differences = @($leftHashes.Keys | Where-Object { $leftHashes[$_] -cne $rightHashes[$_] })
    $differences += @($rightHashes.Keys | Where-Object { !$leftHashes.ContainsKey($_) })
    Assert-True (!$differences.Count) "Different files between $Left and $Right ($($differences.Count)): $($differences | Select-Object -First 15)"
}

function Get-Page([string] $Path, [int] $Status = 200, [string] $Mime) {
    $response = $client.GetAsync([uri]::new($baseUri, $Path)).GetAwaiter().GetResult()
    try {
        Assert-True ([int]$response.StatusCode -eq $Status) "$Path returned $([int]$response.StatusCode), expected $Status."
        if ($Mime) {
            Assert-True ($response.Content.Headers.ContentType.MediaType -match $Mime) "$Path has unexpected content type."
        }
        if ($Path -notmatch '/public/') {
            Assert-True ($response.Headers.CacheControl.NoStore -and !$response.Headers.ETag) "$Path has unexpected cache headers."
        }
        $response.Content.ReadAsStringAsync().GetAwaiter().GetResult()
    } finally {
        $response.Dispose()
    }
}

function Get-Xrefs([string] $Yaml, [string] $Api) {
    $xref = [Collections.Generic.Dictionary[string,string]]::new([StringComparer]::Ordinal)
    $uid = $null
    # The native DocFX xref writer emits simple scalar UIDs and hrefs for this fixture.
    foreach ($line in ($Yaml -split '\r?\n')) {
        if ($line -cmatch '^- uid: (.+)$') { $uid = $Matches[1] }
        if ($uid -and $line -cmatch '^  href: (.+)$') {
            $href = $Matches[1]
            if (("/$href").StartsWith($Api, [StringComparison]::Ordinal)) { $xref.Add($uid, $href) }
            $uid = $null
        }
    }
    $xref
}

$manifest = Read-Json "$site/framework-views.json"
if ($CompareWorkspace) {
    Compare-Trees $site "$CompareWorkspace/site"
    foreach ($entry in $manifest.views.Values) {
        $directory = $entry.directory
        Compare-Trees "$workspace/$directory$($manifest.apiPath)" "$CompareWorkspace/$directory$($manifest.apiPath)"
    }
    Write-Host 'Isolated metadata and packaged site match the comparison build.'
}

$container = $null
$handler = [Net.Http.HttpClientHandler]::new()
$handler.AllowAutoRedirect = $false
$client = [Net.Http.HttpClient]::new($handler)
$client.Timeout = [TimeSpan]::FromSeconds(10)
try {
    if (!$Origin) {
        $mounts = @('--mount', "type=bind,source=$site,target=/usr/share/nginx/html,readonly",
            '--mount', "type=bind,source=$workspace/nginx.conf,target=/etc/nginx/conf.d/default.conf,readonly")
        $container = & docker run --rm -d -p 127.0.0.1::8080 @mounts $NginxImage
        Assert-True ($LASTEXITCODE -eq 0) 'Could not start isolated NGINX.'
        $port = (& docker port $container 8080/tcp).Trim()
        Assert-True ($LASTEXITCODE -eq 0 -and $port -match '^127\.0\.0\.1:\d+$') 'Could not resolve mapped NGINX port.'
        $Origin = "http://$port"
    }
    $baseUri = [uri]$Origin
    Write-Host "Testing $Origin"
    for ($i = 0; $i -lt 20; $i++) {
        try { Get-Page '/framework-views.json' 200 'application/json' | Out-Null; break }
        catch {
            if ($i -eq 19) { throw }
            Start-Sleep -Milliseconds 250
        }
    }

    $api = $manifest.apiPath
    $factoryPath = "$api$factory.html"
    $defaultHtml = Get-Page $factoryPath 200 'text/html'
    Assert-True ($defaultHtml -ceq (Get-Page "$($factoryPath)?view=$($manifest.defaultView)" 200 'text/html')) 'Implicit and explicit default differ.'
    Get-Page "$($factoryPath)?view=unknown" 400 | Out-Null
    Get-Page "$($factoryPath)?view=$($manifest.defaultView.ToUpperInvariant())" 400 | Out-Null
    Get-Page '/index.html?view=unknown' 200 'text/html' | Out-Null
    Get-Page '/public/docfx.min.css?view=unknown' 200 'text/css' | Out-Null
    Get-Page '/public/docfx.min.js' 200 'javascript' | Out-Null
    Get-Page '/images/favicon.ico' 200 'image/' | Out-Null
    $font = Get-ChildItem "$site/public" -Filter '*.woff2' | Select-Object -First 1
    $fontResponse = $client.GetAsync([uri]::new($baseUri, "/public/$($font.Name)")).GetAwaiter().GetResult()
    try {
        Assert-True ($fontResponse.IsSuccessStatusCode -and $fontResponse.Headers.CacheControl.ToString().Contains('immutable')) 'Fingerprinted font caching policy changed.'
    } finally { $fontResponse.Dispose() }

    foreach ($view in $manifest.views.Keys) {
        $entry = $manifest.views[$view]
        $stage = "$workspace/$($entry.directory)"
        $inventory = Read-Json "$stage$($api).manifest"
        Assert-True ($inventory.Count -eq $baselineUids[$entry.tfm]) "Unexpected baseline UID count for $view."
        $xref = Get-Xrefs (Get-Page $entry.xref) $api
        $renderedUids = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
        $renderedUids.UnionWith([string[]]@($xref.Keys | Where-Object { !$_.EndsWith('*') }))
        Assert-True ($renderedUids.SetEquals([string[]]@($inventory.Keys))) "$view served xrefs differ from the isolated DocFX inventory."
        $pages = @{}
        foreach ($uid in $xref.Keys) {
            $path, $anchor = $xref[$uid].Split('#', 2)
            if (!$pages.ContainsKey($path)) { $pages[$path] = Get-Page "/$($path)?view=$view" 200 'text/html' }
            if ($anchor) {
                $id = [Net.WebUtility]::HtmlEncode([uri]::UnescapeDataString($anchor))
                Assert-True ($pages[$path].Contains("id=""$id""")) "Missing anchor for $uid in $view."
            }
        }
        $example = $pages.GetEnumerator() | Where-Object Value -Match 'href="[^"]*run-actions/[^"]+\.ps1"' | Select-Object -First 1
        Assert-True ($null -ne $example) "No run-action link in $view."
        $href = [regex]::Match($example.Value, 'href="([^"]*run-actions/[^"]+\.ps1)"').Groups[1].Value
        $publicPage = [uri]::new($baseUri, "/$($example.Key)?view=$view")
        $runAction = [uri]::new($publicPage, $href)
        Assert-True ($runAction.AbsolutePath.StartsWith('/run-actions/')) "Run-action escaped the shared public path in $view."
        Get-Page $runAction.AbsolutePath | Out-Null

        $isLegacy = $entry.tfm -ceq 'netstandard2.0'
        $toc = Get-Page "$($api)toc.json?view=$view" 200 'application/json'
        $index = Get-Page "/_framework-search/$($entry.directory)/index.json" 200 'application/json' | ConvertFrom-Json -AsHashtable
        foreach ($path in $index.Keys) {
            if (("/$path").StartsWith($api) -and !$pages.ContainsKey($path)) { throw "Search indexes unavailable page $path in $view." }
        }
        $overloads = @($xref.Keys | Where-Object { $_.StartsWith("$factory.Create") -and $_.Contains('System.Buffers.IBufferWriter{System.Byte}') })
        $expectedCount = if ($isLegacy) { 0 } else { 6 }
        Assert-True ($overloads.Count -eq $expectedCount) "Wrong buffer writer overload count in $view."
        $create = $pages[$xref["$factory.Create*"]]
        Assert-True ([regex]::Matches($create, 'id="[^"]+_System_Buffers_IBufferWriter_System_Byte_[^"]*"').Count -eq $expectedCount) "Wrong served overload anchors in $view."
        Assert-True ($create.Contains('href="Cuemon.IO.BufferWriterOptions.html"') -eq !$isLegacy) "Wrong BufferWriterOptions cross-reference in $view."
        $exception = 'Cuemon.ArgumentReservedKeywordException'
        if ($xref.ContainsKey($exception)) {
            $inheritsSerialization = $pages[$xref[$exception]].Contains('system.argumentoutofrangeexception.getobjectdata')
            Assert-True ($inheritsSerialization -eq $isLegacy) "Wrong inherited serialization member in $view."
        }
        if ($isLegacy) {
            $legacyNamespace = $pages[$xref['System.Runtime.CompilerServices']]
            Assert-True ($legacyNamespace.Contains('provides polyfill types')) 'Missing compatibility namespace overwrite.'
        }

        foreach ($type in $legacy, $buffer, 'Cuemon.Validator') {
            $expected = switch ($type) {
                $legacy { $isLegacy }
                $buffer { !$isLegacy }
                default { $entry.tfm -cne 'netstandard2.1' }
            }
            $path = "$($api.TrimStart('/'))$type.html"
            Assert-True ($xref.ContainsKey($type) -eq $expected) "Wrong xref availability for $type in $view."
            Assert-True ($toc.Contains("""topicUid"":""$type""") -eq $expected) "Wrong TOC availability for $type in $view."
            Assert-True ($index.Contains($path) -eq $expected) "Wrong search availability for $type in $view."
            if (!$expected) {
                Get-Page "/$($path)?view=$view" 404 | Out-Null
                continue
            }
            Assert-True ($pages[$path] -match 'github\.com/codebeltnet/cuemon/blob/[^"]+\.cs/#L\d+') "Missing source link for $type in $view."
        }
        foreach ($file in Get-ChildItem "$stage/api/types" -Filter '*.md') {
            if (!$xref.ContainsKey($file.BaseName)) { continue }
            if ((Get-Content -LiteralPath $file.FullName -Raw) -notmatch '(?m)^example:') { continue }
            $html = $pages[$xref[$file.BaseName]]
            Assert-True ($html -match '(?s)<h2 id="[^"]+_examples">Examples</h2>.*?<pre><code[^>]*>\s*\S') "Missing authored example for $($file.BaseName) in $view."
        }
        Get-Page "/_frameworks/$($entry.directory)$factoryPath" 404 | Out-Null
        Write-Host "$view passed $($inventory.Count) UID, anchor, TOC and search checks across $($pages.Count) served API pages."
    }
    if ($Browser) {
        & node "$PSScriptRoot/Test-FrameworkNavigation.cjs" $Origin
        Assert-True ($LASTEXITCODE -eq 0) 'Playwright navigation tests failed.'
    }
} finally {
    $client.Dispose()
    if ($container) { & docker stop $container | Out-Null }
}
