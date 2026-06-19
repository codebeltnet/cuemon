---
uid: Cuemon.Extensions.AspNetCore.Http.Headers.EntityTagCacheableValidator
example:
- *content
---

The following example demonstrates how to register <xref cref="Cuemon.Extensions.AspNetCore.Http.Headers.EntityTagCacheableValidator"/> as a cacheable validator in the ASP.NET Core pipeline.

```csharp
using System;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.Extensions.AspNetCore.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DocfxExamples;

public class EntityTagCacheableValidatorExample
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<CacheableOptions>(options =>
        {
            options.Validators.Add(new EntityTagCacheableValidator());
        });

        var serviceProvider = services.BuildServiceProvider();
        var options = serviceProvider.GetRequiredService<IOptions<CacheableOptions>>();
        Console.WriteLine(options.Value.Validators.Count); // 1
        Console.WriteLine(options.Value.Validators[0] is EntityTagCacheableValidator); // True

}
}

```
