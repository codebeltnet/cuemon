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
using Microsoft.Extensions.Options;

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
        app.MapGet("/", (IOptions<AppTagHelperOptions> options) => $"{options.Value.Scheme}://{options.Value.BaseUrl}");
        app.Run();
    }
}

```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="8edf4fd8b1b5524c7260ae62"><a href="~/run-actions/8edf4fd8b1b5524c7260ae62/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/8edf4fd8b1b5524c7260ae62/docker.ps1" data-transport="docker">View Docker Script</a><p>Starts the host with a timeout, performs the demonstrated action, and shuts down cleanly.</p></div>
<!-- docfx-run-actions:end -->
