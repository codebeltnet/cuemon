---
uid: Cuemon.Extensions.Hosting
summary: *content
---
Detect local development or non-production environments in your application startup code without relying on the built-in development, staging, and production checks. Use this namespace when you need environment-detection beyond the standard ASP.NET Core checks. Start with `IsLocalDevelopment()` on `IHostEnvironment` for detecting local machines, or `IsNonProduction()` for checking non-production environments.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Complements: [Microsoft.Extensions.Hosting namespace](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting?view=dotnet-plat-ext-8.0) 🔗

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IHostBuilder|⬇️|`ConfigureConfigurationSources`, `RemoveConfigurationSource`|
|IHostEnvironment|⬇️|`IsLocalDevelopment`, `IsNonProduction`|
|IHostingEnvironment|⬇️|`IsLocalDevelopment`, `IsNonProduction`|
