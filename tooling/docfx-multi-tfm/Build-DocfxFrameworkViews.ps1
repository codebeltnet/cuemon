#requires -Version 7.0
[CmdletBinding()]
param(
    [string] $DefaultFramework = 'net11.0',
    [string[]] $Framework,
    [string] $SharedTemplatePath,
    [string] $BuilderImage = 'codebeltnet/docfx:2',
    [switch] $BuildInContainer
)

$ErrorActionPreference = 'Stop'
$PSNativeCommandUseErrorActionPreference = $false
$repo = (Resolve-Path "$PSScriptRoot/../..").Path
$source = "$repo/.docfx"
$work = Join-Path ([IO.Path]::GetTempPath()) "cuemon-docfx-native-$([guid]::NewGuid().ToString('N'))"
New-Item -ItemType Directory $work | Out-Null
Write-Host "Artifacts: $work"

function Invoke-BuildStep([string] $Log, [string] $Command, [string[]] $Arguments) {
    & $Command @Arguments *> $Log
    if ($LASTEXITCODE -ne 0) {
        Get-Content $Log -Tail 25 | Write-Host
        throw "$Command exited with $LASTEXITCODE. See $Log"
    }
}

function Write-Json([string] $Path, $Value) {
    ConvertTo-Json -InputObject $Value -Depth 40 | Set-Content -LiteralPath $Path
}

function Read-Json([string] $Path) {
    Get-Content -LiteralPath $Path -Raw | ConvertFrom-Json -AsHashtable
}

$config = Read-Json "$source/docfx.json"
# The PoC opts in one authored group and two of its project globs.
$group = @($config.metadata | Where-Object dest -ceq 'api/dotnet')
if ($group.Count -ne 1 -or $group[0].src.Count -ne 1) { throw 'Expected one api/dotnet group with one source root.' }
$group = $group[0]
$api = $group.dest
$sourceRoot = (Resolve-Path (Join-Path $source $group.src[0].src)).Path
$projects = [ordered]@{}
foreach ($name in 'Cuemon.Kernel', 'Cuemon.IO') {
    $relative = "$name/$name.csproj"
    if (-not ($group.src[0].files | Where-Object { $relative -clike $_ })) { throw "$relative is outside the authored group." }
    $evaluation = & dotnet msbuild "$sourceRoot/$relative" -nologo -getProperty:TargetFrameworks,TargetFramework
    if ($LASTEXITCODE -ne 0) { throw "MSBuild evaluation failed for $relative." }
    $properties = ($evaluation | Out-String | ConvertFrom-Json).Properties
    $projects[$relative] = @("$($properties.TargetFrameworks);$($properties.TargetFramework)" -split ';' | Where-Object { $_ } | Select-Object -Unique)
    if (-not $projects[$relative].Count) { throw "No target frameworks for $relative." }
    Write-Host "$relative declares $($projects[$relative] -join ', ')"
    Invoke-BuildStep "$work/$name.restore.log" dotnet @('restore', "$sourceRoot/$relative")
}

$matrix = @($projects.Values | ForEach-Object { $_ } | Sort-Object -Unique -CaseSensitive)
if ($Framework) {
    foreach ($target in $Framework) {
        if ($target -cnotin $matrix) { throw "Undeclared framework: $target" }
    }
    $matrix = @($matrix.Where({ $_ -cin $Framework }))
}
if ($DefaultFramework -cnotin $matrix) { throw "Default framework $DefaultFramework is outside the selected matrix." }

$shared = "$work/shared"
New-Item -ItemType Directory $shared | Out-Null
if ($SharedTemplatePath) {
    Copy-Item -LiteralPath (Resolve-Path $SharedTemplatePath).Path -Destination "$shared/ms-style" -Recurse
} else {
    Invoke-BuildStep "$work/templates.log" docker @('run', '--rm', '--mount', "type=bind,source=$shared,target=/stage",
        $BuilderImage, 'cp', '-R', '/opt/docfx/templates/ms-style', '/stage/ms-style')
}
$template = "$shared/ms-style/layout/_master.tmpl"
$layout = Get-Content -LiteralPath $template -Raw
$entryPoint = '<script type="module" src="./{{_rel}}public/docfx.min.js"></script>'
if (!$layout.Contains($entryPoint)) { throw 'The shared template no longer has the expected DocFX module entry point.' }
$adapter = @'
<meta name="docfx:framework-api" content="/{{_frameworkApi}}/">
      <meta name="docfx:framework-directory" content="{{_frameworkDirectory}}">
      <script src="{{_rel}}public/framework-view.js"></script>
