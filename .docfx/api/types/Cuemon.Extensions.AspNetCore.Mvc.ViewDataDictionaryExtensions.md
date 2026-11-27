---
uid: Cuemon.Extensions.AspNetCore.Mvc.ViewDataDictionaryExtensions
example:
- *content
---

The following example follows the same pattern as the sample MVC app in the test project: controller actions populate breadcrumbs from the current model, and a shared Razor partial reads them back from <see cref="ViewDataDictionary"/>. Three `RegionController` actions call `AddBreadcrumbs` with a `RegionPageModel` that holds hierarchical labels. An `ExampleRazorPage` then retrieves the breadcrumbs via `GetBreadcrumbs`, and the sample prints each breadcrumb, demonstrating hierarchical navigation-data propagation in ASP.NET Core MVC.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Mvc;
using Cuemon.Extensions.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Cuemon.Extensions.AspNetCore.Mvc.DocExamples;

ViewDataDictionaryExtensionsExample.Demonstrate();

namespace Cuemon.Extensions.AspNetCore.Mvc.DocExamples
{
    public class RegionController : Controller
    {
        public IActionResult Index()
        {
            var model = new RegionPageModel("Regions", "Northern Europe", "Danish");
            ViewData.AddBreadcrumbs(this, model, page => page.Labels);
            return new EmptyResult();
        }

        public IActionResult Region(string regionName, string regionDisplayName)
        {
            var model = new RegionPageModel("Regions", regionDisplayName, "Danish");
            ViewData.AddBreadcrumbs(this, model, page => page.Labels);
            return new EmptyResult();
        }

        public IActionResult Culture(string regionName, string regionDisplayName, string cultureName)
        {
            var model = new RegionPageModel("Regions", regionDisplayName, cultureName);
            ViewData.AddBreadcrumbs(this, model, page => page.Labels);
            return new EmptyResult();
        }
    }
    public static class ViewDataDictionaryExtensionsExample
    {
        public static void Demonstrate()
        {
            var controller = new RegionController
            {
                ControllerContext = new ControllerContext
                {
                    RouteData = new RouteData()
                }
            };
            controller.ControllerContext.RouteData.Values["controller"] = "Region";
            controller.ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());

            var model = new RegionPageModel("Regions", "Northern Europe", "Danish");
            controller.ViewData.AddBreadcrumbs(controller, model, page => page.Labels);

            var razorPage = new ExampleRazorPage
            {
                ViewContext = new ViewContext
                {
                    RouteData = controller.ControllerContext.RouteData
                }
            };
            var breadcrumbs = controller.ViewData.GetBreadcrumbs(razorPage).ToList();
            foreach (var breadcrumb in breadcrumbs)
            {
                Console.WriteLine($"{breadcrumb.Label} ({breadcrumb.ControllerName}/{breadcrumb.ActionName})");
            }
        }
    }
    public sealed class ExampleRazorPage : RazorPage
    {
        public override Task ExecuteAsync() => Task.CompletedTask;
    }
    public sealed class RegionPageModel
    {
        public RegionPageModel(params string[] labels)
        {
            Labels = labels;
        }

        public IReadOnlyList<string> Labels { get; }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="3698a43fb3254212d9728bdf"><a href="~/run-actions/3698a43fb3254212d9728bdf/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/3698a43fb3254212d9728bdf/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
