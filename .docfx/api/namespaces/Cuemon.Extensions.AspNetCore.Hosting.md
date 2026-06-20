---
uid: Cuemon.Extensions.AspNetCore.Hosting
summary: *content
---
Bridge ASP.NET Core hosting with Cuemon hosting abstractions by making `IWebHostEnvironment` available through a single extension method. Use this namespace when you need to integrate `Cuemon.AspNetCore.Hosting` middleware with the ASP.NET Core hosting environment. Start with `UseHostingEnvironment` on `IApplicationBuilder` to register the hosting middleware.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

Complements: [Cuemon.AspNetCore.Hosting namespace](/api/aspnet/Cuemon.AspNetCore.Hosting.html) 📘

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IApplicationBuilder|⬇️|`UseHostingEnvironment`|
