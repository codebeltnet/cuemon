---
uid: Cuemon.Extensions.Reflection
summary: *content
---
Discover types, fields, properties, events, methods, and embedded resources at runtime, check auto-property status, and retrieve assembly version information. Use this namespace when you need advanced reflection without boilerplate. Start with `GetDerivedTypes` or `GetAllProperties` on `Type` for type discovery, or `GetAssemblyVersion` on `Assembly` for version information.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Complements: [Cuemon.Reflection namespace](/api/dotnet/Cuemon.Reflection.html) 📘

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|Assembly|⬇️|`GetAssemblyVersion`, `GetFileVersion`, `GetProductVersion`, `IsDebugBuild`|
|MemberInfo|⬇️|`HasAttributes`|
|PropertyInfo|⬇️|`IsAutoProperty`|
|Type|⬇️|`GetAllProperties`, `GetAllFields`, `GetAllEvents`, `GetAllMethods`, `GetDerivedTypes`, `GetInheritedTypes`, `GetHierarchyTypes`, `GetEmbeddedResources`, `GetRuntimePropertiesExceptOf<T>`, `ToFullNameIncludingAssemblyName`|
