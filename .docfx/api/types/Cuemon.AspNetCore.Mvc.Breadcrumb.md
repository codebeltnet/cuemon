---
uid: Cuemon.AspNetCore.Mvc.Breadcrumb
example:
- *content
---

The following example demonstrates how to use <xref cref="Cuemon.AspNetCore.Mvc.Breadcrumb"/> to build navigation breadcrumbs for an MVC application.

```csharp
using System;
using System.Collections.Generic;
using Cuemon.AspNetCore.Mvc;

namespace MyApp.Examples;

public class BreadcrumbExample
{
    public void Demonstrate()
    {
        // Build a breadcrumb trail for a product detail page
        var breadcrumbs = new List<Breadcrumb>
        {
            new() { Label = "Home", ControllerName = "Home", ActionName = "Index" },
            new() { Label = "Products", ControllerName = "Product", ActionName = "Index" },
            new() { Label = "Electronics", ControllerName = "Product", ActionName = "Category" },
            new() { Label = "Smartphone X", ControllerName = "Product", ActionName = "Details" }
        };

        // Render breadcrumb items
        foreach (var crumb in breadcrumbs)
        {
            Console.WriteLine($"<a href='/{crumb.ControllerName}/{crumb.ActionName}'>{crumb.Label}</a>");
        // Output:
        //   <a href='/Home/Index'>Home</a>
        //   <a href='/Product/Index'>Products</a>
        //   <a href='/Product/Category'>Electronics</a>
        //   <a href='/Product/Details'>Smartphone X</a>

        // The last breadcrumb typically represents the current page (no link)
        var current = breadcrumbs[^1];
        Console.WriteLine($"<span>{current.Label}</span>");
        // Output: <span>Smartphone X</span>

}}
}

```
