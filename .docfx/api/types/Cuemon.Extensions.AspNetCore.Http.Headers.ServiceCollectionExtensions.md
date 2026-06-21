---
uid: Cuemon.Extensions.AspNetCore.Http.Headers.ServiceCollectionExtensions
example:
- *content
---

The following example demonstrates how to register API key and User-Agent validation rules and then inspect the resolved options through `IOptions<T>`.

```csharp
using System;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.Extensions.AspNetCore.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DocfxExamples;

public class HeaderServiceCollectionExtensionsExample
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddOptions();
        services.AddApiKeySentinelOptions(options =>
        {
            options.AllowedKeys.Add("known-key");
            options.HeaderName = "X-Test-Key";
            options.UseGenericResponse = true;
        });
        services.AddUserAgentSentinelOptions(options =>
        {
            options.AllowedUserAgents.Add("Cuemon-Agent");
            options.RequireUserAgentHeader = true;
            options.ValidateUserAgentHeader = true;
        });

        var provider = services.BuildServiceProvider();
        var apiKeyOptions = provider.GetRequiredService<IOptions<ApiKeySentinelOptions>>().Value;
        var userAgentOptions = provider.GetRequiredService<IOptions<UserAgentSentinelOptions>>().Value;

        Console.WriteLine($"{apiKeyOptions.HeaderName}:{apiKeyOptions.AllowedKeys.Count}");
        Console.WriteLine($"{userAgentOptions.RequireUserAgentHeader}:{userAgentOptions.AllowedUserAgents.Count}");
    }
}
```
