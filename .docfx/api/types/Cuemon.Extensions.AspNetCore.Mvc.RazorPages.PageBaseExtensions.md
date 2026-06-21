---
uid: Cuemon.Extensions.AspNetCore.Mvc.RazorPages.PageBaseExtensions
example:
- *content
---

The following example demonstrates how to resolve application-base URLs and CDN URLs for static resources from a Razor Page model.

```csharp
using Cuemon.Extensions.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Examples;

public class PageBaseExtensionsExample
{
    public void Demonstrate(PageBase pageModel)
    {
        string appScript = pageModel.GetAppUrl("js/site.js");
        string cdnImage = pageModel.GetCdnUrl("images/logo.png");

}
}

```
