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
- Source TFMs: `net11.0;net10.0;netstandard2.0`.
- Test TFMs: `net11.0;net10.0` on Linux; adds `net48` on Windows.
- Benchmark TFMs: `net11.0;net10.0`.
- Central package management via `Directory.Packages.props` (`ManagePackageVersionsCentrally=true`).
- CI runs on Linux (ubuntu-24.04) and Windows (windows-2025), both X64 and ARM64.
- TFM compatibility is mandatory: proposals and code changes must work for all source TFMs. Do not assume `net9.0`/`net10.0` APIs exist in `netstandard2.0`; use conditional compilation (`#if NET10_0_OR_GREATER`) or compatible fallbacks where needed.

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

## Agent Workflow

1. Identify the correct project area (`src/`, `test/`, `tuning/`, `tooling/`).
2. Follow namespace and naming rules **before** writing any code.
3. Before potentially refactoring any code, verify the code in question is well tested; if coverage is missing, add or update tests first to reduce regression risk.
4. Build the affected source project to check for style violations.
5. Run targeted tests when changing logic.
6. Keep changes minimal and consistent with existing local style.
