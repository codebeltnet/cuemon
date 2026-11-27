---
uid: Cuemon.Collections.Specialized.DictionaryDecoratorExtensions
example:
- *content
---

The following example demonstrates how to use the `ToNameValueCollection` extension method to convert an `IDictionary<string, string[]>` into a `NameValueCollection`.

```csharp
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Cuemon;
using Cuemon.Collections.Specialized;

namespace MyApp.Examples;

public class DictionaryDecoratorExtensionsExample
{
    public static void Main()
    {
        var input = new Dictionary<string, string[]>
        {
            ["colors"] = new[] { "red", "green", "blue" },
            ["sizes"] = new[] { "small", "medium", "large" }
        };

        // Wrap the dictionary with Decorator and call the extension method.
        NameValueCollection nvc = Decorator.Enclose(input).ToNameValueCollection();

        foreach (string key in nvc)
        {
            Console.WriteLine("{0} = {1}", key, nvc[key]);

        // Output:
        // colors = red,green,blue
        // sizes = small,medium,large

}}
}

```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="81ffca40f4929d80ab9e544a"><a href="~/run-actions/81ffca40f4929d80ab9e544a/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/81ffca40f4929d80ab9e544a/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
