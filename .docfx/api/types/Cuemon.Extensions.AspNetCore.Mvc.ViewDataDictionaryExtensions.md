---
uid: Cuemon.Extensions.AspNetCore.Mvc.ViewDataDictionaryExtensions
example:
- *content
---

The following example follows the same pattern as the sample MVC app in the test project: controller actions populate breadcrumbs from the current model, and a shared Razor partial reads them back from <see cref="ViewDataDictionary"/>. Three `RegionController` actions call `AddBreadcrumbs` with a `RegionPageModel` that holds hierarchical labels. A `BreadcrumbPartial` class then retrieves the breadcrumbs via `GetBreadcrumbs` and projects each into a readable string, demonstrating hierarchical navigation-data propagation in ASP.NET Core MVC.

```csharp
using System.Collections.Generic;
using System.Linq;
using Cuemon.Extensions.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Cuemon.Extensions.AspNetCore.Mvc.DocExamples;

public sealed class RegionController : Controller
{
    public IActionResult Index()
    {
        var model = new RegionPageModel("Regions", "Northern Europe", "Danish");
        ViewData.AddBreadcrumbs(this, model, page => page.Labels);
        return View(model);
    }

    public IActionResult Region(string regionName, string regionDisplayName)
    {
        var model = new RegionPageModel("Regions", regionDisplayName, "Danish");
        ViewData.AddBreadcrumbs(this, model, page => page.Labels);
        return View("CultureCollection", model);
    }

    public IActionResult Culture(string regionName, string regionDisplayName, string cultureName)
    {
        var model = new RegionPageModel("Regions", regionDisplayName, cultureName);
        ViewData.AddBreadcrumbs(this, model, page => page.Labels);
        return View("Culture", model);
    }
}

public sealed class BreadcrumbPartial
{
    public IReadOnlyList<string> Render(ViewDataDictionary viewData, IRazorPage currentPage)
    {
        return viewData
            .GetBreadcrumbs(currentPage)
            .Select(link => $"{link.Label} ({link.ControllerName}/{link.ActionName})")
            .ToList();
    }
}

public sealed class RegionPageModel
{
    public RegionPageModel(params string[] labels)
    {
        Labels = labels;
    }

    public IReadOnlyList<string> Labels { get; }
}
```
