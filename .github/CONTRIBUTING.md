# Contributing to Cuemon for .NET

This repository is part of the Codebelt .NET library estate. The instructions below describe the current checkout and its CI contract. Please keep changes focused and preserve the shared Codebelt build skeleton unless a deliberate policy change is being made.

## Before you start

- Read the repository `README.md` and open an issue before starting a non-trivial feature or behavioral change.
- Use an installed .NET SDK that can build the target frameworks listed below. This repository currently targets: **net10.0, net48, net9.0, netstandard2.0, netstandard2.1**.
- The solution is `Cuemon.slnx`. Central package versions are maintained in `Directory.Packages.props`.
- The shared build behavior is in `Directory.Build.props` and `Directory.Build.targets`; repository-specific TFMs, package references and metadata remain local to this library.

## Repository shape

- `src/` contains production projects.
- `test/` contains xUnit v3 test projects.
- `Cuemon.slnx` is the solution used for local development.
- `.github/workflows/ci-pipeline.yml` is the CI workflow and the authority for the test matrix.
- `testenvironments.json` declares the supported `WSL-Ubuntu` and `Docker-Ubuntu` test environments.

## Build

Restore and build the solution from the repository root:

```powershell
dotnet restore "Cuemon.slnx"
dotnet build "Cuemon.slnx" --configuration Release --no-restore
```

CI builds both Debug and Release configurations. A clean build should complete before opening a pull request.

## Test

Run tests one project at a time so a failing or hanging project is attributable. This mirrors the CI matrix; it does not silently turn skipped integration tests into passing tests.

```powershell
$testProjects = Get-ChildItem test -Filter *.csproj -Recurse
$testProjects = $testProjects | Where-Object { $_.BaseName -notin @('Cuemon.Data.SqlClient.Tests') }
foreach ($project in $testProjects) {
    dotnet test --project $project.FullName --configuration Release --no-restore
}
```

The CI test plan currently runs **38** project(s) and excludes **1** project(s). The workflow also has an optional macOS test job.

## Integration and infrastructure

- `WSL-Ubuntu` — WSL distribution `Ubuntu-24.04`.
- `Docker-Ubuntu` — Docker image `codebeltnet/ubuntu-testrunner:8-9-10-11`.

This repository has `docker-compose.yml` with these services: `database`.

Start the services before running the opt-in integration tests and remove them afterwards:

```powershell
docker compose up -d
docker compose down
```

The normal CI test matrix excludes these integration-dependent projects:
- `Cuemon.Data.SqlClient.Tests`

## Package and documentation

Create packages using the same solution and Release configuration:

```powershell
dotnet pack "Cuemon.slnx" --configuration Release --no-restore
```

Package-specific release notes live under `.nuget/<ProjectName>/PackageReleaseNotes.txt` and package README files live beside them. `Directory.Build.targets` imports the release notes during packing. Public API changes also require XML documentation updates; DocFX documentation is built by the repository automation.

## Pull requests

1. Create or join an issue before substantial work, then fork the repository and create a branch from `main`.
2. Add or update focused tests and public API documentation where applicable.
3. Run restore, build, and the relevant per-project tests locally.
4. Keep the pull request small, explain the behavior change and validation performed, and wait for the CI checks to pass.

## Issues

Include the affected project, target framework, operating system, SDK version, exact command, expected result, actual result, and a minimal reproduction. Identify whether the behavior differs between local Windows/WSL, Docker-Ubuntu and GitHub Actions.

## Coding guidelines

Follow the existing style, the Framework Design Guidelines, the repository `.editorconfig`, and the shared Codebelt conventions. Do not make unrelated formatting or infrastructure changes in a feature pull request.

## License

By contributing to Cuemon for .NET, you agree that your contributions will be licensed under the MIT license.
