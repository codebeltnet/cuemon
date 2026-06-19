---
uid: Cuemon.Extensions.Collections.Generic.ListExtensions
example:
- *content
---

The following example demonstrates how to use the <xref:Cuemon.Extensions.Collections.Generic.ListExtensions> extension methods to safely navigate and manipulate lists.

```csharp
using System;
using System.Collections.Generic;
using Cuemon.Extensions.Collections.Generic;

namespace DocExamples
{
    public static class ListExtensionsExample
    {
        public static void Main()
        {
            var fruits = new List<string> { "Apple", "Banana", "Cherry", "Date" };

            // Remove the first element matching a condition
            bool removed = fruits.Remove(f => f == "Banana");
            Console.WriteLine($"Removed 'Banana': {removed}");
            Console.WriteLine($"Fruits after removal: {string.Join(", ", fruits)}");

            // Check if an index exists in the list
            bool hasIndex = fruits.HasIndex(5);
            Console.WriteLine($"Has index 5: {hasIndex}");
            Console.WriteLine($"Has index 1: {fruits.HasIndex(1)}");

            // Get the next element relative to an index
            string next = fruits.Next(0);
            Console.WriteLine($"Element after index 0: {next}");

            // Get the previous element relative to an index
            string prev = fruits.Previous(2);
            Console.WriteLine($"Element before index 2: {prev}");

            // Returns default when out of bounds
            string beyond = fruits.Next(10);
            Console.WriteLine($"Element after index 10: {(beyond == null ? "null (default)" : beyond)}");

            // Try to add an element (only if not already present)
            bool added = fruits.TryAdd("Cherry");
            Console.WriteLine($"Added 'Cherry' (duplicate): {added}");

            bool addedNew = fruits.TryAdd("Elderberry");
            Console.WriteLine($"Added 'Elderberry' (new): {addedNew}");
            Console.WriteLine($"Fruits: {string.Join(", ", fruits)}");

}}
}

```
