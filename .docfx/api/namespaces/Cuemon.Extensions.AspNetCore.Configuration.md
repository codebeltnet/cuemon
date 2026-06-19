---
uid: Cuemon.Extensions.AspNetCore.Configuration
summary: *content
---
Enable cache invalidation based on assembly version changes in ASP.NET Core applications through `IServiceCollection` extension methods. Use `AddAssemblyCacheBusting` or `AddDynamicCacheBusting` to register cache-busting services when you need clients to receive fresh static resources after deployment. Start with `AddAssemblyCacheBusting` for the simplest cache invalidation setup.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

Complements: [Cuemon.AspNetCore.Configuration namespace](/api/aspnet/Cuemon.AspNetCore.Configuration.html) 📘

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IServiceCollection|⬇️|`AddAssemblyCacheBusting`, `AddCacheBusting<T>`, `AddDynamicCacheBusting`|