'@
$layout.Replace($entryPoint, "$adapter$([Environment]::NewLine)      $entryPoint") | Set-Content -LiteralPath $template -NoNewline
Copy-Item "$PSScriptRoot/framework-view.js" "$shared/ms-style/public"
& "$source/BuildRunActions.ps1" -OutputRoot "$work/run-actions"
& "$source/TestRunActions.ps1" -ScriptsRoot "$work/run-actions"

$views = [ordered]@{}
foreach ($tfm in $matrix) {
    if ($tfm -cnotmatch '^(net|netstandard|netcoreapp)(\d+)\.(\d+)$') { throw "No public view mapping for $tfm." }
    $view = "$($Matches[1])-$($Matches[2]).$($Matches[3])"
    $directory = if ($Matches[1] -ceq 'net' -and $Matches[3] -eq '0') { "net$($Matches[2])" } else { $tfm }
    $stage = "$work/$directory"
    New-Item -ItemType Directory $stage | Out-Null
    Copy-Item -LiteralPath "$source/includes", "$source/images", "$source/templates", "$source/index.md", "$work/run-actions" -Destination $stage -Recurse
    Copy-Item -LiteralPath "$shared/ms-style" -Destination "$stage/templates/ms-style" -Recurse
    Copy-Item -LiteralPath (Join-Path $source $group.filter) -Destination (Join-Path $stage $group.filter)
    $selected = @($projects.Keys | Where-Object { $tfm -cin $projects[$_] })
    $excluded = @($projects.Keys | Where-Object { $_ -cnotin $selected })
    Write-Host "$view includes $($selected -join ', ')"
    if ($excluded) { Write-Host "No $tfm target declared by $($excluded -join ', ')" }

    $metadata = (ConvertTo-Json $group -Depth 30 | ConvertFrom-Json -AsHashtable)
    $metadata.src[0].src = $sourceRoot
    $metadata.src[0].files = $selected
    if (!$metadata.properties) { $metadata.properties = @{} }
    $metadata.properties.TargetFramework = $tfm
    $signingArguments = @()
    if (!(Test-Path -LiteralPath "$repo/cuemon.snk" -PathType Leaf)) {
        $metadata.properties.SkipSignAssembly = 'true'
        $signingArguments = @('-p:SkipSignAssembly=true')
    }
    if ($excluded) {
        # Match Cuemon's MinVer-derived assembly identity to its on-disk dependencies.
        # The ordinary build lets MSBuild resolve compatible dependency TFMs.
        $versions = foreach ($project in $selected) {
            $name = [IO.Path]::GetFileNameWithoutExtension($project)
            $buildArguments = @('build', "$sourceRoot/$project", '-c', 'Release', '-f', $tfm, '--no-restore') + $signingArguments
            Invoke-BuildStep "$stage/$name.build.log" dotnet $buildArguments
            $version = & dotnet msbuild "$sourceRoot/$project" -nologo -p:Configuration=Release "-p:TargetFramework=$tfm" -t:GetAssemblyVersion -getProperty:AssemblyVersion @signingArguments
            if ($LASTEXITCODE -ne 0 -or $version -notmatch '^\d+(\.\d+){3}$') { throw "Cannot evaluate assembly version for $project." }
            $version
        }
        if (@($versions | Select-Object -Unique).Count -ne 1) { throw 'The Cuemon mixed-target workaround requires a common assembly version.' }
        $metadata.properties.AssemblyVersion = @($versions)[0]
    }
    Write-Json "$stage/metadata.json" @{ metadata = @($metadata) }
    Invoke-BuildStep "$stage/metadata.log" docfx @('metadata', "$stage/metadata.json", '--noRestore')
    $inventory = Read-Json "$stage/$api/.manifest"
    if (!$inventory.Count) { throw "Empty DocFX metadata for $view." }

    foreach ($kind in 'namespaces', 'types') {
        $destination = "$stage/api/$kind"
        New-Item -ItemType Directory $destination | Out-Null
        Get-ChildItem -LiteralPath "$source/api/$kind" -Filter '*.md' |
            Where-Object { $inventory.Contains($_.BaseName) } | Copy-Item -Destination $destination
    }

    $build = (ConvertTo-Json $config.build -Depth 40 | ConvertFrom-Json -AsHashtable)
    $build.Remove('sitemap')
    $build.Remove('xref') # The production Cuemon xref would link back to APIs absent from this view.
    $build.content = @(@{ files = @("$api/**/*.yml", 'index.md', 'toc.yml') })
    $build.globalMetadata._frameworkApi = $api
    $build.globalMetadata._frameworkDirectory = $directory
    $build.globalMetadata.Remove('_googleAnalyticsTagId')
    @"
- name: Concept Reference
  href: index.md
- name: .NET API
  href: $api/Cuemon.IO.yml
"@ | Set-Content "$stage/toc.yml"
    Write-Json "$stage/docfx.json" @{ build = $build }
    if ($BuildInContainer) {
        Invoke-BuildStep "$stage/build.log" docker @('run', '--rm', '--mount', "type=bind,source=$stage,target=/stage",
            '-w', '/stage', $BuilderImage, 'docfx', 'build', '--warningsAsErrors')
    } else {
        Invoke-BuildStep "$stage/build.log" docfx @('build', "$stage/docfx.json", '--warningsAsErrors')
    }
    $views[$view] = [ordered]@{
        tfm = $tfm; directory = $directory; xref = "/xrefmap.yml?view=$view"; projects = $selected
    }
    Write-Host "$view rendered $($inventory.Count) metadata UIDs"
}

