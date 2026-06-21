---
uid: Cuemon.Reflection
summary: *content
---
Retrieve assembly metadata, inspect members and parameters, and resolve versioning schemes (traditional and semantic) without verbose reflection boilerplate. The `Cuemon.Reflection` namespace extends `Assembly`, `MemberInfo`, and `MethodInfo` through `IDecorator<T>` extension methods for attribute inspection, type discovery, and version resolution. Use these extensions when you need to check assembly build type, detect custom attributes, or inspect member metadata. Start with `HasAttribute<T>` on `IDecorator<MemberInfo>` for attribute detection, or `GetTypes` on `IDecorator<Assembly>` for type discovery.

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
