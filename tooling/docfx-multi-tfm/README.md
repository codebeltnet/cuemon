# Multi-target DocFX proof of concept

Run from the repository root with PowerShell 7, the .NET 11 SDK, and DocFX 2.81.0.

```powershell
pwsh tooling/docfx-multi-tfm/Test-DocfxMultiTfm.ps1
```

The script prints its temporary workspace. Each round contains the original metadata for each target, consolidated metadata in `api/dotnet`, and the rendered site in `wwwroot`. It uses the repository's filter, matching authored overwrites, templates, xref maps, and generated run-action assets. The file-based consolidator uses YamlDotNet 18.1.0 and runs from the temporary workspace to keep its package pin independent of repository-wide central package management.

## Findings

DocFX 2.81.0 uses Roslyn MSBuildWorkspace 5.9.0. When no global `TargetFramework` is supplied, Roslyn runs a design-time build for each declared target and loads the resulting projects. `OpenProjectAsync` returns the first one. DocFX calls `GetCompilationAsync` for that project, or for the first matching file path already loaded into the workspace, and exports its assembly symbols. The unpinned Cuemon run produced byte-identical YAML to the explicit `net11.0` run. The two modern targets have identical metadata in this experiment. [Roslyn target iteration](https://github.com/dotnet/roslyn/blob/35d9211b841e7613c1d2f8f5af6d628ace696c4c/src/Workspaces/MSBuild/BuildHost/Build/ProjectBuildManager.cs#L219) · [Roslyn project selection](https://github.com/dotnet/roslyn/blob/35d9211b841e7613c1d2f8f5af6d628ace696c4c/src/Workspaces/MSBuild/Core/MSBuild/MSBuildWorkspace.cs#L260) · [DocFX compilation](https://github.com/dotnet/docfx/blob/ba844df625370a6bd7c1ecab0d0ec4d1d9128ac4/src/Docfx.Dotnet/DotnetApiCatalog.Compile.cs)

Multiple metadata groups with distinct destinations produced the correct UID counts in a single process. In the .NET 10 → .NET 11 → .NET Standard 2.0 order, 17 additional shared item records had different `derivedClasses` metadata. The grouped .NET Standard 2.0 output listed `BufferWriterOptions` twice under `StreamEncodingOptions.derivedClasses`. The isolated .NET Standard 2.0 output contained neither entry. `SetDerivedClass` accumulates relationships on a resolver instance held in a static pipeline. A fresh workspace per group therefore still needs process isolation for trustworthy TFM attribution. [DocFX resolver pipeline](https://github.com/dotnet/docfx/blob/ba844df625370a6bd7c1ecab0d0ec4d1d9128ac4/src/Docfx.Dotnet/ManagedReference/Resolvers/YamlMetadataResolver.cs) · [Derived-class resolver](https://github.com/dotnet/docfx/blob/ba844df625370a6bd7c1ecab0d0ec4d1d9128ac4/src/Docfx.Dotnet/ManagedReference/Resolvers/SetDerivedClass.cs)

`docfx merge` exited with a `NullReferenceException` in `HostServiceCreator.LoadModels` for these inputs. Upstream documents the command as broken. Its merge implementation also keeps the first shared declaration and reference payload. Feeding several target assemblies to one metadata job has a similar limitation because the exporter collapses duplicate symbols by UID before writing pages. Copying only missing files would omit framework-specific children and references on existing pages. [Merge CLI documentation](https://dotnet.github.io/docfx/reference/docfx-cli-reference/docfx-merge.html) · [Merge implementation](https://github.com/dotnet/docfx/blob/ba844df625370a6bd7c1ecab0d0ec4d1d9128ac4/src/Docfx.Build.ManagedReference/MergeManagedReferenceDocument.cs) · [Metadata exporter](https://github.com/dotnet/docfx/blob/ba844df625370a6bd7c1ecab0d0ec4d1d9128ac4/src/Docfx.Dotnet/DotnetApiCatalog.ManagedReference.cs)

## Consolidation contract

| Target | UIDs, including namespaces |
|---|---:|
| `net10.0` | 557 |
| `net11.0` | 557 |
| `netstandard2.0` | 548 |
| Union | 561 |

DocFX's managed-reference files group types and their member records, even with `separatePages`. Items, page-local references, and TOC entries all use `uid`. The XML `commentId` is a separate field, and overload group UIDs with a trailing `*` appear as references. The output keeps the first input as the primary representation, retains original UIDs and page filenames, and unions the page items, child lists, page-local references, TOC entries, and UID manifest. DocFX's build step still creates the member and overload HTML pages. Per-item `platform` uses DocFX's existing metadata field. Remarks render the availability in the current templates and carry framework-specific relationship deltas. Source URLs and line numbers are preserved. When Git source metadata exists, its repository-relative path replaces the machine-relative fallback path.

The compatibility target contributes the `System.Runtime.CompilerServices` namespace, `CallerArgumentExpressionAttribute`, its constructor, and its property. The modern targets contribute `BufferWriterOptions` and two members, four Brotli methods, and six `StreamFactory.Create` overloads. Shared declarations and XML comments matched. Three shared types have framework-specific derived types and three exceptions have framework-specific inherited BCL members. Their common relationships stay in the regular lists and the differences appear with the relevant framework labels.

An unexpected difference in syntax, XML documentation, reference payload, or any other shared field fails with the UID, framework, field, and both values. If a future input has a genuine same-UID signature variant, the smallest extension is to collect that UID's syntax by TFM and render labeled syntax blocks on its canonical page. The proof also treats ambiguous filenames, duplicate UIDs, missing inventory entries, and unresolvable child or overload references as errors.

The three documented targets are the milestone scope. MSBuild also reports `netstandard2.1` for Cuemon.IO. An assembly-input probe of a normal .NET Standard 2.1 build confirmed `BufferWriterOptions` and the six overloads there too. The equivalent project-input metadata job hit CS0433 from duplicate Cuemon.Kernel identities, so mixed project target sets need their own validation before integration. The `netstandard2.0` project-input run emits three workspace reference warnings on Windows and Linux. It completes and returns the same filtered UID inventory on both hosts.

## Verification

The test runs extraction, consolidation, and `docfx build --warningsAsErrors` twice. It checks the full UID union against all three source manifests and verifies that every UID resolves to a rendered page and to its anchor when present. It also checks the six overload sections, both representative types, namespace and TOC links, Git source links, and authored examples. A single-target fixture passes through byte for byte. A changed declaration and a missing YAML page both fail before an output is written.

The consolidated metadata uses LF on both hosts. The site comparison normalizes two fields in DocFX's generated `manifest.json`: the absolute `source_base_path` and the order of the `files` records. It compares every record and hashes all other files directly within each host.

Two runs passed on Windows and Ubuntu 26.04 with SDK 11.0.100 and DocFX 2.81.0. The consolidated trees were byte-identical across hosts. The rendered trees matched across hosts after normalizing line endings and the two manifest fields. All 62 staged type overwrite examples appeared on their pages. The existing `codebeltnet/docfx:2` image also rendered the staged metadata with its shared templates, zero warnings, and the same xref map.

## Integration boundary

The local image wrapper currently restores once, fingerprints metadata inputs and outputs, clears generated YAML and manifests, and regenerates all groups in one process on a cache miss. The OCI action installs its pinned CLI on the host, reuses matching restore assets, generates metadata before Docker, and clears generated YAML and manifests after the build in its `finally` block. Docker consumes the generated YAML and runs the site build with the shared templates. [Local wrapper](../../.docfx/BuildDocfxImage.ps1) · [OCI host script](https://github.com/codebeltnet/docfx-oci-build/blob/v1/scripts/build-docfx-oci.ps1) · [Builder Dockerfile](https://github.com/codebeltnet/docfx/blob/main/Dockerfile)

The shared entry point belongs in `docfx-oci-build`. An opt-in metadata group should resolve its project and framework matrix from MSBuild, generate each target in a fresh process, validate the consolidated staging directory, and promote it to the configured destination. The local wrapper can call the same versioned entry point and add its version and selected framework matrix to the cache inputs. Keep the existing path for groups that do not opt in. Before replacing a production group, test cross-group relationships explicitly because the current all-groups pass relies on retained resolver state. The shared builder can continue to receive the same documentation assets and run `docfx build`.
