---
uid: Cuemon.Extensions.AspNetCore.Diagnostics
summary: *content
---
Add Server-Timing headers and fault-descriptor exception handling to your ASP.NET Core pipeline. Use this namespace when you need to emit server-timing metrics or provide structured fault responses. Start with `UseServerTiming` on `IApplicationBuilder` for timing headers, or `UseFaultDescriptorExceptionHandler` for structured exception handling. Register services with `AddServerTiming` or `AddFaultDescriptorOptions` on `IServiceCollection`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

Complements: [Cuemon.AspNetCore.Diagnostics namespace](https://docs.cuemon.net/api/aspnet/Cuemon.AspNetCore.Diagnostics.html) 📘

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IApplicationBuilder|⬇️|`UseServerTiming`, `UseFaultDescriptorExceptionHandler`|
|IServiceCollection|⬇️|`AddServerTiming`, `AddServerTiming<T>`, `AddServerTimingOptions`, `AddFaultDescriptorOptions`, `AddExceptionDescriptorOptions`, `PostConfigureAllExceptionDescriptorOptions`|
|IServiceProvider|⬇️|`GetExceptionResponseFormatters`|
