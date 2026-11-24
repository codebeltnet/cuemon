# Native DocFX framework views

The fixture builds Cuemon.Kernel and Cuemon.IO independently for each framework through `docfx-oci-build` and the shared `ms-style` template. NGINX serves the selected view under the same public API pathname. The default is .NET 11.

## Run

Use PowerShell 7, the .NET 11 SDK, and DocFX 2.81.0 installed as a .NET tool and available on `PATH`. The fixture downloads the shared implementation by commit ID. It also downloads a pinned template override while the published builder image lacks the selector. Both downloads are staged in a temporary directory. Metadata runs on the host. Pass `-BuildInContainer` to render HTML in the pinned builder image. The HTTP regression uses Docker with Linux containers.

```powershell
$work = Join-Path ([IO.Path]::GetTempPath()) "cuemon-framework-views-$([guid]::NewGuid().ToString('N'))"
& tooling/docfx-multi-tfm/Build-DocfxFrameworkViews.ps1 -OutputRoot $work -TestSharedImplementation
& tooling/docfx-multi-tfm/Test-DocfxFrameworkViews.ps1 -Workspace $work
```

Pass `-Framework net11.0,netstandard2.0` to build two views, or `-Framework net11.0` to build a single view. Include the default in the matrix. Build again and pass `-CompareWorkspace` with the other artifacts directory to compare isolated metadata and the packaged site. The comparison normalizes line endings, the DocFX build path and manifest file order, search record order, and machine-relative metadata source paths when DocFX supplies a corresponding Git path. `Test-DocfxFrameworkViews.ps1` also accepts `-Origin` for an already running NGINX instance.

In PR Flow, enable `run_docfx_framework_views` to run the same fixture entry point on Windows and Linux. `-TestSharedImplementation` also exercises the shared builder's synthetic regression for exclusions, group destinations, and cache recovery. The fixture covers the two-project baseline. The production build remains `.\.docfx\BuildDocfxImage.ps1`.

To run the browser regression, install Playwright and Chromium outside the checkout, make the Playwright module available to Node, and add `-Browser` to the test command.

```powershell
$deps = Join-Path ([IO.Path]::GetTempPath()) 'cuemon-docfx-browser'
npm install --prefix $deps playwright@1.61.1
node "$deps/node_modules/playwright/cli.js" install chromium
$env:NODE_PATH = "$deps/node_modules"
& tooling/docfx-multi-tfm/Test-DocfxFrameworkViews.ps1 -Workspace $work -Browser
```

For manual browsing, mount the packaged site and generated configuration into a disposable NGINX container. The sample URLs use the actual DocFX member page as well as a shared type pathname.

```powershell
$mounts = @('--mount', "type=bind,source=$work/site,target=/usr/share/nginx/html,readonly",
    '--mount', "type=bind,source=$work/nginx.conf,target=/etc/nginx/conf.d/default.conf,readonly")
docker run --rm -p 127.0.0.1:8080:8080 @mounts dhi.io/nginx:1.31-alpine
```

