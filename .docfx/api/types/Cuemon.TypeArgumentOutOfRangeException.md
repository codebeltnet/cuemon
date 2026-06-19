---
uid: Cuemon.TypeArgumentOutOfRangeException
example:
- *content
---

The following example demonstrates how to throw a <see cref="Cuemon.TypeArgumentOutOfRangeException"/> when an enum-based type argument is outside the valid range.

```csharp
using System;
using Cuemon;

namespace Contoso.Logging;

public sealed class TypeArgumentOutOfRangeExceptionExample
{
    public static void Run()
    {
        LogLevel parsed = ParseEnum<LogLevel>("Warning");
        Console.WriteLine(parsed);

        try
        {
            ParseEnum<int>("1");
        }
        catch (TypeArgumentOutOfRangeException ex)
        {
            Console.WriteLine($"{ex.ParamName}: {ex.ActualValue}");
        }
    }

    private static TEnum ParseEnum<TEnum>(string text)
        where TEnum : struct
    {
        if (!typeof(TEnum).IsEnum)
        {
            throw new TypeArgumentOutOfRangeException(
                nameof(TEnum),
                typeof(TEnum),
                "Type arguments must be enum types.");
        }

        return (TEnum)Enum.Parse(typeof(TEnum), text, ignoreCase: true);
    }

    private enum LogLevel
    {
        Debug,
        Information,
        Warning,
        Error
    }
}
```
