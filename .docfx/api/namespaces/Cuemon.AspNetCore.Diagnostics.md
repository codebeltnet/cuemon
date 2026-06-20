---
uid: Cuemon.AspNetCore.Diagnostics
summary: *content
---
Handle HTTP errors and communicate request-response metrics in ASP.NET Core with structured exception descriptors, fault resolvers, and Server-Timing support. Use this namespace when you need consistent error responses, server-timing headers, or structured exception handling in your ASP.NET Core pipeline. Start with `HttpExceptionDescriptor` and `HttpFaultResolver` for mapping exceptions to HTTP error responses.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

Complements: [Microsoft.AspNetCore.Diagnostics namespace](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.diagnostics) 🔗

Related:

- [Cuemon.AspNetCore.Mvc.Filters.Diagnostics namespace](/api/aspnet/Cuemon.AspNetCore.Mvc.Filters.Diagnostics.html) 📘
- [Cuemon.Extensions.AspNetCore.Diagnostics namespace](/api/extensions/aspnet/Cuemon.Extensions.AspNetCore.Diagnostics.html) 📘

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|FaultDescriptorOptions|⬇️|`TryResolveHttpExceptionDescriptor`|
|IDecorator<T>|⬇️|`TryResolveHttpExceptionDescriptor<T>`|
|IDecorator<HttpExceptionDescriptor>|⬇️|`ToProblemDetails`|
|HttpExceptionDescriptorResponseHandler|⬇️|`AddResponseHandler`|
|IDecorator<IList<HttpFaultResolver>>|⬇️|`AddHttpFaultResolver<T>`|