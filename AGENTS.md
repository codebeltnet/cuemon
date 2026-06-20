# AGENTS.md

This file guides agentic coding tools working in this repo.
Follow the repo rules first; do not invent commands or conventions.

## Repository Layout

- Solution: `Cuemon.slnx` in repo root.
- `src/` — NuGet packages (shipped to nuget.org).
- `test/` — xUnit v3 unit and functional tests.
- `tuning/` — BenchmarkDotNet benchmarks.
- `tooling/` — internal CLI tools.
- `.nuget/<PackageName>/` — per-package `README.md` and `PackageReleaseNotes.txt`.

## Toolchain

- .NET SDK with `LangVersion=latest`.
- Source TFMs: `net10.0;net9.0;netstandard2.0`.
- Test TFMs: `net10.0;net9.0` on Linux; adds `net48` on Windows.
- Benchmark TFMs: `net10.0;net9.0`.
- Central package management via `Directory.Packages.props` (`ManagePackageVersionsCentrally=true`).
- CI runs on Linux (ubuntu-24.04) and Windows (windows-2025), both X64 and ARM64.
- TFM compatibility is mandatory: proposals and code changes must work for all source TFMs. Do not assume `net9.0`/`net10.0` APIs exist in `netstandard2.0`; use conditional compilation (`#if NET9_0_OR_GREATER`) or compatible fallbacks where needed.

## Build Commands

```
dotnet build -c Release                                           # entire solution
dotnet build src/Cuemon.Core/Cuemon.Core.csproj -c Release        # single project
dotnet pack -c Release                                            # pack all NuGet packages
```

## Lint / Analyzers

- No separate lint step; code style is enforced during build (`EnforceCodeStyleInBuild=true` for source projects).
- Analyzers are **disabled** for test and benchmark projects (`RunAnalyzers=false`, `AnalysisLevel=none`).
- Run `dotnet build -c Release` on source projects to surface style violations.

## Test Commands

```
# all tests in one project
dotnet test test/Cuemon.Core.Tests/Cuemon.Core.Tests.csproj -c Release

# single test (recommended when iterating)
dotnet test test/Cuemon.Core.Tests/Cuemon.Core.Tests.csproj -c Release \
  --filter "FullyQualifiedName~DateSpanTest.Parse_ShouldGetOneMonthOfDifference_UsingIso8601String"

# all tests
dotnet test -c Release test/
```

### Integration Tests (SQL Server)

- Project: `test/Cuemon.Data.SqlClient.Tests/Cuemon.Data.SqlClient.Tests.csproj`.
- Requires env var `CONNECTIONSTRINGS__ADVENTUREWORKS`.
- CI spins up SQL Server via `docker-compose.yml`; use the same locally.

### Benchmarks

- Live under `tuning/`; run with `tooling/bdn-runner`.
- Not unit tests; do not include in test runs.

## Cursor / Copilot Rules

- No Cursor rules (`.cursor/rules/` and `.cursorrules` are absent).
- Copilot rules live in `.github/copilot-instructions.md` — **must follow**.

## Code Style and Conventions

### General Principles
- Follow Framework Design Guidelines and Microsoft Engineering Guidelines.
- Adhere to SOLID, DRY, separation of concerns.
- Apply the boy scout rule; do not duplicate code.

### Formatting
- 4 spaces for `.cs` / `.vb`; 2 spaces for `.xml` (`.editorconfig`).
- Keep existing style in files; many modern analyzers are explicitly disabled.

### Namespace Style
- **Prefer file-scoped namespaces** (`namespace Cuemon.Foo;`) for new files.
- The current majority of the codebase uses **block-scoped namespaces** — do not convert existing files unless explicitly asked.
- When editing an existing file, follow whichever style that file already uses.
- **Never use top-level statements.** Always use explicit class declarations with a proper namespace.

### Disabled Analyzers (key rules — do NOT introduce these patterns)

| Rule | What it forces | Why disabled |
|------|---------------|--------------|
| IDE0066 | switch expressions | style consistency |
| IDE0063 | using declarations | style consistency |
| IDE0290 | primary constructors | style consistency |
| IDE0022 | expression-bodied methods | style consistency |
| IDE0300/0301/0028/0305 | collection expressions | netstandard2.0 compat |
| CA1846/1847/1865-1867 | Span/char overloads | netstandard2.0 compat |
| IDE0330 | `System.Threading.Lock` | requires net9.0+ |
| Performance category | various | netstandard2.0 compat |

### Imports
- Keep `using` directives explicit and minimal.
- Follow existing ordering; do not auto-reorder.

### Types and `var`
- Do not blindly enforce `var`; use explicit types when it improves clarity.
- IDE0008 (use explicit type) is disabled — either form is acceptable.

