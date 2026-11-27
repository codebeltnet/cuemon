---
uid: Cuemon.Security.FowlerNollVoAlgorithm
example:
- *content
---

The following example demonstrates how to use the `FowlerNollVoAlgorithm` enumeration to select between the FNV-1 and FNV-1a hash variants.

```csharp
using System;
using Cuemon.Security;

namespace MyApp.Examples;

public class FowlerNollVoAlgorithmExample
{
    public static void Main()
    {
        FowlerNollVoAlgorithm[] variants = { FowlerNollVoAlgorithm.Fnv1, FowlerNollVoAlgorithm.Fnv1a };

        foreach (var variant in variants)
        {
            string label = variant switch
            {
                FowlerNollVoAlgorithm.Fnv1 => "FNV-1 (original)",
                FowlerNollVoAlgorithm.Fnv1a => "FNV-1a (recommended)",
                _ => "Unknown"
            };
            Console.WriteLine("{0} -> {1}", variant, label);

        // Output:
        // Fnv1 -> FNV-1 (original)
        // Fnv1a -> FNV-1a (recommended)

}}
}

```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="81b0607d5a43c84eea6fc977"><a href="~/run-actions/81b0607d5a43c84eea6fc977/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/81b0607d5a43c84eea6fc977/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
