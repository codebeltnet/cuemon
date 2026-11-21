---
uid: Cuemon.TypeArgumentException
example:
- *content
---

The following example demonstrates how to throw and catch a <see cref="Cuemon.TypeArgumentException"/> when a generic type argument does not satisfy expected constraints at runtime.

```csharp
// Program.cs
using System;
using System.IO;
using Cuemon;
using Contoso.DependencyInjection;

TypeArgumentExceptionExample.Run();

namespace Contoso.DependencyInjection
{
    public sealed class TypeArgumentExceptionExample
    {
        public static void Run()
        {
            IDisposable disposable = Create<IDisposable, MemoryStream>();
            disposable.Dispose();

            try
            {
                Create<IDisposable, Widget>();
            }
            catch (TypeArgumentException ex)
            {
                Console.WriteLine($"{ex.ParamName}: {ex.Message}");
            }
        }

        private static TService Create<TService, TImplementation>()
            where TImplementation : class, new()
        {
            if (!typeof(TService).IsAssignableFrom(typeof(TImplementation)))
            {
                throw new TypeArgumentException(
                    nameof(TImplementation),
                    $"{typeof(TImplementation).Name} must implement {typeof(TService).Name}.");
            }

            return (TService)(object)new TImplementation();
        }

        private sealed class Widget
        {
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="f0efccf4e4aded40e2507d37"><a href="~/run-actions/f0efccf4e4aded40e2507d37/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/f0efccf4e4aded40e2507d37/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
