---
uid: Cuemon.AspNetCore.Http.Headers.VaryAcceptMiddleware
example:
- *content
---

The following example demonstrates how to invoke <xref cref="Cuemon.AspNetCore.Http.Headers.VaryAcceptMiddleware"/> against a `DefaultHttpContext` and observe the `Vary: Accept` header written when the response starts.

```csharp
// Program.cs
using System;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Net.Http.Headers;

var responseFeature = new CapturingHttpResponseFeature();
var context = new DefaultHttpContext();
context.Features.Set<IHttpResponseFeature>(responseFeature);

var middleware = new VaryAcceptMiddleware(async httpContext =>
{
    await responseFeature.FireOnStartingAsync();
    await httpContext.Response.WriteAsync("Hello");
});

await middleware.InvokeAsync(context);

Console.WriteLine($"{HeaderNames.Vary}: {context.Response.Headers[HeaderNames.Vary]}");

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
<div class="docfx-run-actions" data-example-id="4d1bcdd3ff9f3bc9ae67c6f1"><a href="~/run-actions/4d1bcdd3ff9f3bc9ae67c6f1/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/4d1bcdd3ff9f3bc9ae67c6f1/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
