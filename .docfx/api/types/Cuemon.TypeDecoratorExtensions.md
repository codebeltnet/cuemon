---
uid: Cuemon.TypeDecoratorExtensions
example:
- *content
---

The following example demonstrates how to use the <xref:Cuemon.TypeDecoratorExtensions> to generate a reflection report before loading a plugin type.

```csharp
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Cuemon;

namespace Contoso.Reflection;

public sealed class TypeDecoratorExtensionsExample
{
    public static void Run()
    {
        var orderType = Decorator.Enclose(typeof(AuditedOrder));
        var streamType = Decorator.Enclose(typeof(Stream));

        var propertyNames = orderType.GetAllProperties().Select(property => property.Name).OrderBy(name => name).ToArray();
        var fieldNames = orderType.GetAllFields().Select(field => field.Name).OrderBy(name => name).ToArray();
        var eventNames = orderType.GetAllEvents().Select(@event => @event.Name).OrderBy(name => name).ToArray();
        var methodNames = orderType
            .GetAllMethods()
            .Where(method => method.DeclaringType == typeof(AuditedOrder) && !method.IsSpecialName)
            .Select(method => method.Name)
            .Distinct()
            .OrderBy(name => name)
            .ToArray();
        var orderOnlyProperties = orderType.GetRuntimePropertiesExceptOf<TrackedEntity>().Select(property => property.Name).OrderBy(name => name).ToArray();

        bool hasTypes = Decorator.Enclose(typeof(FileStream)).HasTypes(typeof(Stream));
        bool hasInterfaces = Decorator.Enclose(typeof(List<>)).HasInterfaces(typeof(IEnumerable<>));
        bool hasAttribute = orderType.HasAttribute(typeof(DataContractAttribute), typeof(DataMemberAttribute));
        bool hasComparable = Decorator.Enclose(typeof(string)).HasComparableImplementation();
        bool hasComparer = Decorator.Enclose(typeof(StringComparer)).HasComparerImplementation();
        bool hasDictionary = Decorator.Enclose(typeof(ReadOnlyDictionary<int, string>)).HasDictionaryImplementation();
        bool hasEqualityComparer = Decorator.Enclose(typeof(StringComparer)).HasEqualityComparerImplementation();
        bool hasEnumerable = Decorator.Enclose(typeof(ConcurrentBag<int>)).HasEnumerableImplementation();
        bool hasKeyValuePair = Decorator.Enclose(typeof(KeyValuePair<string, int>)).HasKeyValuePairImplementation();
        bool isNullable = Decorator.Enclose(typeof(int?)).IsNullable();
        bool hasAnonymousCharacteristics = Decorator.Enclose(new { ReferenceNumber = "PO-42" }.GetType()).HasAnonymousCharacteristics();
        bool hasDefaultCtor = Decorator.Enclose(typeof(MemoryStream)).HasDefaultConstructor();
        bool isComplex = streamType.IsComplex();

        object defaultValue = Decorator.Enclose(typeof(Guid)).GetDefaultValue();
        string friendlyName = Decorator.Enclose(typeof(IList<string>)).ToFriendlyName();
        MethodBase publishMethod = orderType.MatchMember(nameof(AuditedOrder.Publish));

        bool inheritedIncludesObject = streamType.GetInheritedTypes().Contains(typeof(object));
        bool derivedIncludesMemoryStream = streamType.GetDerivedTypes().Contains(typeof(MemoryStream));
        bool hierarchyIncludesFileStream = streamType.GetHierarchyTypes().Contains(typeof(FileStream));

        var loop = LinkedNode.CreateLoop();
        bool hasCircularReference = Decorator.Enclose(typeof(LinkedNode)).HasCircularReference(loop, maxDepth: 1);

        Console.WriteLine(string.Join(", ", propertyNames));
        Console.WriteLine(string.Join(", ", fieldNames));
        Console.WriteLine(string.Join(", ", eventNames));
        Console.WriteLine(string.Join(", ", methodNames));
        Console.WriteLine(string.Join(", ", orderOnlyProperties));
        Console.WriteLine($"{hasTypes}, {hasInterfaces}, {hasAttribute}");
        Console.WriteLine($"{hasComparable}, {hasComparer}, {hasDictionary}, {hasEqualityComparer}, {hasEnumerable}, {hasKeyValuePair}");
        Console.WriteLine($"{isNullable}, {hasAnonymousCharacteristics}, {hasDefaultCtor}, {isComplex}");
        Console.WriteLine(defaultValue);
        Console.WriteLine(friendlyName);
        Console.WriteLine(publishMethod.Name);
        Console.WriteLine($"{inheritedIncludesObject}, {derivedIncludesMemoryStream}, {hierarchyIncludesFileStream}, {hasCircularReference}");
    }
}

[DataContract]
public sealed class AuditedOrder : TrackedEntity
{
    public string PublicNote;

    [DataMember]
    public string ReferenceNumber { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public event EventHandler Published;

    public void Publish()
    {
        Published?.Invoke(this, EventArgs.Empty);
    }
}

public abstract class TrackedEntity
{
    public DateTime CreatedAt { get; set; }
}

public sealed class LinkedNode
{
    public LinkedNode Next { get; set; }

    public static LinkedNode CreateLoop()
    {
        var node = new LinkedNode();
        node.Next = node;
        return node;
    }
}
```
