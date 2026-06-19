---
uid: Cuemon.Extensions.AspNetCore.Http.Throttling
summary: *content
---
Configure throttling middleware for your ASP.NET Core application with an in-memory throttling cache. Use this namespace when you need to rate-limit requests based on client characteristics. Start with `UseThrottlingSentinel` on `IApplicationBuilder` to enable rate limiting, or register a cache provider with `AddMemoryThrottlingCache` or `AddThrottlingCache<T>` on `IServiceCollection`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

Complements: [Cuemon.AspNetCore.Http.Throttling namespace](/api/aspnet/Cuemon.AspNetCore.Http.Throttling.html) 📘

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IApplicationBuilder|⬇️|`UseThrottlingSentinel`|
|IServiceCollection|⬇️|`AddThrottlingCache<T>`, `AddMemoryThrottlingCache`, `AddThrottlingSentinelOptions`|
