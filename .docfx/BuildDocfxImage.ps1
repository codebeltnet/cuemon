#requires -Version 7.0
<#
.SYNOPSIS
Builds and verifies the local Cuemon documentation image using the shared Codebelt DocFX builder.
#>
[CmdletBinding()]
param(
    # Regenerate metadata even when the shared builder can reuse its cache.
    [switch] $ForceMetadata
)

$ErrorActionPreference = 'Stop'
$PSNativeCommandUseErrorActionPreference = $false

function Invoke-Checked {
    $command, $arguments = $args
    & $command @arguments
    if ($LASTEXITCODE -ne 0) { throw "$command $($arguments[0]) failed with exit code $LASTEXITCODE." }
}

$repoRoot = Split-Path -Parent $PSScriptRoot
$builder = 'codebeltnet/docfx:2'
$botRoot = Join-Path $repoRoot '.bot/docfx'
$cacheRoot = Join-Path $botRoot 'cache'
$outputRoot = Join-Path $botRoot 'output'
$work = Join-Path ([IO.Path]::GetTempPath()) "cuemon-docfx-$([Guid]::NewGuid().ToString('N'))"
$verifyDir = Join-Path $work 'verify'
$runActionsDir = Join-Path $verifyDir 'run-actions'
$timer = [Diagnostics.Stopwatch]::StartNew()

