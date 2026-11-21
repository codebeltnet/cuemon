---
uid: Cuemon.Security.NonCryptoAlgorithm
example:
- *content
---

The following example demonstrates how to use the `NonCryptoAlgorithm` enumeration to select a non-cryptographic hash algorithm.

```csharp
using System;
using Cuemon.Security;

namespace MyApp.Examples;

public class NonCryptoAlgorithmExample
{
    public static void Main()
    {
        NonCryptoAlgorithm[] algorithms = 
        {
            NonCryptoAlgorithm.Fnv32,
            NonCryptoAlgorithm.Fnv64,
            NonCryptoAlgorithm.Fnv128,
            NonCryptoAlgorithm.Fnv256,
            NonCryptoAlgorithm.Fnv512,
            NonCryptoAlgorithm.Fnv1024
        };

        foreach (var algo in algorithms)
        {
            Console.WriteLine("{0} (value: {1})", algo, (int)algo);

        // Output:
        // Fnv32 (value: 0)
        // Fnv64 (value: 1)
        // Fnv128 (value: 2)
        // Fnv256 (value: 3)
        // Fnv512 (value: 4)
        // Fnv1024 (value: 5)

}}
}

```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="40ffa641c4095bb1836c0463"><a href="~/run-actions/40ffa641c4095bb1836c0463/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/40ffa641c4095bb1836c0463/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
