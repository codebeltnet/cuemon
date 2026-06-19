---
uid: Cuemon.AspNetCore.Http.HttpContextDecoratorExtensions
example:
- *content
---

The following example demonstrates how to use the decorator extensions on `HttpContext` to invoke throttling sentinels, API key sentinels, user-agent sentinels, and write exception descriptor responses.

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
