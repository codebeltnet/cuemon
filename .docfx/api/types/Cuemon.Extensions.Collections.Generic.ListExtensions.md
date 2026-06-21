---
uid: Cuemon.Extensions.Collections.Generic.ListExtensions
example:
- *content
---

`ListExtensions` provides extension methods for `List<T>` for safe navigation and manipulation including predicate-based removal, bounds checking, adjacent-element access, and conditional addition. This example creates a list of fruit names `["Apple", "Banana", "Cherry", "Date"]`, calls `Remove` with a predicate to delete `"Banana"`, checks whether index `5` exists with `HasIndex`, and retrieves adjacent elements using `Next(0)` and `Previous(2)` without throwing on out-of-bounds access. `TryAdd` conditionally adds `"Cherry"` (duplicate, returns `false`) and `"Elderberry"` (new, returns `true`). Console output confirms the removal, bounds-check results, neighbor values, and the success of each conditional addition.

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