Push-Location $repoRoot
try {
    $version = Invoke-Checked minver -i -t v -v w
    $image = "cuemon-docfx:$version"
    $revision = (Invoke-Checked git rev-parse HEAD).Trim()
    $branch = (Invoke-Checked git rev-parse --abbrev-ref HEAD).Trim()
    $remoteUrl = (git config --get remote.origin.url 2>$null | Out-String).Trim()
    $sourceRevision = (& git log -1 --format=%H -- src Directory.Build.props Directory.Build.targets Directory.Packages.props global.json nuget.config).Trim()
    if ($LASTEXITCODE -ne 0) { throw "git log failed with exit code $LASTEXITCODE." }

    Invoke-Checked docker pull $builder
    $builderId = (Invoke-Checked docker image inspect --format '{{.Id}}' $builder).Trim()

    $xrefCacheRoot = Join-Path $cacheRoot 'xref'
    New-Item -ItemType Directory -Path $cacheRoot, $xrefCacheRoot, $verifyDir -Force | Out-Null
    Copy-Item -LiteralPath "$PSScriptRoot/verify-docfx-site.sh", "$PSScriptRoot/run-actions.manifest.json" -Destination $verifyDir

    & "$PSScriptRoot/BuildRunActions.ps1" -OutputRoot $runActionsDir
    & "$PSScriptRoot/TestRunActions.ps1" -ScriptsRoot $runActionsDir

    # Pack clean authored inputs without host bin/obj or generated DocFX YAML.
    $inputs = @(Invoke-Checked git -c core.quotePath=false ls-files --cached --others --exclude-standard -- .docfx src)
    if (Test-Path -LiteralPath .docfx/packages -PathType Container) {
        $inputs += @(Get-ChildItem -LiteralPath .docfx/packages -Recurse -File | ForEach-Object {
            [IO.Path]::GetRelativePath($repoRoot, $_.FullName).Replace('\', '/')
        })
    }
    $stagedFiles = @($inputs | Sort-Object -Unique | Where-Object { Test-Path -LiteralPath $_ -PathType Leaf })
    [IO.File]::WriteAllLines("$work/inputs.txt", $stagedFiles)
    Invoke-Checked tar -cf "$work/inputs.tar" -T "$work/inputs.txt"
    Invoke-Checked tar -cf "$work/run-actions.tar" -C $verifyDir run-actions

    $configPath = Join-Path $PSScriptRoot 'docfx.json'
    $config = Get-Content -LiteralPath $configPath -Raw | ConvertFrom-Json -AsHashtable
    $configModified = $false
    $skipSignAssembly = !(Test-Path -LiteralPath cuemon.snk -PathType Leaf)
    if ($skipSignAssembly) {
        foreach ($group in $config.metadata) {
            $group.properties ??= @{}
            $group.properties.SkipSignAssembly = 'true'
        }
        $configModified = $true
    }

    # Cache remote xref maps under .bot/docfx/cache/xref so every TFM build avoids multi-100MB downloads.
    $xrefDownloads = [Collections.Generic.List[string]]::new()
    if ($config.ContainsKey('build') -and $config.build -and $config.build.ContainsKey('xref') -and $config.build.xref) {
        $rewrittenXref = @(foreach ($entry in @($config.build.xref)) {
            $url = [string]$entry
            if ($url -match '^https?://') {
                $urlHash = [Convert]::ToHexString([Security.Cryptography.SHA256]::HashData([Text.Encoding]::UTF8.GetBytes($url))).ToLowerInvariant()
                $ext = [IO.Path]::GetExtension(([Uri]$url).AbsolutePath)
                if ([string]::IsNullOrWhiteSpace($ext)) { $ext = '.json' }
                $fileName = "$urlHash$ext"
                $xrefDownloads.Add("$fileName`t$url")
                "/cache/xref/$fileName"
            } else {
                $url
            }
        })
        if ($xrefDownloads.Count -gt 0) {
            $config.build.xref = $rewrittenXref
            $configModified = $true
        }
    }
    if ($configModified) {
        ConvertTo-Json -InputObject $config -Depth 100 | Set-Content -LiteralPath "$work/docfx.json"
    }
    if ($xrefDownloads.Count -gt 0) {
        [IO.File]::WriteAllText("$work/xref-urls.tsv", ($xrefDownloads -join "`n") + "`n", [Text.UTF8Encoding]::new($false))
    }

    # The upstream engine hashes its own implementation, docfx.json, SDK versions, and discovered views.
    # This fingerprint covers Cuemon's source files, branch/remote, last source-affecting commit, root build/restore/signing inputs, and metadata filters.
    # Doc-only or script-only commits keep the same metadata cache key and reuse cached metadata.
    $buildInputs = Get-ChildItem -Force -File | Where-Object {
        $_.Extension -in '.props', '.targets', '.snk' -or $_.Name -in 'global.json', 'nuget.config'
    } | Sort-Object Name
    $srcFiles = @($stagedFiles | Where-Object { $_.StartsWith('src/', [StringComparison]::Ordinal) })
    $filterFiles = @($config.metadata.filter | Where-Object { $_ } | Sort-Object -Unique)
    $gitSourceRef = if ($branch -ceq 'HEAD') { $revision } else { $branch }
    $fingerprint = @(
        $builder
        $builderId
        "Branch=$gitSourceRef"
        "Remote=$remoteUrl"
        "SourceRevision=$sourceRevision"
        "SkipSignAssembly=$skipSignAssembly"
        "CI=$($env:CI)"
        "EMAIL=$($env:EMAIL)"
        foreach ($file in $buildInputs) { "$($file.Name)=$((Get-FileHash -LiteralPath $file.FullName).Hash)" }
        foreach ($path in $srcFiles) { "$path=$((Get-FileHash -LiteralPath $path).Hash)" }
        foreach ($name in $filterFiles) { ".docfx/$name=$((Get-FileHash -LiteralPath (Join-Path $PSScriptRoot $name)).Hash)" }
    ) -join "`n"
    $cacheKey = (Get-FileHash -InputStream ([IO.MemoryStream]::new([Text.Encoding]::UTF8.GetBytes($fingerprint)))).Hash
    $metadataMarker = Join-Path $cacheRoot "metadata/.cuemon-key-$cacheKey"
    $needsPreRestore = $ForceMetadata.IsPresent -or -not (Test-Path -LiteralPath $metadataMarker -PathType Leaf)

    $dockerArgs = @(
        'run', '--rm'
        '--mount', "type=bind,source=$repoRoot,target=/workspace,readonly"
        '--mount', 'type=volume,target=/workspace/src,volume-nocopy'
        '--mount', 'type=volume,target=/workspace/.docfx,volume-nocopy'
        '--mount', "type=bind,source=$work,target=/work"
        '--mount', "type=bind,source=$cacheRoot,target=/cache"
        '--mount', 'type=volume,source=cuemon-docfx-nuget,target=/root/.nuget/packages'
        '-w', '/workspace'
        '-e', "CUEMON_PRE_RESTORE=$(if ($needsPreRestore) { '1' } else { '0' })"
        '-e', "CUEMON_SKIP_SIGN=$(if ($skipSignAssembly) { 'true' } else { 'false' })"
        '-e', "CUEMON_CACHE_KEY=$cacheKey"
    )
    if ($env:CI) { $dockerArgs += @('-e', 'CI') }
    if ($env:EMAIL) { $dockerArgs += @('-e', 'EMAIL') }

    $generator = @(
        '-DocfxConfig', '.docfx/docfx.json'
        '-OutputRoot', '/tmp/docfx-output'
        '-MetadataCache', '/cache/metadata'
        '-MetadataCacheKey', $cacheKey
    )
    if ($ForceMetadata) { $generator += '-ForceMetadata' }
    if ($needsPreRestore) { $generator += '-NoRestore' }

    # Generate on the container filesystem and transfer packaged artifacts back as archives.
    $buildSite = @'
set -eu
test -f /opt/docfx/framework-views/Build-DocfxFrameworkViews.ps1
test -f /opt/docfx/framework-views/framework-views.Dockerfile
git config --global --add safe.directory /workspace
tar -xf /work/inputs.tar -C /workspace
tar -xf /work/run-actions.tar -C /workspace/.docfx
if [ -f /work/docfx.json ]; then
    cp /work/docfx.json /workspace/.docfx/docfx.json
fi
if [ -f /work/xref-urls.tsv ]; then
    mkdir -p /cache/xref
    while IFS="$(printf '\t')" read -r fname url; do
        [ -n "$fname" ] || continue
        target="/cache/xref/$fname"
        if [ ! -s "$target" ]; then
            echo "Caching xref map $url -> $target"
            tmp="$target.$$.tmp"
            curl -fsSL --retry 3 --retry-delay 2 --retry-all-errors --compressed "$url" -o "$tmp" || { rm -f "$tmp"; exit 1; }
            mv -f "$tmp" "$target"
        fi
    done < /work/xref-urls.tsv
fi
sh .docfx/verify-docfx-site.sh --context .docfx
cp /opt/docfx/framework-views/framework-views.Dockerfile /work/framework-views.Dockerfile
if [ "${CUEMON_PRE_RESTORE:-0}" = "1" ]; then
    printf '<Solution>\n' > /tmp/restore.slnx
    find /workspace/src -name '*.csproj' | sort | while IFS= read -r proj; do
        printf '  <Project Path="%s" />\n' "$proj" >> /tmp/restore.slnx
    done
    printf '</Solution>\n' >> /tmp/restore.slnx
    dotnet restore /tmp/restore.slnx --verbosity quiet -p:Configuration=Release "-p:SkipSignAssembly=${CUEMON_SKIP_SIGN:-false}"
fi
mkdir /tmp/docfx-output
status=0
pwsh -NoProfile -File /opt/docfx/framework-views/Build-DocfxFrameworkViews.ps1 "$@" || status=$?
cd /tmp/docfx-output
find . -type f -name '*.log' | tar -cf /work/logs.tar -T -
[ "$status" -eq 0 ] || exit "$status"
mkdir -p /cache/metadata
touch "/cache/metadata/.cuemon-key-${CUEMON_CACHE_KEY}"
tar -cf /work/site.tar site nginx.conf
'@
    Set-Content -LiteralPath "$work/build-site.sh" -Value ($buildSite.Replace("`r`n", "`n")) -NoNewline

    # Clean only generated output; keep persistent cache under .bot/docfx/cache untouched.
    if (Test-Path -LiteralPath $outputRoot) { Remove-Item -LiteralPath $outputRoot -Recurse -Force }
    New-Item -ItemType Directory -Path $outputRoot -Force | Out-Null

    Write-Host "DocFX build output and logs will be saved to $outputRoot"
    try {
        Invoke-Checked docker @dockerArgs $builderId sh /work/build-site.sh @generator
    } finally {
        if (Test-Path -LiteralPath "$work/logs.tar") {
            Invoke-Checked tar -xf "$work/logs.tar" -C $outputRoot
        }
    }
    Invoke-Checked tar -xf "$work/site.tar" -C $outputRoot
    Copy-Item -LiteralPath "$work/framework-views.Dockerfile" -Destination "$outputRoot/framework-views.Dockerfile"

    & "$PSScriptRoot/TestRunActions.ps1" -ScriptsRoot $runActionsDir -SiteRoot "$outputRoot/site"

    @('*', '!site/', '!site/**', '!nginx.conf') | Set-Content -LiteralPath "$outputRoot/.dockerignore"
    Invoke-Checked docker buildx build --platform 'linux/amd64,linux/arm64' --load -t $image -t cuemon-docfx:latest `
        -f "$outputRoot/framework-views.Dockerfile" $outputRoot

    Invoke-Checked docker run --rm --entrypoint sh --mount "type=bind,source=$verifyDir,target=/verify,readonly" $image `
        /verify/verify-docfx-site.sh --site /usr/share/nginx/html --source /verify --manifest /verify/run-actions.manifest.json

    Write-Host ("Built and verified {0} in {1:N2}s." -f $image, $timer.Elapsed.TotalSeconds)
    Write-Host 'docker run --rm -p 127.0.0.1:8080:8080 cuemon-docfx'
    Write-Host 'Browse to http://localhost:8080/'
} finally {
    if (Test-Path -LiteralPath $work) { Remove-Item -LiteralPath $work -Recurse -Force }
    Pop-Location
}
