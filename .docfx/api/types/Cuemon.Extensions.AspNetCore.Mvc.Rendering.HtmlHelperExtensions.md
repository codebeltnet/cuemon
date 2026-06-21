---
uid: Cuemon.Extensions.AspNetCore.Mvc.Rendering.HtmlHelperExtensions
example:
- *content
---

The following example demonstrates how to conditionally render content based on the current controller and action using `UseWhenView`, and based on the current Razor Page using `UseWhenPage`.

```csharp
using Cuemon.Extensions.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyApp.Examples;

public class HtmlHelperExtensionsExample
{
    public void Demonstrate(IHtmlHelper helper)
    {
        var showBanner = helper.UseWhenView("Index", "Home", () => "<div>Welcome to the Home Page</div>");

        var pageTitle = helper.UseWhenPage("Contact", () => "Contact Us");

}
}

```
