---
uid: Cuemon.Extensions.DependencyInjection
summary: *content
---
Register services in the Microsoft DI container with or without options, specifying service and implementation types through a rich set of generic extension methods. Use this namespace when you need flexible DI registration with typed options. Start with `Add<TService, TImplementation>` for basic registration, or `Add<TService, TImplementation, TOptions>` when your service requires configuration options.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Complements: [Microsoft.Extensions.DependencyInjection namespace](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection?view=dotnet-plat-ext-8.0) 🔗

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IServiceCollection|⬇️|`Add`, `Add<TService>`, `Add<TOptions>`, `Add<TService, TImplementation>`, `Add<TService, TImplementation, TOptions>`, `TryAdd`, `TryAdd<TService>`, `TryAdd<TOptions>`, `TryAdd<TService, TImplementation>`, `TryAdd<TService, TImplementation, TOptions>`, `TryConfigure<TOptions>`, `PostConfigureAllOf<TOptions>`|
|IServiceProvider|⬇️|`GetServiceDescriptors`|
|type|⬇️|`TryGetDependencyInjectionMarker`|
