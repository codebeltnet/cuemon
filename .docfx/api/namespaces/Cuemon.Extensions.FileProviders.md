---
uid: Cuemon.Extensions.FileProviders
summary: *content
---
Resolve physical files and directories through the familiar `IFileProvider` abstraction while treating path segments case-insensitively across operating systems. Use this namespace when callers may supply different casing for the same content path and ambiguous case-only matches must be reported as not found instead of selecting an arbitrary entry. Start with `PortablePhysicalFileProvider` to inspect files, enumerate directories, or watch literal paths while retaining the underlying `PhysicalFileProvider` behavior for filters and polling.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Complements: [Microsoft.Extensions.FileProviders.Physical namespace](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.fileproviders.physical) 🔗
