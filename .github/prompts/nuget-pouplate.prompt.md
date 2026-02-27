---
mode: agent
description: 'Prompt for populating commits to PackageReleaseNotes.txt files under .nuget/**'
---

Purpose: deterministic, low-analysis instructions so automated runs populate release-note change bullets for the current unreleased version block.

Behavior (exact):
- For every file matching `.nuget/**/PackageReleaseNotes.txt`:
	1. Read line 1 and extract `current-version` from `Version: x.y.z` (strict semantic version format).
	2. Find the next line below line 1 that matches `Version:` and extract `previous-version` from `Version: x.y.z`.
	3. If either version cannot be extracted, do nothing for that file.
	4. Define the editable range as the lines after the first `Availability:` line in the current block and before the next `Version:` match (`previous-version`).
	5. Resolve release anchor tag by matching `previous-version` to git tag `v<previous-version>` first, then `<previous-version>`.
	6. If neither tag exists, do nothing for that file.
	7. Collect commits for that package from `tag(previous-version)..HEAD` using path scope derived from the package name:
		 - Package name = folder name under `.nuget/` for the file.
		 - Primary source path scope: `src/<PackageName>/**`.
	8. Convert commits to release-note bullets using the existing release-note style and headings in that file.
	9. Replace only the change-content area inside the current block range:
		 - Keep `Version:` and `Availability:` unchanged.
		 - Keep `# ALM` section untouched if already present.
		 - Populate or update `# New Features`, `# Improvements`, and `# Bug Fixes` as needed.
		 - Do not modify any content outside the current block range.
	10. Save the file in-place and continue to the next file.

Transformation rules (strict):
- Use heading names exactly: `# New Features`, `# Improvements`, `# Bug Fixes`.
- Use bullet style exactly: `- <VERB> <summary>` where `<VERB>` is uppercase and one of `ADDED`, `EXTENDED`, `CHANGED`, `OPTIMIZED`, `FIXED`, `REMOVED`.
- Keep one bullet per logical change; deduplicate repeated commit messages.
- Prefer imperative, product-facing summaries over raw commit text.
- Mention concrete type/member names and namespace when identifiable, matching existing tone.
- Preserve NBSP-only spacer lines (`U+00A0`) between sections.
- Alaways end with a newline followed by spacer lines (`U+00A0`) between sections.
- Do not reorder historical sections and do not rewrite previous version blocks.

Tag and range rules:
- The comparison baseline is always `previous-version` (the next `Version:` in the same file), not `current-version`.
- Commit range is `tag(previous-version)..HEAD`.
- Ignore merge commits unless they contain meaningful release-note content not present in child commits.
- If range contains no relevant commits for the scoped paths, leave the current block unchanged.

Notes:
- Do not infer target versions from changelog text; parse only explicit `Version: x.y.z` lines.
- Keep edits minimal and strictly inside the current version block.
- DO NOT REMOVE THE ASCII 0xA0 NBSP CHARACTERS OR RUN ANY SORT OF TRIM on spacer lines.
- Do not open PRs or create branches.

Example run command (agent):
`run: /nuget-populate`
