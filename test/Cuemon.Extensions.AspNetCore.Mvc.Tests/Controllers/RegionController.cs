using System.Collections.Generic;
using Cuemon.Collections.Generic;
using Cuemon.Extensions.AspNetCore.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cuemon.Extensions.AspNetCore.Mvc.Controllers;
[Route("regions")]
public class RegionController : Controller
{

    public IActionResult Index()
    {
        var model = new RegionModel();
        ViewData.AddBreadcrumbs(this, model, RegionInitializer);
        return View(model);
    }

    [Route("{regionName}/{regionDisplayName}")]
    public IActionResult Region(string regionName, string regionDisplayName)
    {
        var model = new RegionModel(regionName);
        ViewData.AddBreadcrumbs(this, model, RegionInitializer);
        return View("CultureCollection", model);
    }

    [Route("{regionName}/{regionDisplayName}/cultures/{cultureName}")]
    public IActionResult Culture(string regionName, string regionDisplayName, string cultureName)
    {
        var model = new RegionModel(regionName, cultureName);
        ViewData.AddBreadcrumbs(this, model, RegionInitializer);
        return View("Culture", model);
    }

    private IEnumerable<string> RegionInitializer(RegionModel m)
    {
        return Arguments.ToEnumerableOf("Regions",
            m.Region?.DisplayName,
            m.Culture?.DisplayName);
    }
}
