---
uid: Cuemon.Extensions.AspNetCore.Configuration.ServiceCollectionExtensions
example:
- *content
---

The following example demonstrates how to register the built-in cache-busting services and a custom `ICacheBusting` implementation.

```csharp
using System;
using System.Linq;
using Cuemon.AspNetCore.Configuration;
using Cuemon.Extensions.AspNetCore.Configuration;
using Cuemon.Security.Cryptography;
using Microsoft.Extensions.DependencyInjection;

namespace DocfxExamples;

public class ConfigurationServiceCollectionExtensionsExample
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddOptions();
        services.Configure<AssemblyCacheBustingOptions>(options =>
        {
            options.Assembly = typeof(ConfigurationServiceCollectionExtensionsExample).Assembly;
            options.Algorithm = UnkeyedCryptoAlgorithm.Sha256;
            options.ReadByteForByteChecksum = true;
        });

        services.AddAssemblyCacheBusting();
        services.AddDynamicCacheBusting();
        services.AddCacheBusting<ReleaseCacheBusting>();

        var provider = services.BuildServiceProvider();
        var versions = provider.GetServices<ICacheBusting>().Select(cache => cache.Version).ToList();

        Console.WriteLine(versions.Count);
        Console.WriteLine(string.Join(", ", versions));
    }

    private sealed class ReleaseCacheBusting : ICacheBusting
    {
        public string Version => "20260618";
    }
}
```
