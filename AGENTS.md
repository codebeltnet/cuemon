# AGENTS.md

Purpose
- This file guides agentic coding tools working in this repo.
- Follow the repo rules first; do not invent commands or conventions.

Repository Basics
- Workspace root: this file lives in repo root.
- Solution lives at repo root (see `.docfx/index.md`).
- Projects:
  - `src/` = packages shipped to NuGet.
  - `test/` = unit and functional tests.
  - `tuning/` = BenchmarkDotNet benchmarks.
  - `tooling/` = internal tools.

Toolchain
- .NET targets: net10.0, net9.0, netstandard2.0 (source); tests also net48 on Windows.
- Use `dotnet` CLI; CI runs on Linux and Windows.

Build Commands
- Build solution (all): `dotnet build -c Release`
- Build a project: `dotnet build src/Cuemon.Core/Cuemon.Core.csproj -c Release`
- Pack (if needed): `dotnet pack -c Release`

Lint / Analyzers
- No separate lint command is defined.
- Code style is enforced during build (`EnforceCodeStyleInBuild=true`).
- For tests and benchmarks, analyzers are disabled (see `Directory.Build.props`).
- Prefer build to surface style issues: `dotnet build -c Release`.

Test Commands
- Run all tests in a project:
  - `dotnet test test/Cuemon.Core.Tests/Cuemon.Core.Tests.csproj -c Release`
- Run a single test (recommended):
  - `dotnet test test/Cuemon.Core.Tests/Cuemon.Core.Tests.csproj -c Release --filter "FullyQualifiedName~DateSpanTest.Parse_ShouldGetOneMonthOfDifference_UsingIso8601String"`
- Run all tests under `test/`:
  - `dotnet test -c Release test/`

Integration Tests (SQL Server)
- CI runs `test/Cuemon.Data.SqlClient.Tests.csproj` with a SQL Server container.
- Expect a connection string env var:
  - `CONNECTIONSTRINGS__ADVENTUREWORKS` (see CI workflow).
- Use docker compose if you need local parity.

Benchmarks (BenchmarkDotNet)
- Benchmarks live under `tuning/` and are not unit tests.
- Use `tooling/bdn-runner` to run benchmarks when needed.

Cursor Rules
- None found in `.cursor/rules/` or `.cursorrules`.

Copilot Rules (must follow)
- Source: `.github/copilot-instructions.md`.
- Tests must inherit `Codebelt.Extensions.Xunit.Test` and use `using Xunit;`.
- Do NOT use `Xunit.Abstractions` or `using Xunit.Abstractions`.
- Test namespaces MUST match the SUT namespace (no `.Tests` suffix).
- Test class names end with `Test`.
- Do not use `InternalsVisibleTo`; test through public APIs.
- Benchmarks follow the benchmark prompt and naming rules.
- XML doc comments should match existing style for public/protected APIs.

Code Style and Conventions

General Principles (from `.docfx/index.md`)
- Follow Framework Design Guidelines and Microsoft Engineering Guidelines.
- Prefer SOLID, DRY, and separation of concerns.
- Do not duplicate code; apply the boy scout rule.

Formatting
- Indentation: 4 spaces for `.cs` and `.vb` (`.editorconfig`).
- XML files: 2 spaces.
- Many modern style analyzers are disabled; keep existing style in files.

Imports
- Keep `using` directives explicit and minimal.
- Follow existing ordering in the file; do not auto-reorder unless needed.

Types and Language Features
- Avoid style rules that are explicitly disabled in `.editorconfig`:
  - No forced switch expressions, using declarations, or primary constructors.
  - Avoid forced collection expressions for arrays/empty/initializers.
  - Avoid forced expression-bodied members.
- Use explicit types when it improves clarity; do not blindly enforce `var`.

Naming
- Public API naming follows .NET Framework Design Guidelines.
- Test class names end with `Test`; benchmark class names end with `Benchmark`.
- Namespaces for tests/benchmarks match the production namespace exactly.

Error Handling
- Use guard clauses and `Validator.ThrowIfNull` style patterns where present.
- Prefer deterministic, testable error paths; avoid swallowing exceptions.

Extension Methods
- Extension methods belong only in `Cuemon.Extensions.*` projects.
- Non-extension assemblies must hide extension-like APIs via `IDecorator`.

Tests (Unit / Functional)
- Base class: `Test` from `Codebelt.Extensions.Xunit`.
- Use `[Fact]` for unit tests, `[Theory]` for parameterized tests.
- Keep tests deterministic and isolated; prefer fakes/stubs/spies.
- Avoid mocking unless necessary; Moq allowed in special cases.
- Never mock `IMarshaller`; use `new JsonMarshaller()` instead.

Benchmarks
- Namespace matches production namespace; no `.Benchmarks` suffix.
- Place under `tuning/` in matching benchmark project.
- Use `[MemoryDiagnoser]` and `[GroupBenchmarksBy]` where relevant.
- Use deterministic data and `GlobalSetup` for expensive prep.

Docs / XML Comments
- Public and protected members should have XML doc comments.
- Follow existing wording and style; see `.github/copilot-instructions.md`.

Release Notes
- Package release notes live under `.nuget/<PackageName>/PackageReleaseNotes.txt`.
- Keep notes updated for public API changes.

Suggested Workflow for Agents
- Identify correct project location (src/test/tuning/tooling).
- Follow namespace and naming rules before writing code.
- Build or run targeted tests when changing logic.
- Keep changes minimal and consistent with local style.
