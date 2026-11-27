---
uid: Cuemon.AspNetCore.Http.Headers.RequestIdentifierMiddleware
example:
- *content
---

The following example demonstrates how to execute <xref cref="Cuemon.AspNetCore.Http.Headers.RequestIdentifierMiddleware"/> against a `DefaultHttpContext` and observe the Request-ID stored in `HttpContext.Items` and written to the response header when the response starts.

```csharp
// Program.cs
using System;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;

var responseFeature = new CapturingHttpResponseFeature();
var context = new DefaultHttpContext();
context.Features.Set<IHttpResponseFeature>(responseFeature);

var options = Options.Create(new RequestIdentifierOptions
{
    Token = new RequestToken("req-42")
});

var middleware = new RequestIdentifierMiddleware(
    async httpContext =>
    {
        await responseFeature.FireOnStartingAsync();
        await httpContext.Response.WriteAsync("Hello");
    },
    options);

await middleware.InvokeAsync(context);

Console.WriteLine($"Items[{RequestIdentifierMiddleware.HttpContextItemsKey}]: {context.Items[RequestIdentifierMiddleware.HttpContextItemsKey]}");
Console.WriteLine($"{options.Value.HeaderName}: {context.Response.Headers[options.Value.HeaderName]}");

sealed class CapturingHttpResponseFeature : HttpResponseFeature
{
    private Func<Task>? _onStarting;

    public override void OnStarting(Func<object, Task> callback, object state)
    {
        var prior = _onStarting;
        _onStarting = async () =>
        {
            await callback(state);
            if (prior != null) { await prior(); }
        };
    }

    public Task FireOnStartingAsync() => _onStarting?.Invoke() ?? Task.CompletedTask;
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="75655cd27d52e33acd356228"><a href="~/run-actions/75655cd27d52e33acd356228/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/75655cd27d52e33acd356228/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
