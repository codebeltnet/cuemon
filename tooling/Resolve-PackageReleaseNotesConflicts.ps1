#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Resolves merge conflicts in all PackageReleaseNotes.txt files.

.DESCRIPTION
    During a merge, PackageReleaseNotes.txt files accumulate standard
    conflict markers. This script resolves all of them by keeping both
    sides — current branch (A) on top, incoming (B) below — and stages
    the resolved files with `git add`.

.EXAMPLE
    # Run from the repo root while a merge is in progress:
    .\tooling\Resolve-PackageReleaseNotesConflicts.ps1
#>

$files = git diff --name-only --diff-filter=U | Where-Object { $_ -match "PackageReleaseNotes\.txt" }

if (-not $files) {
    Write-Host "No conflicted PackageReleaseNotes.txt files found."
    exit 0
}

$count = 0
foreach ($file in $files) {
    $content = Get-Content $file -Raw
    $resolved = $content `
        -replace '(?m)^<<<<<<< .+\r?\n', '' `
        -replace '(?m)^=======\r?\n', "`n" `
        -replace '(?m)^>>>>>>> .+\r?\n', ''
    [System.IO.File]::WriteAllText((Resolve-Path $file), $resolved)
    git add $file
    $count++
    Write-Host "Resolved: $file"
}

Write-Host ""
Write-Host "Done. Resolved and staged $count file(s)."
