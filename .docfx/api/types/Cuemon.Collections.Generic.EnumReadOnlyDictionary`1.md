---
uid: Cuemon.Collections.Generic.EnumReadOnlyDictionary`1
example:
- *content
---

The following example demonstrates how to use <xref cref="Cuemon.Collections.Generic.EnumReadOnlyDictionary{TEnum}"/> to create a read-only dictionary that maps each enum value (as its underlying integral type wrapped in <see cref="IConvertible"/>) to its string name.

```csharp
using System.Collections.Generic;
using System;
using System.Linq;
using Cuemon.Collections.Generic;

namespace MyApp.Examples;

public class EnumReadOnlyDictionaryExample
{
    public void Demonstrate()
    {
        // Create a dictionary that maps DayOfWeek values to their string names
        var days = new EnumReadOnlyDictionary<DayOfWeek>();

        Console.WriteLine(days.Count); // 7

        // Iterate through all entries (keys are the underlying integral values)
        foreach (var kvp in days.OrderBy(kvp => kvp.Key.ToInt32(null)))
        {
            Console.WriteLine($"{kvp.Key.ToInt32(null)} -> {kvp.Value}");
        // Output:
        //   0 -> Sunday
        //   1 -> Monday
        //   2 -> Tuesday
        //   3 -> Wednesday
        //   4 -> Thursday
        //   5 -> Friday
        //   6 -> Saturday

        // Access the values collection directly
        foreach (var name in days.Values)
        {
            Console.WriteLine(name); // Sunday, Monday, ..., Saturday

        // Access the keys collection
        foreach (var key in days.Keys)
        {
            Console.WriteLine(key.ToInt32(null)); // 0, 1, 2, ..., 6

}}}}
}

```
