---
uid: Cuemon.Extensions.AspNetCore.Mvc.Rendering
summary: *content
---
Conditionally render HTML content based on the current Razor Page or view type. Use this namespace when you need type-conditional rendering in ASP.NET Core views and pages. Start with `UseWhenPage<T>` to render content only for specific page types, or `UseWhenView<T>` for view-specific content.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

Complements: [Microsoft.AspNetCore.Mvc.Rendering namespace](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.rendering) 🔗

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IHtmlHelper|⬇️|`UseWhenPage<T>`, `UseWhenView<T>`|
