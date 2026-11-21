#requires -Version 7.0
[CmdletBinding()]
param(
    [string] $ManifestPath = (Join-Path $PSScriptRoot 'run-actions.manifest.json'),
    [string] $OutputRoot = (Join-Path $PSScriptRoot 'run-actions')
)
$ErrorActionPreference = 'Stop'
$PSNativeCommandUseErrorActionPreference = $false
Set-StrictMode -Version Latest

if (-not (Test-Path -LiteralPath $ManifestPath -PathType Leaf)) {
    if ($PSBoundParameters.ContainsKey('ManifestPath')) {
        throw "Run-actions manifest not found: $ManifestPath"
    }
    Write-Host "No run-actions manifest found at $ManifestPath; skipping."
    return
}

$CsrunTestVerificationTemplate = @'
function Assert-CsrunTestOutput([string] $Output) {
    $lines = @($Output -split '\r?\n')
    $headers = @($lines | Where-Object { $_ -match '^Test run summary: Passed! - .+\)$' })
    if ($headers.Count -ne 1) { throw 'Expected exactly one completed xUnit test summary.' }
    $headerIndex = [Array]::IndexOf($lines, $headers[0])
    $summaryLines = @($lines | Select-Object -Skip ($headerIndex + 1) -First 4)
    $counts = @{}
    foreach ($line in $summaryLines) {
        if ($line -match '^\s*(total|failed|succeeded|skipped):\s*(\d+)\s*$') { $counts[$Matches[1]] = [int]$Matches[2] }
    }
    if ($counts.Count -ne 4) { throw 'The xUnit test summary counts are incomplete or ambiguous.' }
    $failed = $counts.failed
    $passed = $counts.succeeded
    $skipped = $counts.skipped
    $total = $counts.total
    if ($total -lt 1 -or $passed -lt 1 -or $failed -ne 0 -or $skipped -ne 0 -or $passed -ne $total) {
        throw 'Tests must execute at least once, all pass, and none fail or skip.'
    }
}
'@

function Normalize([string] $Text) { $Text.Replace("`r`n", "`n").TrimEnd("`n", "`r") }
function Quote-Literal([string] $Value) { "'" + $Value.Replace("'", "''") + "'" }

function Sort-ManifestValue($Value) {
    if ($Value -is [System.Collections.IDictionary]) {
        $ordered = [ordered]@{}
        [string[]] $keys = @($Value.Keys)
        [Array]::Sort($keys, [StringComparer]::Ordinal)
        foreach ($key in $keys) { $ordered[$key] = Sort-ManifestValue $Value[$key] }
        return $ordered
    }
    if ($Value -is [array]) { return ,@($Value | ForEach-Object { Sort-ManifestValue $_ }) }
    return $Value
}

