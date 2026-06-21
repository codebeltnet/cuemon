---
uid: Cuemon.AspNetCore.Builder.MiddlewareBuilderFactory
example:
- *content
---

The following example demonstrates how to register a custom middleware in the application pipeline using `MiddlewareBuilderFactory`.

```csharp
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Builder;
using Cuemon.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace MyApp.Examples;

public class TimingMiddleware : Middleware
{
    public TimingMiddleware(RequestDelegate next) : base(next) { }

    public override async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        await Next(context);
        sw.Stop();
        Console.WriteLine($"{context.Request.Path} took {sw.ElapsedMilliseconds} ms");
    }
}

public static class MiddlewareBuilderFactoryExample
{
    public static void Demonstrate()
    {
        var builder = WebApplication.CreateBuilder().Build();

        MiddlewareBuilderFactory.UseMiddleware<TimingMiddleware>(builder);
    }
}

```
