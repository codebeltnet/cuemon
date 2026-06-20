---
uid: Cuemon.Extensions.AspNetCore.Mvc.Filters.Cacheable
summary: *content
---
Add or insert custom cacheable filters and attach ETag or Last-Modified headers to HTTP responses in the ASP.NET Core MVC pipeline. Use this namespace when you need to extend cacheable filters or attach HTTP caching headers to MVC responses. Start with `AddFilter<T>` to register a new cacheable filter or `AddEntityTagHeader` for ETag header support.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

Complements: [Cuemon.AspNetCore.Mvc.Filters.Cacheable namespace](/api/aspnet/Cuemon.AspNetCore.Mvc.Filters.Cacheable.html) 🔗

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IList<ICacheableAsyncResultFilter>|⬇️|`AddFilter<T>`, `AddFilter<T, TOptions>`, `InsertFilter<T>`, `InsertFilter<T, TOptions>`|
|ICacheableAsyncResultFilter|⬇️|`AddEntityTagHeader`, `AddLastModifiedHeader`|