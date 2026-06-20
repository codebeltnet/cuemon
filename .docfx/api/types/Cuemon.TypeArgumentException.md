---
uid: Cuemon.TypeArgumentException
example:
- *content
---

The following example demonstrates how to throw and catch a <see cref="Cuemon.TypeArgumentException"/> when a generic type argument does not satisfy expected constraints at runtime.

```csharp
using System;
using System.IO;
using Cuemon;

namespace Contoso.DependencyInjection;

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
```
