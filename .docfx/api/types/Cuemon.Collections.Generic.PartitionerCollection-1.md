---
uid: Cuemon.Collections.Generic.PartitionerCollection`1
example:
- *content
---

The following example shows how to partition a list of numbers into fixed-size groups using `PartitionerCollection`. It iterates through each partition and prints partition-level and collection-level metadata.

```csharp
using System;
using System.Collections.Generic;
using Cuemon.Collections.Generic;

namespace MyApp.Examples
{
    public class PartitionerCollectionExample
    {
        public static void Demonstrate()
        {
            var numbers = new List<int>();
            for (int i = 1; i <= 50; i++) numbers.Add(i);

            // Process in partitions of 12 items each
            var partitioner = new PartitionerCollection<int>(numbers, partitionSize: 12);

            Console.WriteLine($"Total items: {partitioner.Count}");
            Console.WriteLine($"Partition size: {partitioner.PartitionSize}");
            Console.WriteLine($"Total partitions: {partitioner.PartitionsCount}");
            Console.WriteLine($"Items remaining: {partitioner.Remaining}");
            Console.WriteLine();

            int partitionIndex = 0;
            while (partitioner.HasPartitions)
            {
                partitionIndex++;
                Console.Write($"Partition {partitionIndex}: ");
                foreach (var item in partitioner)
                {
                    Console.Write($"{item} ");
                Console.WriteLine();

            Console.WriteLine();
            Console.WriteLine($"Iterated count: {partitioner.IteratedCount}");
            Console.WriteLine($"Has remaining partitions: {partitioner.HasPartitions}");

}}}}
}

```
