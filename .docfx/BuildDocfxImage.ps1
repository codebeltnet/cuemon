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
$builder = 'codebeltnet/docfx:2.81.0-v1.1.0'
$expectedCliVersion = '2.81.0'
$expectedEngineVersion = '1.1.0'
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
    $revision = Invoke-Checked git rev-parse HEAD
    $description = Invoke-Checked git describe --tags --always --match 'v*'

    if (!(Invoke-Checked docker image ls -q $builder)) {
        Invoke-Checked docker pull $builder
    }
    $builderMeta = (Invoke-Checked docker image inspect --format '{{.Id}}|{{index .Config.Labels "net.codebelt.docfx.cli.version"}}|{{index .Config.Labels "net.codebelt.docfx.engine.version"}}' $builder).Trim()
    $builderId, $cliVersion, $engineVersion = $builderMeta.Split('|', 3)
    if ($cliVersion -cne $expectedCliVersion -or $engineVersion -cne $expectedEngineVersion) {
        throw "Builder image '$builder' reported CLI '$cliVersion' and engine '$engineVersion'; expected CLI '$expectedCliVersion' and engine '$expectedEngineVersion'."
    }

    New-Item -ItemType Directory -Path $cacheRoot, $verifyDir -Force | Out-Null
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
    $skipSignAssembly = !(Test-Path -LiteralPath cuemon.snk -PathType Leaf)
    if ($skipSignAssembly) {
        foreach ($group in $config.metadata) {
            $group.properties ??= @{}
            $group.properties.SkipSignAssembly = 'true'
        }
        ConvertTo-Json -InputObject $config -Depth 100 | Set-Content -LiteralPath "$work/docfx.json"
    }

    # The upstream engine hashes its own implementation, docfx.json, SDK versions, and discovered views.
    # This fingerprint covers Cuemon's source files, revision, root build/restore/signing inputs, and metadata filters.
    $buildInputs = Get-ChildItem -Force -File | Where-Object {
        $_.Extension -in '.props', '.targets', '.snk' -or $_.Name -in 'global.json', 'nuget.config'
    } | Sort-Object Name
    $srcFiles = @($stagedFiles | Where-Object { $_.StartsWith('src/', [StringComparison]::Ordinal) })
    $filterFiles = @($config.metadata.filter | Where-Object { $_ } | Sort-Object -Unique)
    $fingerprint = @(
        $builder
        $builderId
        $revision
        $description
        $version
        "SkipSignAssembly=$skipSignAssembly"
        "CI=$($env:CI)"
        "EMAIL=$($env:EMAIL)"
        foreach ($file in $buildInputs) { "$($file.Name)=$((Get-FileHash -LiteralPath $file.FullName).Hash)" }
        foreach ($path in $srcFiles) { "$path=$((Get-FileHash -LiteralPath $path).Hash)" }
        foreach ($name in $filterFiles) { ".docfx/$name=$((Get-FileHash -LiteralPath (Join-Path $PSScriptRoot $name)).Hash)" }
    ) -join "`n"
    $cacheKey = (Get-FileHash -InputStream ([IO.MemoryStream]::new([Text.Encoding]::UTF8.GetBytes($fingerprint)))).Hash

    $dockerArgs = @(
        'run', '--rm'
        '--mount', "type=bind,source=$repoRoot,target=/workspace,readonly"
        '--mount', 'type=volume,target=/workspace/src,volume-nocopy'
        '--mount', 'type=volume,target=/workspace/.docfx,volume-nocopy'
        '--mount', "type=bind,source=$work,target=/work"
        '--mount', "type=bind,source=$cacheRoot,target=/cache"
        '--mount', 'type=volume,source=cuemon-docfx-nuget,target=/root/.nuget/packages'
        '-w', '/workspace'
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
sh .docfx/verify-docfx-site.sh --context .docfx
cp /opt/docfx/framework-views/framework-views.Dockerfile /work/framework-views.Dockerfile
mkdir /tmp/docfx-output
status=0
pwsh -NoProfile -File /opt/docfx/framework-views/Build-DocfxFrameworkViews.ps1 "$@" || status=$?
cd /tmp/docfx-output
find . -type f -name '*.log' | tar -cf /work/logs.tar -T -
[ "$status" -eq 0 ] || exit "$status"
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
