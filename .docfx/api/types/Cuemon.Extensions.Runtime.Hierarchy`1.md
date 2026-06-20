---
uid: Cuemon.Extensions.Runtime.Hierarchy`1
example:
- *content
---

The following example demonstrates how to build a tree structure using <see cref="Cuemon.Extensions.Runtime.Hierarchy{T}"/> and navigate nodes through parent-child relationships, depth, index, and path.

```csharp
using System;
using System.Linq;
using Cuemon.Extensions.Runtime;

namespace MyApp.Examples;

public class HierarchyOfTExample
{
    public void Demonstrate()
    {
        var root = new Hierarchy<string>();
        var rootNode = root.Add("Root");
        var child = root.Add("Child");
        var grandchild = child.Add("Grandchild");
        var sibling = root.Add("Sibling");

        Console.WriteLine($"Root depth: {root.Depth}, index: {root.Index}");       // 0, 0
        Console.WriteLine($"Child depth: {child.Depth}, index: {child.Index}");    // 1, 1
        Console.WriteLine($"Grandchild depth: {grandchild.Depth}");                 // 2
        Console.WriteLine($"Path: {grandchild.GetPath()}");                         // Root.Child.Grandchild
        Console.WriteLine($"HasChildren: {root.HasChildren}");                      // True
        Console.WriteLine($"HasParent: {child.HasParent}");                         // True

        // Retrieve via indexer
        Console.WriteLine(root[0].Instance); // Root
        Console.WriteLine(root[2].Instance); // Grandchild

        // Enumerate children
        foreach (var node in root.GetChildren())
        {
            Console.WriteLine(node.Instance);
        // Output: Child, Sibling

}}
}

```
