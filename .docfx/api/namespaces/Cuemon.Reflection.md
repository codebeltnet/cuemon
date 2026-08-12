---
uid: Cuemon.Reflection
summary: *content
---
Retrieve assembly metadata, inspect members and parameters, resolve versioning schemes (traditional and semantic), and map assemblies to target framework monikers without verbose reflection boilerplate. The `Cuemon.Reflection` namespace extends `Assembly`, `MemberInfo`, and `MethodInfo` through `IDecorator<T>` extension methods for attribute inspection, type discovery, and version resolution, and it includes `TargetFrameworkMoniker` for resolving short TFMs such as `net10.0` or `netstandard2.0`. Use these APIs when you need to check assembly build type, detect custom attributes, inspect member metadata, or determine which target framework an assembly was built for. Start with `HasAttribute<T>` on `IDecorator<MemberInfo>` for attribute detection, `GetTypes` on `IDecorator<Assembly>` for type discovery, or `TargetFrameworkMoniker.ResolveCurrent` for current-process TFM resolution.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Complements: [System.Reflection namespace](https://docs.microsoft.com/en-us/dotnet/api/system.reflection) 🔗

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IDecorator<Assembly>|⬇️|`GetTypes`, `IsDebugBuild`, `GetAssemblyVersion`, `GetFileVersion`, `GetProductVersion`, `GetManifestResources`|
|IDecorator<MemberInfo>|⬇️|`HasAttribute<T>`, `HasAttribute`|
|IDecorator<MethodInfo>|⬇️|`IsOverridden`|
|IDecorator<PropertyInfo>|⬇️|`IsOverridden`, `IsAutoProperty`|
|IDecorator<Stack<IList<MemberArgument>>>|⬇️|`CreateException`|
