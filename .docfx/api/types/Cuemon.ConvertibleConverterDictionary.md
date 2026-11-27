---
uid: Cuemon.ConvertibleConverterDictionary
example:
- *content
---

The following example demonstrates how to use <xref cref="Cuemon.ConvertibleConverterDictionary"/> to register and use type-specific converters that transform <see cref="IConvertible"/> values into byte arrays.

```csharp
// Program.cs
using System;
using System.Text;
using Cuemon;

var converters = new ConvertibleConverterDictionary()
    .Add<int>(value => BitConverter.GetBytes(value))
    .Add<long>(value => BitConverter.GetBytes(value))
    .Add<string>(value => Encoding.UTF8.GetBytes(value))
    .Add<bool>(value => BitConverter.GetBytes(value));

Console.WriteLine(converters.Count);

Console.WriteLine(converters.ContainsKey(typeof(int)));
Console.WriteLine(converters.ContainsKey(typeof(float)));

if (converters.TryGetValue(typeof(string), out var stringConverter))
{
    byte[] bytes = stringConverter("Hello");
    Console.WriteLine(bytes.Length);
    Console.WriteLine(Encoding.UTF8.GetString(bytes));
}

var intConverter = converters[typeof(int)];
if (intConverter != null)
{
    byte[] intBytes = intConverter(42);
    Console.WriteLine(BitConverter.ToInt32(intBytes));
}

foreach (var kvp in converters)
{
    Console.WriteLine($"{kvp.Key.Name} -> converter registered");
}

converters.Add(typeof(double), value => BitConverter.GetBytes(value.ToDouble(null)));
Console.WriteLine(converters.Count);
Console.WriteLine(converters.ContainsKey(typeof(double)));
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="08d377f47415311de1f46f2c"><a href="~/run-actions/08d377f47415311de1f46f2c/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/08d377f47415311de1f46f2c/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
