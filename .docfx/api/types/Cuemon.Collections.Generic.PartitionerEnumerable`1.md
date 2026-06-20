---
uid: Cuemon.Collections.Generic.PartitionerEnumerable`1
example:
- *content
---

The following example demonstrates how to use <xref cref="Cuemon.Collections.Generic.PartitionerEnumerable{T}"/> to iterate over a sequence in fixed-size partitions. It wraps a sequence of 1000 integers with a partition size of 100, then processes partitions sequentially using `ToList` while tracking the remaining partitions via `HasPartitions`. A smaller string partitioner shows the same behavior with an uneven final batch. The example outputs partition counts and content to the console, verifying that batching works correctly.

```csharp
using System;
using System.Linq;
using Cuemon.Collections.Generic;

namespace MyApp.Examples;

public class PartitionerEnumerableExample
{
    public void Demonstrate()
    {
        // Create a sequence of 1000 numbers
        var numbers = Enumerable.Range(1, 1000);

        // Wrap in a partitioner with partition size of 100
        var partitioner = new PartitionerEnumerable<int>(numbers, partitionSize: 100);

        Console.WriteLine(partitioner.PartitionSize); // 100
        Console.WriteLine(partitioner.HasPartitions); // True
        Console.WriteLine(partitioner.IteratedCount); // 0

        // Process the first partition (items 1-100)
        var firstBatch = partitioner.ToList();
        Console.WriteLine(firstBatch.Count);  // 100
        Console.WriteLine(firstBatch[0]);     // 1
        Console.WriteLine(firstBatch[99]);    // 100

        Console.WriteLine(partitioner.IteratedCount); // 1
        Console.WriteLine(partitioner.HasPartitions); // True

        // Process the remaining partitions
        while (partitioner.HasPartitions)
        {
            var batch = partitioner.ToList();
            Console.WriteLine($"Batch {partitioner.IteratedCount}: {batch.Count} items");
        // After exhausting the sequence:
        Console.WriteLine(partitioner.HasPartitions); // False
        Console.WriteLine(partitioner.IteratedCount); // 10 (1000/100)

        // A partitioner can be iterated multiple times (each iteration
        // advances through the source sequence)
        var words = new PartitionerEnumerable<string>(
            new[] { "a", "b", "c", "d", "e", "f", "g", "h" },
            partitionSize: 3);

        while (words.HasPartitions)
        {
            var chunk = words.ToList();
            Console.WriteLine(string.Join(",", chunk));
        // Output:
        //   a,b,c
        //   d,e,f
        //   g,h

}}}
}

```
