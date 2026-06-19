---
uid: Cuemon.Patterns
example:
- *content
---

The following example demonstrates how to use the <see cref="Patterns"/> class to safely invoke delegates, configure options via the Options pattern, and guard against fatal exceptions.

```csharp
using System;
using System.Threading;
using Cuemon;
using Cuemon.Threading;

namespace Contoso.Infrastructure;

public sealed class PatternsExample
{
    public static void Run()
    {
        var options = Patterns.Configure<AsyncOptions>(setup =>
        {
            setup.CancellationToken = CancellationToken.None;
        });

        var profile = Patterns.CreateInstance<EndpointProfile>(instance =>
        {
            instance.Name = "health";
        });

        bool wroteMessage = Patterns.TryInvoke(() => Console.WriteLine(profile.Name));
        int fallbackPort = Patterns.InvokeOrDefault(() => int.Parse("not-a-number"), -1);
        bool recoverable = Patterns.IsRecoverableException(new InvalidOperationException("Transient."));

        Console.WriteLine($"Token can cancel: {options.CancellationToken.CanBeCanceled}");
        Console.WriteLine($"TryInvoke succeeded: {wroteMessage}");
        Console.WriteLine($"Fallback port: {fallbackPort}");
        Console.WriteLine($"Recoverable: {recoverable}");
    }

    private sealed class EndpointProfile
    {
        public string Name { get; set; }
    }
}
```
