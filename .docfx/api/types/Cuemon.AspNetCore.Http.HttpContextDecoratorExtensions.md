---
uid: Cuemon.AspNetCore.Http.HttpContextDecoratorExtensions
example:
- *content
---

`HttpContextDecoratorExtensions` provides extension methods on `Decorator.Enclose` for invoking throttling sentinels, API key sentinels, user-agent sentinels, and writing exception descriptor responses on `HttpContext`. This example configures a `DefaultHttpContext` with `"X-Api-Key: secret-key"` and `"User-Agent: Cuemon Docs"` headers, sets up `ThrottlingSentinelOptions` with a quota of 2 requests per minute, `ApiKeySentinelOptions` with allowed keys, and `UserAgentSentinelOptions`. Key steps include calling `InvokeThrottlerSentinelAsync`, `InvokeApiKeySentinelAsync`, and `InvokeUserAgentSentinelAsync`, then writing a `400 Bad Request` exception descriptor response via `WriteExceptionDescriptorResponseAsync`. Console output confirms the final response status code.

```csharp
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.AspNetCore.Http.Throttling;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using HttpMediaTypeHeaderValue = System.Net.Http.Headers.MediaTypeHeaderValue;

namespace Cuemon.AspNetCore.Http;

public static class HttpContextDecoratorExtensionsExample
{
    public static async Task DemonstrateAsync()
    {
        var context = new DefaultHttpContext();
        context.Request.Headers["X-Api-Key"] = "secret-key";
        context.Request.Headers[HeaderNames.UserAgent] = "Cuemon Docs";

        var throttlingOptions = new ThrottlingSentinelOptions
        {
            ContextResolver = _ => "client-1",
            Quota = new ThrottleQuota(2, TimeSpan.FromMinutes(1))
        };

        await Decorator.Enclose(context).InvokeThrottlerSentinelAsync(new MemoryThrottlingCache(), throttlingOptions);

        var apiKeyOptions = new ApiKeySentinelOptions
        {
            AllowedKeys = new List<string> { "secret-key" }
        };

        await Decorator.Enclose(context).InvokeApiKeySentinelAsync(apiKeyOptions);

        var userAgentOptions = new UserAgentSentinelOptions();
        await Decorator.Enclose(context).InvokeUserAgentSentinelAsync(userAgentOptions);

        var handler = new HttpExceptionDescriptorResponseHandler(
            new HttpMediaTypeHeaderValue("text/plain"),
            exceptionDescriptor => new HttpResponseMessage((HttpStatusCode)exceptionDescriptor.StatusCode)
            {
                Content = new StringContent(exceptionDescriptor.Message)
            });

        var descriptor = new HttpExceptionDescriptor(new InvalidOperationException("Bad request"), StatusCodes.Status400BadRequest);
        await Decorator.Enclose(context).WriteExceptionDescriptorResponseAsync(handler, descriptor, CancellationToken.None);

        Console.WriteLine(context.Response.StatusCode);
    }
}
```
