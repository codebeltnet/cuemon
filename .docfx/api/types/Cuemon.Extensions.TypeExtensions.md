---
uid: Cuemon.Extensions.TypeExtensions
example:
- *content
---

`TypeExtensions` provides extension methods for `Type` that simplify classification and metadata retrieval via methods like `ToFriendlyName`, `ToTypeCode`, and various `Has*`/`Is*` predicates. This example inspects `IList<string>`, `ConcurrentDictionary<string, int>`, `StringComparer`, `int?`, and anonymous types, calling `HasEnumerableImplementation`, `HasDictionaryImplementation`, `IsNullable`, `IsComplex`, `GetDefaultValue`, and `ToFriendlyName`. It also demonstrates `HasAnonymousCharacteristics` on an anonymous object, `HasTypes` for hierarchy checks, `HasInterfaces` for generic interface matching, and `HasAttributes` for attribute presence. Console output prints friendly type names like `IList<String>`, boolean capability flags, and the default value `0` for `typeof(int)`.

```csharp
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using Cuemon.Extensions;

namespace MyApp.Examples;

public static class TypeExtensionsExample
{
    public static void Demonstrate()
    {
        Type listType = typeof(IList<string>);
        Type dictionaryType = typeof(ConcurrentDictionary<string, int>);
        Type comparerType = typeof(StringComparer);
        Type nullableInt = typeof(int?);

        Console.WriteLine(listType.ToFriendlyName());
        Console.WriteLine(listType.ToFriendlyName(options => options.FullName = true));
        Console.WriteLine(typeof(string).ToTypeCode());
        Console.WriteLine(listType.HasEnumerableImplementation());
        Console.WriteLine(typeof(string).HasComparableImplementation());
        Console.WriteLine(dictionaryType.HasDictionaryImplementation());
        Console.WriteLine(comparerType.HasEqualityComparerImplementation());
        Console.WriteLine(comparerType.HasComparerImplementation());
        Console.WriteLine(typeof(KeyValuePair<string, int>).HasKeyValuePairImplementation());
        Console.WriteLine(nullableInt.IsNullable());
        Console.WriteLine(typeof(Stream).IsComplex());
        Console.WriteLine(typeof(int).IsSimple());
        Console.WriteLine(typeof(int).GetDefaultValue());
        Console.WriteLine(typeof(FileStream).HasTypes(typeof(Stream)));
        Console.WriteLine(typeof(List<>).HasInterfaces(typeof(IEnumerable<>)));
        Console.WriteLine(typeof(string).HasAttributes(typeof(SerializableAttribute)));
        Console.WriteLine(new { Name = "sample", Value = 42 }.GetType().HasAnonymousCharacteristics());
    }
}

```
