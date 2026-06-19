---
uid: Cuemon.Extensions.ObjectExtensions
example:
- *content
---

The following example demonstrates how to use the <xref:Cuemon.Extensions.ObjectExtensions> to wrap, convert, and adjust objects.

```csharp
using System;
using System.Collections.Generic;
using Cuemon.Extensions;

namespace MyApp.Examples;

public static class ObjectExtensionsExample
{
    public static void Demonstrate()
    {
        object raw = "42";
        int number = raw.As(0);
        int fallback = "not-a-number".As(99);
        object nullValue = null;
        int safe = nullValue.As(0);
        string upper = "hello".As(value => value.ToUpperInvariant());
        object converted = raw.As(typeof(int));

        var wrapped = "docs".UseWrapper(extender =>
        {
            extender["source"] = "example";
        });

        var memberWrapped = 42.UseWrapper(typeof(int).GetMethod("ToString"), data =>
        {
            data["category"] = "number";
        });

        var numbers = new List<int> { 1, 2, 3 };
        var adjusted = numbers.Adjust(list =>
        {
            var copy = new List<int>(list);
            copy.Add(4);
            return copy;
        });
        var altered = numbers.Alter(list => list.Add(4));
        string delimited = adjusted.ToDelimitedString(options => options.Delimiter = ";");

        Console.WriteLine(number);
        Console.WriteLine(fallback);
        Console.WriteLine(safe);
        Console.WriteLine(upper);
        Console.WriteLine(converted);
        Console.WriteLine(wrapped.Data["source"]);
        Console.WriteLine(memberWrapped.MemberReference?.Name);
        Console.WriteLine(adjusted.GetHashCode32());
        Console.WriteLine(altered.GetHashCode64());
        Console.WriteLine(delimited);
        Console.WriteLine(default(int?).IsNullable<int?>());
    }
}

```