function New-StandalonePair {
    param(
        [Parameter(Mandatory)][hashtable] $Manifest,
        [Parameter(Mandatory)][string] $Image
    )
    $ordered = Sort-ManifestValue $Manifest
    $manifestJson = ConvertTo-Json -InputObject $ordered -Depth 100
    $verifyTests = $ordered.mode -eq 'test'
    $prefix = "#requires -Version 7.0`n`$ErrorActionPreference = 'Stop'`n`$PSNativeCommandUseErrorActionPreference = `$false`n`$manifest = @'`n" + $manifestJson + "`n'@`n"
    $suffix = @'
if ($process.ExitCode -ne 0) { throw "csrun execution failed with exit code $($process.ExitCode). $diagnostics" }
if ($verifyTests) { Assert-CsrunTestOutput $output }
'@
    $engineSetup = @'
# Downloads and executes the latest stable csrun release with your permissions.
# Source and release history: https://github.com/codebeltnet/csrun
$engineDirectory = Join-Path ([IO.Path]::GetTempPath()) ('csrun-' + [guid]::NewGuid().ToString('N'))
$null = New-Item -ItemType Directory -Path $engineDirectory
$enginePath = Join-Path $engineDirectory 'csrun.ps1'
$process = $null
try {
    $release = Invoke-RestMethod 'https://api.github.com/repos/codebeltnet/csrun/releases/latest'
    $assets = @($release.assets | Where-Object name -CEQ 'csrun.ps1')
    if ($assets.Count -ne 1 -or $assets[0].digest -notmatch '^sha256:[0-9a-f]{64}$') {
        throw "csrun release $($release.tag_name) must publish one csrun.ps1 asset with a SHA-256 digest."
    }
    $asset = $assets[0]
    Invoke-WebRequest $asset.browser_download_url -OutFile $enginePath
    $actualHash = (Get-FileHash -LiteralPath $enginePath -Algorithm SHA256).Hash
    if ("sha256:$actualHash" -ne $asset.digest) { throw "csrun asset checksum mismatch: $actualHash" }
'@
    $nativeInvoke = @'
$startInfo = [Diagnostics.ProcessStartInfo]::new('pwsh')
foreach ($argument in @('-NoProfile','-NonInteractive','-File')) { $startInfo.ArgumentList.Add($argument) }
$startInfo.ArgumentList.Add($enginePath)
$startInfo.ArgumentList.Add('-ManifestJson')
$startInfo.ArgumentList.Add($manifest)
$startInfo.RedirectStandardOutput = $true
$startInfo.RedirectStandardError = $true
$startInfo.UseShellExecute = $false
$process = [Diagnostics.Process]::new()
$process.StartInfo = $startInfo
if (-not $process.Start()) { throw 'Could not start csrun.' }
$stdoutTask = $process.StandardOutput.ReadToEndAsync()
$stderrTask = $process.StandardError.ReadToEndAsync()
$process.WaitForExit()
$output = $stdoutTask.GetAwaiter().GetResult()
$diagnostics = $stderrTask.GetAwaiter().GetResult()
Write-Output $output
if ($diagnostics) { [Console]::Error.Write($diagnostics) }
'@
    $dockerInvoke = (@'
$enginePayload = [Convert]::ToBase64String([IO.File]::ReadAllBytes($enginePath))
$containerInput = $enginePayload + "`n" + $manifest
$command = '
$ErrorActionPreference = "Stop"
$enginePath = Join-Path ([IO.Path]::GetTempPath()) ("csrun-release-" + [guid]::NewGuid().ToString("N") + ".ps1")
$engineBytes = [Convert]::FromBase64String([Console]::In.ReadLine())
[IO.File]::WriteAllBytes($enginePath, $engineBytes)
$manifest = [Console]::In.ReadToEnd()
& $enginePath -ManifestJson $manifest
exit $LASTEXITCODE
'
$output = (@($containerInput | & docker run --rm -i --entrypoint pwsh __IMAGE__ -NoProfile -NonInteractive -Command $command) -join [Environment]::NewLine)
$process = [pscustomobject]@{ ExitCode = $LASTEXITCODE }
$diagnostics = ''
Write-Output $output
'@).Replace('__IMAGE__', (Quote-Literal $Image))
    $engineCleanup = @'
} catch {
    [Console]::Error.WriteLine("csrun diagnostics retained in: $engineDirectory")
    throw
} finally {
    $resolvedTemp = [IO.Path]::TrimEndingDirectorySeparator([IO.Path]::GetFullPath([IO.Path]::GetTempPath()))
    $resolvedEngineDirectory = [IO.Path]::GetFullPath($engineDirectory)
    if ([IO.Path]::GetDirectoryName($resolvedEngineDirectory) -ne $resolvedTemp -or
        [IO.Path]::GetFileName($resolvedEngineDirectory) -cnotmatch '^csrun-[a-f0-9]{32}$') { throw 'Unsafe cleanup path.' }
    if ($process -and $process.ExitCode -eq 0) { Remove-Item -LiteralPath $resolvedEngineDirectory -Recurse -Force }
}
'@
    $encoding = '$OutputEncoding = [Text.UTF8Encoding]::new($false)' + "`n" + '[Console]::OutputEncoding = $OutputEncoding' + "`n"
    $native = "& {`n" + ($prefix + $encoding + '$verifyTests = $' + $verifyTests.ToString().ToLowerInvariant() + "`n" + $CsrunTestVerificationTemplate + "`n" + $engineSetup + "`n" + $nativeInvoke + "`n" + $engineCleanup + "`n" + $suffix).TrimEnd() + "`n}`n"
    $docker = "& {`n" + ($prefix + $encoding + '$verifyTests = $' + $verifyTests.ToString().ToLowerInvariant() + "`n" + $CsrunTestVerificationTemplate + "`n" + $engineSetup + "`n" + $dockerInvoke + "`n" + $engineCleanup + "`n" + $suffix).TrimEnd() + "`n}`n"
    return @{ native = $native; docker = $docker }
}

function New-RepositoryPair {
    param(
        [Parameter(Mandatory)][string] $Repository,
        [Parameter(Mandatory)][string] $Commit,
        [Parameter(Mandatory)][string] $Project,
        [Parameter(Mandatory)][string] $Test,
        [Parameter(Mandatory)][string] $Framework,
        [Parameter(Mandatory)][string] $Image
    )
    if ($Repository -notmatch '^[A-Za-z0-9_.-]+/[A-Za-z0-9_.-]+$' -or $Commit -notmatch '^[a-fA-F0-9]{40}$' -or
        $Project -notmatch '^[A-Za-z0-9_./-]+\.csproj$' -or $Project.Split('/') -contains '..' -or $Project.StartsWith('/') -or
        $Test -notmatch '^[A-Za-z0-9_.+]+$' -or $Framework -notmatch '^net\d+\.\d+$' -or
        $Image -notmatch '^mcr\.microsoft\.com/dotnet/sdk:[A-Za-z0-9_.-]+$') {
        throw 'Expected a pinned GitHub repository, relative project, exact xUnit method, TFM and official SDK image.'
    }
    $template = @'
& {
#requires -Version 7.0
    $ErrorActionPreference = 'Stop'
    $PSNativeCommandUseErrorActionPreference = $false
    # Executes trusted repository build tasks and NuGet packages with your permissions.
    # Review the pinned source before running; a container is not a security sandbox.
    __PREREQUISITE__
    $work = Join-Path ([IO.Path]::GetTempPath()) ('dx-' + [guid]::NewGuid().ToString('N').Substring(0, 8))
    $null = New-Item -ItemType Directory -Path $work
    try {
        $commit = '__COMMIT__'
        Invoke-WebRequest "https://codeload.github.com/__REPOSITORY__/zip/$commit" -OutFile "$work/source.zip"
        Expand-Archive -LiteralPath "$work/source.zip" -DestinationPath "$work/extracted"
        Move-Item -LiteralPath "$work/extracted/__ARCHIVE__-$commit" -Destination "$work/repo"
        $project = '__PROJECT__'
        $test = '__TEST__'
        __EXECUTE__
        $code = $LASTEXITCODE
        if ($code -ne 0) { throw "Repository test failed with exit code $code. See $work/run.log" }
        # Require exactly the original test, executed and passing, with no runner errors.
        [xml] $report = Get-Content -LiteralPath "$work/results/result.xml" -Raw
        $tests = @($report.SelectNodes('/assemblies/assembly/collection/test'))
        $assemblies = @($report.SelectNodes('/assemblies/assembly'))
        if ($assemblies.Count -ne 1 -or $assemblies[0].total -ne '1' -or $assemblies[0].passed -ne '1' -or
            $assemblies[0].errors -ne '0' -or $tests.Count -ne 1 -or $tests[0].name -cne $test -or $tests[0].result -cne 'Pass') {
            throw "Expected exactly one passing $test; inspect $work/results/result.xml"
        }
        Write-Host "Verified: $test (1 executed, 1 passed)."
    }
    catch {
        # Preserve diagnostics outside the repository even though downloaded source is cleaned.
        $diagnostics = "$work-diagnostics"
        $null = New-Item -ItemType Directory -Path $diagnostics -Force
        foreach ($name in @('run.log', 'results')) {
            if (Test-Path -LiteralPath "$work/$name") { Copy-Item -LiteralPath "$work/$name" -Destination $diagnostics -Recurse }
        }
        Write-Warning "Execution not verified. Diagnostics: $diagnostics"
        throw
    }
    finally {
        $resolved = [IO.Path]::GetFullPath($work)
        $temp = [IO.Path]::GetFullPath([IO.Path]::GetTempPath()).TrimEnd('/','\') + [IO.Path]::DirectorySeparatorChar
        if (-not $resolved.StartsWith($temp, [StringComparison]::OrdinalIgnoreCase)) { throw 'Unsafe cleanup path.' }
        Remove-Item -LiteralPath $resolved -Recurse -Force
    }
}
'@
    $native = (@'
Push-Location "$work/repo"
        try {
            dotnet run --project $project --framework __FRAMEWORK__ -p:SkipSignAssembly=true -- --filter-method $test --report-xunit-xml --report-xunit-xml-filename result.xml --results-directory "$work/results" 2>&1 | Tee-Object -FilePath "$work/run.log"
        }
        finally { Pop-Location }
'@).Replace('__FRAMEWORK__', $Framework)
    $docker = (@'
$dockerUser = @()
        if ($IsLinux) {
            # Keep bind-mounted build output removable by the host user.
            $hostUid = id -u
            if ($LASTEXITCODE -ne 0) { throw 'Cannot resolve host UID.' }
            $hostGid = id -g
            if ($LASTEXITCODE -ne 0) { throw 'Cannot resolve host GID.' }
            $dockerUser = @('--user', "${hostUid}:${hostGid}")
        }
        # Let Docker's init reap orphaned build processes and keep build servers disposable.
        docker run --rm --init @dockerUser --env HOME=/tmp --env DOTNET_CLI_HOME=/tmp --env MSBUILDDISABLENODEREUSE=1 --mount "type=bind,source=$work,target=/work" --workdir /work/repo __IMAGE__ dotnet run --disable-build-servers --project $project --framework __FRAMEWORK__ -p:SkipSignAssembly=true -- --filter-method $test --report-xunit-xml --report-xunit-xml-filename result.xml --results-directory /work/results 2>&1 | Tee-Object -FilePath "$work/run.log"
'@).Replace('__FRAMEWORK__', $Framework).Replace('__IMAGE__', $Image)
    $template = $template.Replace('__COMMIT__', $Commit).Replace('__REPOSITORY__', $Repository).Replace('__ARCHIVE__', $Repository.Split('/')[1]).Replace('__PROJECT__', $Project).Replace('__TEST__', $Test)
    return @{
        native = $template.Replace('__PREREQUISITE__', "`$null = Get-Command dotnet -ErrorAction Stop").Replace('__EXECUTE__', $native)
        docker = $template.Replace('__PREREQUISITE__', "`$null = Get-Command docker -ErrorAction Stop`n    docker info *> `$null`n    if (`$LASTEXITCODE -ne 0) { throw 'Docker daemon unavailable.' }").Replace('__EXECUTE__', $docker)
    }
}

$doc = Get-Content -LiteralPath $ManifestPath -Raw | ConvertFrom-Json -AsHashtable
if ($doc.schemaVersion -ne 1) { throw "Unsupported run-actions manifest schemaVersion: $($doc.schemaVersion)" }
$examples = @($doc.examples)
$utf8NoBom = [Text.UTF8Encoding]::new($false)
$expectedScripts = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
$written = 0

foreach ($example in $examples) {
    $id = [string] $example.id
    if ($id -cnotmatch '^[a-f0-9]{24}$') { throw "Invalid run-action id: $id" }
    $pair = if ($example.strategy -ceq 'repository') {
        New-RepositoryPair -Repository $example.repository -Commit $example.commit -Project $example.project -Test $example.test -Framework $example.framework -Image $example.image
    } elseif ($example.strategy -ceq 'standalone') {
        New-StandalonePair -Manifest $example.manifest -Image $example.image
    } else {
        throw "Unsupported run-action strategy '$($example.strategy)' for $id"
    }
    $dir = Join-Path $OutputRoot $id
    foreach ($transport in @($example.transports)) {
        if ($transport -notin @('native', 'docker')) { throw "Unsupported transport '$transport' for $id" }
        $scriptText = Normalize $pair[$transport]
        $tokens = $null; $errors = $null
        $null = [Management.Automation.Language.Parser]::ParseInput($scriptText, [ref]$tokens, [ref]$errors)
        if ($errors.Count) { throw "Materialized $transport script for $id has syntax errors: $errors" }
        $path = Join-Path $dir "$transport.ps1"
        if (-not (Test-Path -LiteralPath $path) -or [IO.File]::ReadAllText($path) -cne $scriptText) {
            $null = [IO.Directory]::CreateDirectory($dir)
            [IO.File]::WriteAllText($path, $scriptText, $utf8NoBom)
        }
        $null = $expectedScripts.Add("$id/$transport.ps1")
        $written++
    }
}

if (Test-Path -LiteralPath $OutputRoot -PathType Container) {
    foreach ($exampleDir in @(Get-ChildItem -LiteralPath $OutputRoot -Directory)) {
        if ($exampleDir.Name -cnotmatch '^[a-f0-9]{24}$' -or
            ($exampleDir.Attributes -band [IO.FileAttributes]::ReparsePoint)) { continue }
        foreach ($name in @('native.ps1', 'docker.ps1')) {
            if ($expectedScripts.Contains("$($exampleDir.Name)/$name")) { continue }
            $stalePath = Join-Path $exampleDir.FullName $name
            if (Test-Path -LiteralPath $stalePath -PathType Leaf) {
                Remove-Item -LiteralPath $stalePath -Force
            }
        }
        if (-not @(Get-ChildItem -LiteralPath $exampleDir.FullName -Force).Count) {
            Remove-Item -LiteralPath $exampleDir.FullName
        }
    }
}

Write-Host "Materialized $written run-action script(s) for $($examples.Count) example(s) into $OutputRoot."
