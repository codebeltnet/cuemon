---
uid: Cuemon.AspNetCore.Configuration.DynamicCacheBusting
example:
- *content
---

The following example demonstrates how to use `DynamicCacheBusting` to resolve versioned static resource URLs at runtime.

```csharp
using System;
using Cuemon;
using Cuemon.AspNetCore.Configuration;
using Microsoft.Extensions.Options;

namespace MyApp.Examples;

public class DynamicCacheBustingExample
{
    public void Demonstrate()
    {
        var options = Options.Create(new DynamicCacheBustingOptions
        {
            PreferredLength = 8,
            PreferredCharacters = Alphanumeric.LettersAndNumbers,
            TimeToLive = TimeSpan.FromHours(12)
        });

        var cacheBusting = new DynamicCacheBusting(options);

        // Version is regenerated when the configured TimeToLive has elapsed.
        string version = cacheBusting.Version;
        Console.WriteLine(version);
    }
}

```
