---
uid: Cuemon.ConvertibleConverterDictionary
example:
- *content
---

The following example demonstrates how to use <xref cref="Cuemon.ConvertibleConverterDictionary"/> to register and use type-specific converters that transform <see cref="IConvertible"/> values into byte arrays.

```csharp
using System;
using System.Linq;
using System.Text;
using Cuemon;

namespace MyApp.Examples;

public class ConvertibleConverterDictionaryExample
{
    public void Demonstrate()
    {
        // Create a dictionary of converters for different IConvertible types
        var converters = new ConvertibleConverterDictionary()
            .Add<int>(value => BitConverter.GetBytes(value))
            .Add<long>(value => BitConverter.GetBytes(value))
            .Add<string>(value => Encoding.UTF8.GetBytes(value))
            .Add<bool>(value => BitConverter.GetBytes(value));

        Console.WriteLine(converters.Count); // 4

        // Check if a converter exists for a specific type
        Console.WriteLine(converters.ContainsKey(typeof(int)));   // True
        Console.WriteLine(converters.ContainsKey(typeof(float))); // False

        // Use a registered converter
        if (converters.TryGetValue(typeof(string), out var stringConverter))
        {
            byte[] bytes = stringConverter("Hello");
            Console.WriteLine(bytes.Length); // 5
            Console.WriteLine(Encoding.UTF8.GetString(bytes)); // Hello

        // Use the indexer to get a converter
        var intConverter = converters[typeof(int)];
        if (intConverter != null)
        {
            byte[] intBytes = intConverter(42);
            Console.WriteLine(BitConverter.ToInt32(intBytes)); // 42

        // Iterate all registered converters
        foreach (var kvp in converters)
        {
            Console.WriteLine($"{kvp.Key.Name} -> converter registered");
        // Output:
        //   Int32 -> converter registered
        //   Int64 -> converter registered
        //   String -> converter registered
        //   Boolean -> converter registered

        // Add a converter using the non-generic Add method
        converters.Add(typeof(double), value => BitConverter.GetBytes(value.ToDouble(null)));
        Console.WriteLine(converters.Count); // 5
        Console.WriteLine(converters.ContainsKey(typeof(double))); // True

}}}}
}

```