### Naming
- Public API naming follows .NET Framework Design Guidelines.
- Test classes end with `Test`; benchmark classes end with `Benchmark`.
- Namespaces for tests and benchmarks **must match the production namespace exactly** (no `.Tests` / `.Benchmarks` suffix). Override `<RootNamespace>` in `.csproj`.

### Error Handling
- Use guard clauses and `Validator.ThrowIfNull` patterns.
- Prefer deterministic, testable error paths; never swallow exceptions.

### Extension Methods
- Extension methods belong **only** in `Cuemon.Extensions.*` projects.
- Non-extension assemblies may expose similar APIs only behind the `IDecorator` interface.

## Writing Tests

- Test framework: **xUnit v3** (`xunit.v3` package).
- Base class: `Test` from `Codebelt.Extensions.Xunit`.
- **Do NOT** use `Xunit.Abstractions` or `using Xunit.Abstractions` (removed in xUnit v3).
- Constructor signature: `public FooTest(ITestOutputHelper output) : base(output) { }`.
- Use `TestOutput.WriteLine(...)` for output, inherited from `Test`.
- Use `[Fact]` for unit tests, `[Theory]` with `[InlineData]` for parameterized tests.
- Assertions: xUnit `Assert.*` methods only.
- Keep tests deterministic and isolated; prefer fakes/stubs/spies.
- Mocking (Moq) only under special circumstances; **never mock `IMarshaller`** — use `new JsonMarshaller()`.
- Do NOT use `InternalsVisibleTo`; test through public APIs (Public Facade Testing pattern).
- Assembly naming: `Cuemon.Foo.Tests` for unit tests, `Cuemon.Foo.FunctionalTests` for functional tests.

### Test File Template

```csharp
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Foo // matches SUT namespace exactly
{
    public class BarTest : Test
    {
        public BarTest(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void Method_ShouldExpectedBehavior_WhenCondition()
        {
            // Arrange / Act / Assert
        }
    }
}
```

## Writing Benchmarks

- Place in `tuning/` in a `*.Benchmarks` project; namespace matches production (no `.Benchmarks` suffix).
- Use `[MemoryDiagnoser]`, `[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]`.
- Use `[GlobalSetup]` for expensive prep; keep measured methods focused.
- Use `[Params]` for multiple input sizes; use deterministic data; avoid external systems.
- Mark one method `Baseline = true`; use descriptive `Description` values.

## XML Documentation

- All public and protected members should have XML doc comments.
- Follow existing wording and style in the codebase.
- See `.github/copilot-instructions.md` for detailed examples.

## Release Notes

- Per-package notes in `.nuget/<PackageName>/PackageReleaseNotes.txt`.
- Keep updated for public API changes.

## Commit Style (Gitmoji)

This repo uses **gitmoji** commit messages — do **not** use Conventional Commits (`feat:`, `fix:`, etc.).

Format: `<emoji> <subject>`

**Always use the actual Unicode emoji character**, not the GitHub shortcode (e.g., use `✨` not `:sparkles:`).

Example: `✨ Add DateSpan.TryParse overload`

### Common Gitmojis

| Emoji | Use for |
|-------|---------|
| ✨ | New feature |
| 🐛 | Bug fix |
| ♻️ | Refactoring |
| ✅ | Adding / updating unit test / functional test |
| 📝 | Documentation |
| ⚡ | Performance improvement |
| 🎨 | Code style / formatting |
| 🔥 | Removing code or files |
| 🚧 | Work in progress |
| 📦 | Package / dependency update |
| 🔧 | Configuration / tooling |
| 🚚 | Moving / renaming files |
| 💥 | Breaking change |
| 🩹 | Non-critical fix |

### Rules

1. **One emoji per commit** — each commit has exactly one primary gitmoji.
2. **Be specific** — choose the most appropriate emoji, not a generic one.
3. **Consistent scope** — use consistent scope names across commits.
4. **Clear messages** — the subject line should be understandable without a body.
5. **Atomic commits** — each commit should be independently buildable and testable.

## Git Operations Safeguards

Agents must never automatically commit code changes or push to remote repositories. Both actions require explicit user approval:

- **Commits**: Always request confirmation from the user before staging and committing code. Present a clear summary of the changes and wait for approval before executing the commit.
- **Remote Operations**: Do not push, pull, fetch, or interact with `origin` or any remote repository without explicit user instruction. These operations modify repository history and can cause data loss if performed unexpectedly.

**Rationale:** Automatic commits can clutter history with incomplete work, temporary debugging code, or unintended changes. Unexpected remote operations risk overwriting or losing commits on shared branches. Always require explicit user approval before performing these actions.

## Agent Workflow

