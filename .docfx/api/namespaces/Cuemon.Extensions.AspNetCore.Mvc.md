---
uid: Cuemon.Extensions.AspNetCore.Mvc
summary: *content
---
Add cache-control headers, entity tags, and last-modified headers to ASP.NET Core MVC action results with fluent extension methods. Use this namespace when you need to apply HTTP caching headers to MVC action results. Start with `WithCacheableHeaders<T>` on any object to combine entity-tag and last-modified headers in one call.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

Complements: [Cuemon.AspNetCore.Mvc namespace](/api/aspnet/Cuemon.AspNetCore.Mvc.html) 📘

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|T|⬇️|`WithLastModifiedHeader<T>`, `WithEntityTagHeader<T>`, `WithCacheableHeaders<T>`|
|ViewDataDictionary|⬇️|`AddBreadcrumbs<T>`, `GetBreadcrumbs`|
