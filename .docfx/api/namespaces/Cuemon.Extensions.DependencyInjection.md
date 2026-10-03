---
uid: Cuemon.Extensions.DependencyInjection
summary: *content
---
Register service contracts and typed configuration in Microsoft's dependency injection container. Use `Add<TService, TImplementation>` to select a service implementation and lifetime, or `Add<TService, TImplementation, TOptions>` to also register its configuration using the existing first-configuration convention. Choose `AddConfiguredOptions<TOptions>` when consumers need Cuemon Parameter Object conventions within the Microsoft Options lifecycle, together with direct options and configurator injection.

Call `AddConfiguredOptions<TOptions>` when an options type implements Cuemon's `IParameterObject` conventions and needs to participate in the [Microsoft Options pattern](https://learn.microsoft.com/en-us/dotnet/core/extensions/options). Microsoft constructs the options and runs configuration, post-configuration, and validation. Cuemon's `IPostConfigurableParameterObject.PostConfigureOptions()` and `IValidatableParameterObject.ValidateOptions()` participate in the corresponding stages for every options name. Recoverable validation exceptions become `OptionsValidationException` failures when options are materialized; post-configuration exceptions and fatal validation exceptions propagate without translation.

Direct `TOptions` consumption resolves the same cached default instance as `IOptions<TOptions>.Value`, with singleton semantics. Use `IOptionsSnapshot<TOptions>` for scoped snapshots and `IOptionsMonitor<TOptions>` for named options, invalidation, and change notifications; their Microsoft lifecycles remain independent of direct consumption. Post-configurators and validators execute in registration order within their respective stages, so Cuemon's conventions are not guaranteed to run last.

The first `AddConfiguredOptions<TOptions>` call registers the primary default configurator and exposes that exact delegate as `Action<TOptions>`. Later calls for the same type are ignored. Ordinary `Configure<TOptions>` registrations before or after it still compose through Microsoft Options, but do not become part of the injectable delegate. Invoking that delegate against a fresh object applies only the primary configuration, without post-configuration or validation. `TryConfigure<TOptions>` retains its existing first-configuration semantics, and `PostConfigureAllOf<TOptions>` can still add bulk post-configuration for compatible options registrations.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Complements: [Microsoft.Extensions.DependencyInjection namespace](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection?view=dotnet-plat-ext-8.0) 🔗

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IServiceCollection|⬇️|`Add`, `Add<TService>`, `Add<TOptions>`, `Add<TService, TImplementation>`, `Add<TService, TImplementation, TOptions>`, `TryAdd`, `TryAdd<TService>`, `TryAdd<TOptions>`, `TryAdd<TService, TImplementation>`, `TryAdd<TService, TImplementation, TOptions>`, `TryConfigure<TOptions>`, `AddConfiguredOptions<TOptions>`, `PostConfigureAllOf<TOptions>`|
|IServiceProvider|⬇️|`GetServiceDescriptors`|
|type|⬇️|`TryGetDependencyInjectionMarker`|
