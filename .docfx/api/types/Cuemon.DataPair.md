---
uid: Cuemon.DataPair
example:
- *content
---

The following example demonstrates how to use the <see cref="DataPair"/> and <see cref="DataPair{T}"/> classes to represent named metadata with type information.

```csharp
using System;
using Cuemon; // for DataPair, DataPair<T>

namespace MyApp.Examples;

public class DataPairExample
{
    public void Demonstrate()
    {
        // Create a generic DataPair<T> for compile-time type safety
        var pair = new DataPair<int>("Age", 30);
        Console.WriteLine(pair);
        // Output: Name: Age, Value: 30, Type: Int32

        Console.WriteLine(pair.Name);   // Age
        Console.WriteLine(pair.Value);  // 30
        Console.WriteLine(pair.Type);   // System.Int32
        Console.WriteLine(pair.HasValue); // True

        // Create a non-generic DataPair
        var generic = new DataPair("CreatedAt", DateTime.UtcNow, typeof(DateTime));
        Console.WriteLine(generic);
        // Output: Name: CreatedAt, Value: ..., Type: DateTime

        // DataPair with null value
        var nullPair = new DataPair<string>("MiddleName", null);
        Console.WriteLine(nullPair.HasValue); // False
        Console.WriteLine(nullPair);
        // Output: Name: MiddleName, Value: <null>, Type: String

}
}

```
