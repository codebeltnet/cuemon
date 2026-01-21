---
mode: agent
description: 'Prompt for populating PackageReleaseNotes.txt files under .nuget/**'
params:
  version: '10.0.0'
---

Purpose: deterministic, low-analysis instructions so automated runs prepend a single, consistent release block.

Behavior (exact):
- For every file matching `.nuget/**/PackageReleaseNotes.txt`:
  1. Read the file and find the first line that starts with `Availability:` (case-sensitive).
  2. If found, capture the remainder of that line as `previous-tfm` and prepend the exact template shown below (substituting `{{version}}` and `{{previous-tfm}}`).
  3. If not found within the first 3 lines, do nothing for that file.
  4. Apply the template exactly as shown, preserving all whitespace and blank lines (including NBSP U+00A0 characters).
  5. Save the file in-place - do not open PRs or create branches.
  6. Continue to the next file until all matching files have been processed.
  7. Do not assume that each file are the same - process each file independently.

Exact template to prepend (preserve whitespace and trailing NBSP U+00A0 on the blank lines):
```
Version {{version}}
Availability: {{previous-tfm}}
 
# ALM
- CHANGED Dependencies have been upgraded to the latest compatible versions for all supported target frameworks (TFMs)
 
```
Notes:
- Do not attempt to infer versions or parse changelogs — use the provided `params.version` value.
- Keep edits minimal: only prepend the template block; preserve the rest of the file unchanged for human interference.
- DO NOT REMOVE THE ASCII 0xA0 NBSP CHARACTERS OR RUN ANY SORT OF TRIM on the blank lines in the template!
- DO NOT RUN ANY SORT OF GIT COMMANDS - once the files are saved, you are done.

Example run command (agent):
`run: /nuget version={{version}}`
