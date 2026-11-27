---
uid: Cuemon.Extensions.DependencyInjection.TypeExtensions
example:
- *content
---

The following example asks `TryGetDependencyInjectionMarker` whether a generic service type carries an `IDependencyInjectionMarker<TMarker>` contract and then reads the discovered marker type. It tests a `DefaultService<OrdersChannel>` that implements the marker interface, and a plain `string` type that does not. The boolean results and the resolved marker type are written to the console, demonstrating how to detect marker-interface contracts for DI service wiring at runtime.

```csharp
// Program.cs
using System;
using Cuemon.Extensions.DependencyInjection;
using Cuemon.Docs.Samples.DependencyInjection;

TypeExtensionsExample.Demonstrate();

namespace Cuemon.Docs.Samples.DependencyInjection
{
    public static class TypeExtensionsExample
    {
        public static void Demonstrate()
        {
            var marked = typeof(DefaultService<OrdersChannel>).TryGetDependencyInjectionMarker(out var markerType);
            var plain = typeof(string).TryGetDependencyInjectionMarker(out _);

            Console.WriteLine(marked);
            Console.WriteLine(markerType == typeof(OrdersChannel));
            Console.WriteLine(plain);
        }

        public sealed class OrdersChannel
        {
        }

        public interface IMessageService
        {
        }

        public interface IMessageService<TMarker> : IMessageService, IDependencyInjectionMarker<TMarker>
        {
        }

        public sealed class DefaultService<TMarker> : IMessageService<TMarker>
        {
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="9a2dbc05b2688f2d6cc9d3f6"><a href="~/run-actions/9a2dbc05b2688f2d6cc9d3f6/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/9a2dbc05b2688f2d6cc9d3f6/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
