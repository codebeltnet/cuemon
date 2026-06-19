---
uid: Cuemon.Reflection.MemberArgument
example:
- *content
---

The following example demonstrates how to create and use MemberArgument instances to represent method parameters and their values, with priority support for ordering.

```csharp
using System;
using Cuemon.Reflection;

namespace MyApp.Reflection
{
    public static class MemberArgumentExamples
    {
        public static void Demonstrate()
        {
            // Create a MemberArgument to represent a method parameter and its value.
            var arg = new MemberArgument("id", 42);
            Console.WriteLine("Name: {0}", arg.Name);       // id
            Console.WriteLine("Value: {0}", arg.Value);      // 42
            Console.WriteLine("Priority: {0}", arg.Priority); // 0

            // Create arguments for use with MemberParser rehydration.
            var args = new[]
            {
                new MemberArgument("name", "Widget"),
                new MemberArgument("price", 19.99m),
                new MemberArgument("quantity", 100)
            };

            // Priority can control the order of processing.
            args[0].Priority = 2;
            args[1].Priority = 1;
            args[2].Priority = 0;

            foreach (var a in args)
            {
                Console.WriteLine("{0} = {1} (priority {2})", a.Name, a.Value, a.Priority);

            // Update a value after creation.
            var updatable = new MemberArgument("status", "pending");
            updatable.Value = "shipped";
            updatable.Priority = 5;
            Console.WriteLine("Updated: {0}", updatable); // [status, shipped, 5]

}}}
}

```
