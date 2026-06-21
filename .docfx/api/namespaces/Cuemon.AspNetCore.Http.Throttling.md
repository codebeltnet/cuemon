---
uid: Cuemon.AspNetCore.Http.Throttling
summary: *content
---
Limit HTTP requests in ASP.NET Core using a middleware-based throttling mechanism with configurable quotas and time windows tied to request context (IP address, authorization header, etc.). Use this namespace when you need rate limiting per client or request characteristic. Start with `ThrottlingSentinelMiddleware` and `ThrottlingSentinelOptions` to configure quotas and windows. For custom throttling storage, implement `IThrottlingCache`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

Related: [Cuemon.Extensions.AspNetCore.Http.Throttling namespace](/api/extensions/aspnet/Cuemon.Extensions.AspNetCore.Http.Throttling.html) 📘