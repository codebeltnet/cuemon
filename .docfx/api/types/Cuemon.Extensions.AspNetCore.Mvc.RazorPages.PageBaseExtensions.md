---
uid: Cuemon.Extensions.AspNetCore.Mvc.RazorPages.PageBaseExtensions
example:
- *content
---

The following example demonstrates how to resolve application-base URLs and CDN URLs for static resources from a Razor Page model. The sample configures application and CDN tag-helper options, creates a PageBase with request services, and prints the resolved static-resource URLs.

```csharp
// Program.cs
using System;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Razor.TagHelpers;
using Cuemon.Extensions.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.Configure<AppTagHelperOptions>(options =>
{
    options.Scheme = ProtocolUriScheme.Relative;
    options.BaseUrl = "static.cuemon.net";
});
services.Configure<CdnTagHelperOptions>(options =>
{
    options.Scheme = ProtocolUriScheme.Https;
    options.BaseUrl = "nblcdn.net";
});

using var provider = services.BuildServiceProvider();
var page = new ExamplePage
{
    PageContext = new PageContext
    {
        HttpContext = new DefaultHttpContext { RequestServices = provider }
    }
};

Console.WriteLine(page.GetAppUrl("js/site.js"));
Console.WriteLine(page.GetCdnUrl("images/logo.png"));

public sealed class ExamplePage : PageBase
{
    public override Task ExecuteAsync()
    {
        return Task.CompletedTask;
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="c38cac57fc823be54704af69"><a href="~/run-actions/c38cac57fc823be54704af69/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/c38cac57fc823be54704af69/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
