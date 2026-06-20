---
uid: Cuemon.Extensions.Reflection.TypeExtensions
example:
- *content
---

`TypeExtensions` in the `Reflection` namespace provides methods for inspecting type hierarchies and enumerating members across the inheritance tree. This example uses `typeof(Stream)` to retrieve derived, inherited, and hierarchy types via `GetDerivedTypes`, `GetInheritedTypes`, and `GetHierarchyTypes`, then uses `TypeCatalog` (extending `BaseCatalog`) with `GetAllProperties`, `GetAllEvents`, `GetAllFields`, and `GetAllMethods` to enumerate members. It also demonstrates `GetRuntimePropertiesExceptOf<BaseCatalog>` to exclude inherited properties, `GetEmbeddedResources` for assembly resource lookup, and `ToFullNameIncludingAssemblyName` for a fully qualified type name. Console output confirms member names, hierarchy inclusion (e.g., `Stream` in `GetHierarchyTypes`), and assembly-qualified type identity.

```csharp
using System;
using System.IO;
using System.Linq;
using Cuemon;
using Cuemon.Extensions.Reflection;
using Cuemon.Reflection;

namespace MyApp.Examples;

public static class TypeExtensionsExample
{
    public static void Demonstrate()
    {
        var streamType = typeof(Stream);
        var derivedTypes = streamType.GetDerivedTypes().Where(type => type.IsPublic).Select(type => type.Name).Take(3).ToArray();
        var inheritedTypes = streamType.GetInheritedTypes().Select(type => type.Name).ToArray();
        var properties = typeof(TypeArgumentOutOfRangeException).GetAllProperties().Select(property => property.Name).ToArray();
        var events = typeof(TypeCatalog).GetAllEvents().Select(@event => @event.Name).ToArray();
        var fields = typeof(TypeCatalog).GetAllFields().Select(field => field.Name).ToArray();
        var methods = typeof(TypeCatalog).GetAllMethods().Select(method => method.Name).ToArray();
        var hierarchy = typeof(Stream).GetHierarchyTypes().Select(type => type.Name).ToArray();
        var resources = typeof(TypeExtensionsExample).GetEmbeddedResources("missing", ManifestResourceMatch.ContainsName);
        var ownProperties = typeof(TypeCatalog).GetRuntimePropertiesExceptOf<BaseCatalog>().Select(property => property.Name).ToArray();
        var fullName = typeof(TypeCatalog).ToFullNameIncludingAssemblyName();

        Console.WriteLine(string.Join(", ", derivedTypes));
        Console.WriteLine(string.Join(", ", inheritedTypes));
        Console.WriteLine(properties.Contains(nameof(TypeArgumentOutOfRangeException.ActualValue)));
        Console.WriteLine(events.Contains(nameof(TypeCatalog.Changed)));
        Console.WriteLine(fields.Contains(nameof(TypeCatalog._state)));
        Console.WriteLine(methods.Contains(nameof(TypeCatalog.MarkChanged)));
        Console.WriteLine(hierarchy.Contains(nameof(Stream)));
        Console.WriteLine(resources.Count);
        Console.WriteLine(ownProperties.Contains(nameof(TypeCatalog.Name)));
        Console.WriteLine(fullName.Contains(nameof(TypeCatalog)));
    }

    private abstract class BaseCatalog
    {
        public int Id { get; set; }
    }

    private sealed class TypeCatalog : BaseCatalog
    {
        internal string _state = "draft";

        public string Name { get; set; } = "Extensions";

        public event EventHandler Changed;

        public void MarkChanged()
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }
    }
}
```