1. Identify the correct project area (`src/`, `test/`, `tuning/`, `tooling/`).
2. Follow namespace and naming rules **before** writing any code.
3. Before potentially refactoring any code, verify the code in question is well tested; if coverage is missing, add or update tests first to reduce regression risk.
4. Build the affected source project to check for style violations.
5. Run targeted tests when changing logic.
6. Keep changes minimal and consistent with existing local style.

<!-- dotnet-docfx-digest:start -->
## DocFX Documentation Maintenance

When changing public .NET APIs, keep the DocFX documentation current in the same change set.

Documentation updates must cover public API only. Do not document private or internal types or members. Do not create namespace overview pages for namespaces that contain no public API.

Public non-abstraction types — including enums, structs, records, plain classes, and static extension containers — are valid documentation targets. Generic public types and generic extension methods are valid documentation targets too. Do not exclude a type solely because it is generic or because reflection reports it as abstract and sealed (that is the IL pattern for a static class).

For public non-abstraction types, include at least one realistic, copy/paste-ready usage example on the generated type page/overwrite section for that type UID. For example, a public `Class1` requires an example on the `Class1` API page, not only on the namespace page. Prefer deriving examples from existing unit, functional, or integration tests, but convert test code into real-life consumer-oriented usage.

Missing type examples must be added through per-type DocFX overwrite files under `.docfx/api/types/{TypeUid}.md` in Codebelt repositories. Namespace overview text and `Extension Members` tables are not substitutes for type-page examples.

Public extension methods must have examples too. Listing an extension method in an `Extension Members` table is required, but it is not enough.

All added or changed code samples must be deterministic and verified to compile. Do not add pseudo-code, ellipses, hidden test helpers, or examples that rely on unverified behavior.

Every namespace containing public API must have a DocFX namespace overview page named after the namespace, such as `X.Y.Z.md`, under `.docfx/api/namespaces/`, using DocFX overwrite front matter with the namespace `uid`.

Namespaces exposing public extension methods must document those extension members at namespace level. The namespace page must include an `Extension Members` table listing the extended type, the extension marker, and the public extension methods. Extension members are rendered under the heading `Extension Members`.

Both namespace overwrite files and type overwrite files are required deliverables in the same run. Generating only namespace pages or only type pages is incomplete.

`docfx.json` must keep namespace and type overwrite files in separate subdirectories. `build.overwrite` must include both `api/namespaces/**/*.md` (for namespace pages) and `api/types/**/*.md` (for type pages). `build.content` must exclude both `api/namespaces/**` and `api/types/**` to prevent overwrite Markdown from being treated as conceptual content. Do not use `api/**/*.md` under `build.overwrite` or `build.content`.

Availability must be documented by referencing the appropriate include file when one exists, or by adding explicit availability text when no suitable include exists. Availability must reflect the actual target frameworks, conditional compilation, and project configuration.

Preserve manual documentation edits. Prefer additive changes, but correct stale or contradictory information so documentation remains accurate.

Preserve working Markdown links, `Related:` references, and historical URL citations during prose rewrites. Remove or replace a URL only after directly verifying that the current destination returns HTTP 404. Timeouts, 403s, rate limits, DNS failures, and other lookup problems are not removal evidence.

Interim scratch artifacts do not belong in the repository working tree. Store assessment queues, project manifests, review reports, captured validator output, progress notes, and one-off helper scripts in temp or session storage instead. New working-tree files are only legitimate when they are the managed `AGENTS.md` block, the active `docfx.json`, or DocFX-authored namespace/type Markdown that maps to a real public namespace or type. Everything else is blocking cleanup work, not a documentation deliverable. The validator auto-detects generic-arity type families (such as `MutableTuple`1`..`MutableTuple`N`) and skips redundant sibling examples from the public API surface alone, so no manifest or skip file is ever written into the repository.

Before completing documentation work, run the relevant verification commands, normally:

```bash
dotnet build
dotnet test
dotnet run --file skills/dotnet-docfx-digest/scripts/docfx.cs -- --repo-root . --verify-docfx-build
```

Codebelt repositories are normally strong-name signed with a `.snk` file in the repository root on the main author's codespace. Preserve and copy that root `.snk` file when building a temporary copy. If the repository or temp copy has no root `.snk`, run build and test verification with `-p:SkipSignAssembly=true`, for example `dotnet build -p:SkipSignAssembly=true` and `dotnet test -p:SkipSignAssembly=true`.

The DocFX build verification must run outside the working tree when possible. The `--verify-docfx-build` option copies the repository to a temp workspace, runs DocFX against the resolved `docfx.json` there, and removes the temp workspace afterward so generated API YAML, manifest files, and site output do not flood git status.

If a command cannot be run, report the exact limitation or failure instead of claiming the documentation was verified.
<!-- dotnet-docfx-digest:end -->
