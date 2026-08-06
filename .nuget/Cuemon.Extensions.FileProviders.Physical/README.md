# Cuemon.Extensions.FileProviders.Physical for .NET

## About

`Cuemon.Extensions.FileProviders.Physical` adds `PortablePhysicalFileProvider`, a physical file-system-backed `IFileProvider` for applications that need case-insensitive path lookups across operating systems.

It decorates `PhysicalFileProvider`, resolves file and directory segments using ordinal, case-insensitive matching, and preserves the physical casing reported by the file system when returning file info, directory contents, and change tokens.

Successful lookups are cached for the lifetime of the provider by using case-insensitive logical keys that normalize supported directory separators together with redundant leading and repeated separators. Equivalent requests such as `assets/logo.svg`, `/assets/logo.svg`, and `assets//logo.svg` therefore share the same successful cache entry.

## Supported Frameworks

This package targets `.NET 10`, `.NET 9`, and `.NET Standard 2.0`.

## Why Pick This Package

- Built on `Microsoft.Extensions.FileProviders.Physical` and exposed through the familiar `IFileProvider` abstraction
- Resolves file and directory paths case-insensitively, including nested segments
- Treats ambiguous case collisions as not found instead of selecting an arbitrary file or directory
- Preserves `PhysicalFileProvider` behavior for exclusion filters, polling options, and wildcard watch filters

## Installation

```bash
dotnet add package Cuemon.Extensions.FileProviders.Physical
```

## Quick Start

```csharp
using Cuemon.Extensions.FileProviders;

var contentRoot = @"C:\app\wwwroot";
using var files = new PortablePhysicalFileProvider(contentRoot);

var logo = files.GetFileInfo("assets/images/logo.svg");
if (logo.Exists)
{
    Console.WriteLine(logo.Name);
    Console.WriteLine(logo.PhysicalPath);
}

var images = files.GetDirectoryContents("ASSETS/IMAGES");
var changeToken = files.Watch("assets/images/logo.svg");
```

If the physical file system contains `Assets/Images/Logo.svg`, the lookup above still resolves it. If the same logical path maps to multiple physical entries that differ only by casing, such as `logo.svg` and `Logo.svg`, the provider returns not-found results and a null change token for literal watch filters instead of choosing one entry.

## Performance Model

- Successful resolved paths are cached, so subsequent successful lookups avoid repeated path traversal.
- Uncached resolution enumerates each directory needed to resolve the requested path.
- A segment must be fully inspected to prove uniqueness or detect a case-insensitive collision, so cold lookup cost grows with directory width.
- A cold path lookup is therefore approximately proportional to the sum of the sibling counts in the traversed directories.
- Misses and collisions are deliberately not cached and are re-evaluated on every call.
- Repeated unresolved requests in wide directories can be substantially more expensive than successful cache hits.
- If arbitrary user-controlled paths reach this provider, consider upstream validation, response caching, rate limiting, or similar controls.
- Previously successful logical paths retain the provider's stable-topology cache behavior for the lifetime of the provider instance.

## Documentation

API documentation for Cuemon packages is published at [docs.cuemon.net](https://docs.cuemon.net/).

## Contributing

Contributions and issue reports are welcome in the [codebeltnet/cuemon](https://github.com/codebeltnet/cuemon) repository.

## License

Licensed under the [MIT License](https://github.com/codebeltnet/cuemon/blob/main/LICENSE.md).
