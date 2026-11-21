---
uid: Cuemon.TypeArgumentOutOfRangeException
example:
- *content
---

The following example demonstrates how to throw a <see cref="Cuemon.TypeArgumentOutOfRangeException"/> when an enum-based type argument is outside the valid range.

```csharp
// Program.cs
using System;
using Cuemon;
using Contoso.Logging;

TypeArgumentOutOfRangeExceptionExample.Run();

namespace Contoso.Logging
{
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
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="880ff03e977cb10ea4747629"><a href="~/run-actions/880ff03e977cb10ea4747629/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/880ff03e977cb10ea4747629/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
