---
uid: Cuemon.ObjectDecoratorExtensions
example:
- *content
---

`ObjectDecoratorExtensions` provides extension methods on `Decorator.Enclose` for type conversion, hierarchical tree traversal, and property resolution via reflection. This example converts string representations of `"42"`, `"2024-01-15T10:30:00Z"`, `"not-a-number"`, and `"Ascending"` into their target types using `ChangeType<T>` and `ChangeTypeOrDefault<T>` with fallback support. It also builds a three-level `TreeNode` hierarchy and visits all nodes with `TraverseWhileNotEmpty`, and reads a property value through `DefaultPropertyValueResolver`. Console output confirms each type conversion succeeded and lists all visited node names in depth-first traversal order.

```csharp
using System;
using System.Collections.Generic;
using System.Globalization;
using Cuemon;

namespace DocExamples
{
    public static class ObjectDecoratorExamples
    {
        public static void Main()
        {
            // Convert a string to an integer
            object input = "42";
            int result = Decorator.Enclose(input).ChangeType<int>();
            Console.WriteLine($"Converted string to int: {result} (type: {result.GetType().Name})");

            // Convert a string to DateTime (UTC)
            object dateInput = "2024-01-15T10:30:00Z";
            DateTime dateResult = Decorator.Enclose(dateInput).ChangeType<DateTime>();
            Console.WriteLine($"Converted to DateTime: {dateResult} (Kind: {dateResult.Kind})");

            // Convert with a fallback value
            object invalidInput = "not-a-number";
            int fallbackResult = Decorator.Enclose(invalidInput).ChangeTypeOrDefault<int>(42);
            Console.WriteLine($"Conversion with fallback: {fallbackResult}");

            // Convert a string to an enum
            object enumInput = "Ascending";
            var enumResult = Decorator.Enclose(enumInput).ChangeType<SortOrder>();
            Console.WriteLine($"Converted to SortOrder: {enumResult}");

            // Traverse a hierarchical tree structure
            var grandchild = new TreeNode { Name = "Grandchild" };
            var child1 = new TreeNode { Name = "Child1", Children = { grandchild } };
            var child2 = new TreeNode { Name = "Child2" };
            var root = new TreeNode { Name = "Root", Children = { child1, child2 } };

            var allNodes = Decorator.Enclose(root).TraverseWhileNotEmpty(node => node.Children);
            foreach (var node in allNodes)
            {
                Console.WriteLine($"Visited: {node.Name}");
            }

            var property = typeof(TreeNode).GetProperty(nameof(TreeNode.Name));
            var resolvedName = Decorator.Enclose((object)root).DefaultPropertyValueResolver(property);
            Console.WriteLine($"Resolved property value: {resolvedName}");
        }

        private class TreeNode
        {
            public string Name { get; set; }
            public List<TreeNode> Children { get; } = new List<TreeNode>();
        }
    }
}
```
