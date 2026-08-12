---
uid: Cuemon.Extensions.FileProviders.PortablePhysicalFileProvider
example:
- *content
---

The following example uses <xref cref="Cuemon.Extensions.FileProviders.PortablePhysicalFileProvider"/> to resolve physical files and directories without requiring callers to match the casing stored on disk. Point `contentRoot` at the application's content directory, then use the familiar `IFileProvider` methods for file metadata, directory enumeration, and change notifications. If the physical file is `Assets/Images/Logo.svg`, the `assets/images/logo.svg` lookup resolves the same file; a case-only collision is reported as not found.

Successful logical paths are cached for the lifetime of the provider by using case-insensitive keys that normalize supported directory separators plus redundant leading and repeated separators. Uncached resolution still enumerates each traversed directory and must inspect a full sibling set to prove uniqueness or detect a collision, so repeated unresolved requests in wide directories remain materially more expensive than warm successful cache hits.

Misses and collisions are deliberately re-evaluated on every call. If callers can supply arbitrary paths, add the surrounding controls that fit the application boundary, such as request validation, response caching, or rate limiting.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

```csharp
using System;
using Cuemon.Extensions.FileProviders;

namespace MyApp.Examples;

public static class PortablePhysicalFileProviderExample
{
    public static void Demonstrate()
    {
        var contentRoot = @"C:\app\wwwroot";
        using var files = new PortablePhysicalFileProvider(contentRoot);

        var logo = files.GetFileInfo("assets/images/logo.svg");
        if (logo.Exists)
        {
            Console.WriteLine($"Resolved file: {logo.Name}");
            Console.WriteLine($"Physical path: {logo.PhysicalPath}");
        }

        var images = files.GetDirectoryContents("ASSETS/IMAGES");
        Console.WriteLine($"Image directory found: {images.Exists}");

        var changeToken = files.Watch("assets/images/logo.svg");
        Console.WriteLine($"Change callbacks active: {changeToken.ActiveChangeCallbacks}");
    }
}
```
