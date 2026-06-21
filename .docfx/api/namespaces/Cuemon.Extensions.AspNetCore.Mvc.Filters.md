---
uid: Cuemon.Extensions.AspNetCore.Mvc.Filters
summary: *content
---
Register ASP.NET Core MVC filters for caching, fault descriptors, server timing, user-agent sentinel, throttling, and API-key sentinel with a single method call. Use this namespace when you need to add HTTP cache validation, structured error handling, server timing, or throttling filters to your MVC filter collection. Start with `AddHttpCacheable` for cache support or `AddFaultDescriptor` for error handling.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

Complements: [Cuemon.AspNetCore.Mvc.Filters namespace](/api/aspnet/Cuemon.AspNetCore.Mvc.Filters.html) 📘

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|FilterCollection|⬇️|`AddHttpCacheable`, `AddFaultDescriptor`, `AddServerTiming`, `AddUserAgentSentinel`, `AddThrottlingSentinel`, `AddApiKeySentinel`|
|IMvcBuilder|⬇️|`AddHttpCacheableOptions`, `AddFaultDescriptorOptions`, `AddServerTimingOptions`, `AddUserAgentSentinelOptions`, `AddThrottlingSentinelOptions`, `AddApiKeySentinelOptions`|
