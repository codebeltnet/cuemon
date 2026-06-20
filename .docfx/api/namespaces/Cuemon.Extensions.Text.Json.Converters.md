---
uid: Cuemon.Extensions.Text.Json.Converters
summary: *content
---
Register `System.Text.Json` converters for Cuemon-specific types like `TransientFaultException`, `DataPair`, `ExceptionDescriptor`, and string-based enum serialization. Use this namespace when you need JSON serialization support for Cuemon types. Start with `AddStringEnumConverter` for enum serialization or `AddExceptionDescriptorConverterOf<T>` for structured error serialization.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

Complements: [System.Text.Json namespace](https://learn.microsoft.com/en-us/dotnet/api/system.text.json) 🔗

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|ICollection<JsonConverter>|⬇️|`AddTransientFaultExceptionConverter`, `AddDateTimeConverter`, `AddStringEnumConverter`, `AddStringFlagsEnumConverter`, `AddExceptionDescriptorConverterOf<T>`, `AddExceptionConverter`, `AddDataPairConverter`, `AddFailureConverter`, `RemoveAllOf`, `RemoveAllOf<T>`|
