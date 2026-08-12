using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.AspNetCore.Mvc;
using Cuemon.Collections.Generic;
using Cuemon.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Cuemon.Extensions.AspNetCore.Mvc;
/// <summary>
/// Extension methods for the <see cref="ViewDataDictionary"/> class. Experimental.
/// </summary>
public static class ViewDataDictionaryExtensions
{
    private const string BreadcrumbKey = "breadcrumbs";

    /// <summary>
    /// Adds a sequence of <see cref="Breadcrumb"/> objects to the specified <paramref name="viewData"/>.
    /// </summary>
    /// <typeparam name="T">The type of the model to retrieve breadcrumb labels from.</typeparam>
    /// <param name="viewData">The <see cref="ViewDataDictionary"/> to extend.</param>
    /// <param name="controller">The controller to resolve all public methods with <see cref="IActionResult"/> as return type from.</param>
    /// <param name="model">The model to retrieve custom breadcrumb labels from.</param>
    /// <param name="initializer">The function delegate that will initialize labels from the spcified <paramref name="model"/>.</param>
    public static void AddBreadcrumbs<T>(this ViewDataDictionary viewData, Controller controller, T model, Func<T, IEnumerable<string>> initializer)
    {
        var list = new List<Breadcrumb>();
        var ct = controller.GetType();
        var actions = ct.GetMethods(new MemberReflection(true, true)).Where(mi => mi.ReturnType == typeof(IActionResult)).ToList();
        var labelInvokers = initializer(model).ToList();
        for (int i = 0; i < actions.Count; i++)
        {
            var bc = new Breadcrumb()
            {
                ActionName = actions[i].Name,
                ControllerName = controller.RouteData.Values["controller"] as string,
                Label = labelInvokers[i]
            };
            list.Add(bc);
        }

        Decorator.Enclose(viewData).AddOrUpdate(BreadcrumbKey, list);
    }

    /// <summary>
    /// Gets a sequence of <see cref="Breadcrumb"/> objects from the specified <paramref name="viewData"/>.
    /// </summary>
    /// <param name="viewData">The <see cref="ViewDataDictionary"/> to extend.</param>
    /// <param name="razor">The razor page from where the breadcrumbs will be rendered.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> sequence of <see cref="Breadcrumb"/> objects (if any).</returns>
    public static IEnumerable<Breadcrumb> GetBreadcrumbs(this ViewDataDictionary viewData, IRazorPage razor)
    {
        var breadcrumbs = viewData[BreadcrumbKey] as List<Breadcrumb> ?? new List<Breadcrumb>();
        return breadcrumbs.TakeWhile(bc => !bc.ActionName?.Equals(razor.ViewContext.RouteData.Values["action"] as string, StringComparison.OrdinalIgnoreCase) ?? false);
    }
}
