---
uid: Cuemon.Reflection.MethodDescriptor
example:
- *content
---

The following example demonstrates how to create a MethodDescriptor from a MethodInfo, inspect its parameters, append runtime arguments, and merge parameter signatures with values.

```csharp
using System;
using System.Linq;
using System.Reflection;
using Cuemon.Reflection;

namespace MyApp.Reflection
{
    public static class MethodDescriptorExamples
    {
        public static void Demonstrate()
        {
            // Create a MethodDescriptor from a MethodInfo.
            MethodInfo methodInfo = typeof(string).GetMethod("IndexOf",
                new[] { typeof(string), typeof(StringComparison) });
            var descriptor = new MethodDescriptor(methodInfo);

            Console.WriteLine("Caller: {0}", descriptor.Caller.FullName);   // System.String
            Console.WriteLine("Method: {0}", descriptor.MethodName);        // IndexOf
            Console.WriteLine("Signature: {0}", descriptor.ToString(true));
            // Output: System.String.IndexOf(String value, StringComparison comparisonType)

            // List all parameters.
            Console.WriteLine("Parameters:");
            foreach (var param in descriptor.Parameters)
            {
                Console.WriteLine("  {0} {1}", param.ParameterType.Name, param.ParameterName);

            // Append runtime arguments for debugging or logging.
            descriptor.AppendRuntimeArguments("Hello", StringComparison.OrdinalIgnoreCase);
            Console.WriteLine("Runtime arguments:");
            foreach (var kvp in descriptor.RuntimeArguments)
            {
                Console.WriteLine("  {0} = {1}", kvp.Key, kvp.Value);

            // Static factory method.
            var fromFactory = MethodDescriptor.Create(
                typeof(Math).GetMethod("Max", new[] { typeof(int), typeof(int) }));
            Console.WriteLine("Factory: {0}", fromFactory.ToString(false));

            // Merge parameters with runtime values.
            var merged = MethodDescriptor.MergeParameters(
                new[] {
                    new ParameterSignature(typeof(string), "input"),
                    new ParameterSignature(typeof(int), "count")
                },
                "test", 3);
            Console.WriteLine("Merged: input={0}, count={1}", merged["input"], merged["count"]);

}}}}
}

```
