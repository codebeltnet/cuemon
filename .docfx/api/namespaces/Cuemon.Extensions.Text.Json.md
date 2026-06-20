---
uid: Cuemon.Extensions.Text.Json
summary: *content
---
Register custom JSON converters, configure naming policies, traverse JSON hierarchies, and configure `JsonSerializerOptions` without writing infrastructure code. Use this namespace when you need advanced `System.Text.Json` configuration beyond the defaults. Start with `JsonConverterCollectionExtensions` for registering custom converters, or `ToHierarchy` on `Utf8JsonReader` for JSON tree traversal.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

Complements: [System.Text.Json namespace](https://learn.microsoft.com/en-us/dotnet/api/system.text.json) 🔗

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|JsonNamingPolicy|⬇️|`DefaultOrConvertName`|
|Utf8JsonReader|⬇️|`ToHierarchy`|
|JsonSerializerOptions|⬇️|`SetPropertyName` and `Clone`|
|Utf8JsonWriter|⬇️|`WriteObject`|