- [Default StreamFactory](http://localhost:8080/api/dotnet/Cuemon.IO.StreamFactory.html)
- [Create overloads on .NET 10](http://localhost:8080/api/dotnet/Cuemon.IO.StreamFactory.Create.html?view=net-10.0)
- [Create overloads on .NET Standard 2.0](http://localhost:8080/api/dotnet/Cuemon.IO.StreamFactory.Create.html?view=netstandard-2.0)
- [BufferWriterOptions on .NET Standard 2.1](http://localhost:8080/api/dotnet/Cuemon.IO.BufferWriterOptions.html?view=netstandard-2.1)

## Framework discovery and metadata

| MSBuild TFM | Public view | Staging directory | Metadata UIDs | Buffer writer overloads |
|---|---|---|---:|---:|
| net11.0 | net-11.0 | net11 | 557 | 6 |
| net10.0 | net-10.0 | net10 | 557 | 6 |
| netstandard2.1 | netstandard-2.1 | netstandard2.1 | 76 | 6 |
| netstandard2.0 | netstandard-2.0 | netstandard2.0 | 548 | 0 |

The shared builder checks the configured framework order against MSBuild's `TargetFrameworks` and `TargetFramework` properties. The fixture narrows the authored `api/dotnet` group to two projects and keeps its destination, filter, separate member-page layout, and namespace and type overwrites. Each metadata extraction and HTML build uses a fresh DocFX process. The fixture includes the authored home page and a two-entry navigation and omits the production sitemap and xref input to keep availability local to the build. DocFX still emits its native Microsoft Learn links.

Cuemon.IO directly targets .NET Standard 2.1. Cuemon.Kernel has no such target, so it contributes no pages in that view. The ordinary .NET Standard 2.1 IO build resolves compatible .NET Standard 2.0 dependencies through MSBuild. DocFX's project workspace can retain both the design-time Cuemon.Kernel symbol (observed at version 1.0.0.0) and an on-disk dependency (observed at 11.0.0.0), producing CS0433. A normal Release build followed by MSBuild's GetAssemblyVersion gives the PoC the version to pass to DocFX for Cuemon's mixed graph. This retained project-source links, native declarations and all 76 IO UIDs on Windows and Linux. The original Roslyn project-reference diagnostics remain in the logs. Compilation errors fail the build. The workaround relies on Cuemon's common assembly version and needs additional validation for independently versioned project graphs. [DocFX project compilation](https://github.com/dotnet/docfx/blob/ba844df625370a6bd7c1ecab0d0ec4d1d9128ac4/src/Docfx.Dotnet/DotnetApiCatalog.Compile.cs)

## Routing, links and search

The default site sits at the web root. Each additional view contributes its native API subtree, xref map and search index under `_frameworks/<directory>`. The package verifies that shared public assets, images and run-action scripts are byte-identical before sharing them. It publishes conceptual HTML and the root navigation once.

NGINX maps exact view IDs to a static root prefix and checks the selected file at the original URI depth. The default applies when view is missing or empty. Unknown IDs receive HTTP 400. A known view with an unavailable page receives HTTP 404. Physical API copies are internal. The visible pathname keeps relative CSS, JavaScript, image, run-action and page links at the depth DocFX generated. Stable responses use the existing no-store policy and fingerprinted public assets keep their immutable policy. [NGINX try_files](https://nginx.org/en/docs/http/ngx_http_core_module.html#try_files)

The shared template loads `framework-view.js` before the native DocFX module on configured sites. It sets the API TOC metadata to the selected TOC JSON URL and carries the view through same-origin API links, including links rendered later by DocFX. External and conceptual navigation use their regular URLs. Direct requests select the correct HTML before JavaScript runs.

DocFX 2.81.0 creates a module worker from `docfx:rel`. The template sets that metadata to `/_framework-search/<directory>/` for explicitly selected API views. NGINX routes the worker and its imports to shared assets and serves the matching native index at the worker's relative path. Search result links and their secondary labels show the public URLs. Unqualified API and conceptual pages use the ordinary DocFX search entry point. Rerun the browser regression when upgrading DocFX because the adapter uses its search metadata, worker URL, and result template. [Modern search code](https://github.com/dotnet/docfx/blob/ba844df625370a6bd7c1ecab0d0ec4d1d9128ac4/templates/modern/src/search.ts) · [Search worker](https://github.com/dotnet/docfx/blob/ba844df625370a6bd7c1ecab0d0ec4d1d9128ac4/templates/modern/src/search-worker.ts)

## Selector contract

The versioned `/framework-views.json` manifest exposes `apiPath`, `defaultView`, `viewOrder`, and `views`. Each view records its TFM, directory, projects, and native xref map URL. The selector resolves the exact UID in the requested view or the nearest newer view in `viewOrder`. When no newer view documents the UID, it prefers the currently displayed view if available, then checks older views. It keeps the fragment and other query parameters and shows a short fallback notice.

## Verification and cost

The acceptance scripts use the actual HTTP responses for xref inventory, all pages and anchors, unavailable APIs, native overload and inheritance differences, TOCs, search indexes, source links, overwrite examples, MIME types and caching. The browser test exercises generated sidebar links, in-page cross-references, native search results, refresh, and back/forward between API pages. DocFX's raw TOC HTML is a legacy partial. The interactive sidebar uses the view-specific JSON.

The original PoC measured 53.29 MiB of output. The shared build preserves the same layout and serves one native search index per page. The browser regression also exercises same-UID switching, fallback to an available view, direct 404 responses, an older-only API, and mobile keyboard focus in dark mode.

The four-view run performs four metadata extractions and four builds, with one Release project build for the mixed graph. The shared metadata cache can reuse verified extraction output. Each API request uses a static NGINX map and file check.

| Approach | Generation and delivery | Tradeoff |
|---|---|---|
| Native query views | One extraction and one HTML build per view, one shared static site and one small browser adapter | Keeps existing default URLs and a stable shared pathname. Requires query-aware hosting and navigation and search integration. |
| Native TFM-prefixed URLs | Same independent builds, served at physical per-view roots | Relative URLs, native TOC and search follow the path automatically. Keep existing default URLs at the root and expose alternate views under prefixes. Sharing identical assets can remain a packaging optimization. |
| Historical consolidation | Three extractions and one render plus a 240-line C# consolidator | Uses less output space, but holds a primary declaration and explicit relationship deltas. Unexpected same-UID signatures fail the consolidation. |

---

# Historical PoC 1 — 561-UID consolidation

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

The proposed production distribution is a .NET tool published by the shared implementation owner. Cuemon and the composite Action should invoke the same explicit tool version. The tool should read project mappings, metadata destinations, and site structure from the consumer's `docfx.json` and use an explicitly pinned published builder image containing the shared template. Include the tool version and selected framework matrix in the local metadata cache fingerprint.

Keep production opt-in pending an image build that retains run-action preparation, context and rendered-site verification, verified metadata reuse, and both Linux image platforms. Verify full-site API paths and cross-group relationships before enabling it. The current single-process metadata pass retains resolver state across all groups.
