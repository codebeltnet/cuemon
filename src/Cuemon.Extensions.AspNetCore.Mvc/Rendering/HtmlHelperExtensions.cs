using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cuemon.Extensions.AspNetCore.Mvc.Rendering;
/// <summary>
/// Extension methods for the <see cref="IHtmlHelper"/> interface.
/// </summary>
public static class HtmlHelperExtensions
{
    /// <summary>
    /// Creates a value of <typeparamref name="T"/> when the specified <paramref name="action"/> name and <paramref name="controller"/> name is matched in <see cref="ActionContext.RouteData"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value for the matched action method.</typeparam>
    /// <param name="helper">The <see cref="IHtmlHelper"/> to extend.</param>
    /// <param name="action">The action name to match.</param>
    /// <param name="controller">The controller name to match.</param>
    /// <param name="body">The function delegate that, when both <paramref name="action"/> and <paramref name="controller"/> is matched in <see cref="ActionContext.RouteData"/>, returns <typeparamref name="T"/>.</param>
    /// <returns>Either the value of the function delegate <paramref name="body"/> or <b>default(T)</b>.</returns>
    public static T UseWhenView<T>(this IHtmlHelper helper, string action, string controller, Func<T> body)
    {
        T result = default;
        UseWhenCore(helper, action, controller, () =>
        {
            result = body();
        }, "controller", "action");
        return result;
    }

    /// <summary>
    /// Creates a value of <typeparamref name="T"/> when the specified <paramref name="target"/> name is matched in <see cref="ActionContext.RouteData"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value for the matched action method.</typeparam>
    /// <param name="helper">The <see cref="IHtmlHelper"/> to extend.</param>
    /// <param name="target">The page name to match.</param>
    /// <param name="body">The function delegate that, when <paramref name="target"/> is matched in <see cref="ActionContext.RouteData"/>, returns <typeparamref name="T"/>.</param>
    /// <returns>Either the value of the function delegate <paramref name="body"/> or <b>default(T)</b>.</returns>
    public static T UseWhenPage<T>(this IHtmlHelper helper, string target, Func<T> body)
    {
        T result = default;
        UseWhenCore(helper, null, target, () =>
        {
            result = body();
        }, "page", null);
        return result;
    }

    private static void UseWhenCore(IHtmlHelper helper, string template, string target, Action whenMatchDelegate, string routeTarget, string routeTemplate)
    {
        var viewTarget = helper.ViewContext.RouteData.Values[routeTarget] as string;
        var viewTemplate = template == null ? null : helper.ViewContext.RouteData.Values[routeTemplate] as string;
        var templates = template?.Split(',');
        var targets = target.Split(',');
        templates ??= targets;
        foreach (var te in templates)
        {
            foreach (var ta in targets)
            {
                var match = viewTemplate?.Equals(te, StringComparison.OrdinalIgnoreCase) ?? true;
                match &= viewTarget?.Equals(ta, StringComparison.OrdinalIgnoreCase) ?? false;
                if (match) { whenMatchDelegate?.Invoke(); }
            }
        }
    }
}
