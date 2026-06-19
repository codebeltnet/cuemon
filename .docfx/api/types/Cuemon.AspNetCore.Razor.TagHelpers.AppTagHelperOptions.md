---
uid: Cuemon.AspNetCore.Razor.TagHelpers.AppTagHelperOptions
example:
- *content
---

The following example demonstrates how to configure <xref cref="Cuemon.AspNetCore.Razor.TagHelpers.AppTagHelperOptions"/> to customize the base URL and URI scheme for application-scoped tag helpers.

```csharp
using Cuemon.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MyApp.Examples;

public class AppTagHelperOptionsExample
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorPages();
        builder.Services.Configure<AppTagHelperOptions>(o =>
        {
            o.Scheme = ProtocolUriScheme.Https;
            o.BaseUrl = "static.example.com";
        });

        var app = builder.Build();
        app.Run();
    }
}

```