$defaultView = @($views.Keys.Where({ $views[$_].tfm -ceq $DefaultFramework }))[0]
$defaultDirectory = $views[$defaultView].directory
$site = "$work/site"
Copy-Item -LiteralPath "$work/$defaultDirectory/$($config.build.dest)" -Destination $site -Recurse
$routes = foreach ($view in $views.Keys) {
    $directory = $views[$view].directory
    if ($view -ceq $defaultView) {
        '    ~^{0}$ "";' -f [regex]::Escape($view)
        continue
    }
    $native = "$work/$directory/$($config.build.dest)"
    $destination = "$site/_frameworks/$directory"
    $apiParent = Split-Path -Parent "$destination/$api"
    New-Item -ItemType Directory $apiParent | Out-Null
    Copy-Item "$native/$api" "$destination/$api" -Recurse
    Copy-Item "$native/index.json", "$native/xrefmap.yml" -Destination $destination
    # Browser-relative dependencies outside the API subtree use the default site.
    foreach ($asset in Get-ChildItem "$native/public", "$native/images", "$native/run-actions" -Recurse -File) {
        $relative = [IO.Path]::GetRelativePath($native, $asset.FullName)
        if (!(Test-Path -LiteralPath "$site/$relative") -or
            (Get-FileHash -LiteralPath "$site/$relative").Hash -cne (Get-FileHash -LiteralPath $asset.FullName).Hash) {
            throw "Cannot share $relative with $view."
        }
    }
    '    ~^{0}$ /_frameworks/{1};' -f [regex]::Escape($view), $directory
}
$searchRoutes = foreach ($view in $views.Keys) {
    $directory = $views[$view].directory
    $root = if ($view -ceq $defaultView) { '""' } else { "/_frameworks/$directory" }
    '    ~^{0}$ {1};' -f [regex]::Escape($directory), $root
}
Write-Json "$site/framework-views.json" ([ordered]@{ schemaVersion = 1; apiPath = "/$api/"; defaultView = $defaultView; views = $views })
(Get-Content "$PSScriptRoot/nginx.conf" -Raw).Replace('__API__', "/$api/").
    Replace('__VIEWS__', ($routes -join [Environment]::NewLine)).Replace('__SEARCH__', ($searchRoutes -join [Environment]::NewLine)) |
    Set-Content "$work/nginx.conf"
Write-Host "Site: $site"
