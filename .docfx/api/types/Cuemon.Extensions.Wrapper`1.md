---
uid: Cuemon.Extensions.Wrapper`1
example:
- *content
---

The following example demonstrates how to use <xref:Cuemon.Extensions.Wrapper`1> to wrap an object with optional member reference metadata.

```csharp
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Cuemon.Extensions;

namespace MyApp.Examples;

public static class WrapperExample
{
    private sealed class WrapperExampleModel
    {
        public string Name { get; set; } = string.Empty;
    }

    public static void Demonstrate()
    {
        PropertyInfo nameProperty = typeof(WrapperExampleModel).GetProperty(nameof(WrapperExampleModel.Name), BindingFlags.Public | BindingFlags.Instance);
        var answer = new Wrapper<int>(42, nameProperty);
        answer.Data["category"] = "number";

        var fromText = new Wrapper<string>("42");
        var bytes = new Wrapper<byte[]>(new byte[] { 1, 2, 3, 4 });
        var type = new Wrapper<Type>(typeof(Dictionary<string, int>));

        Console.WriteLine(answer.Instance);
        Console.WriteLine(answer.MemberReference?.Name);
        Console.WriteLine(answer.InstanceAs<string>(CultureInfo.InvariantCulture));
        Console.WriteLine(fromText.InstanceAs<int>());
        Console.WriteLine(answer.Data["category"]);
        Console.WriteLine(Wrapper.ParseInstance(bytes));
        Console.WriteLine(type.ToString());
    }
}

```
