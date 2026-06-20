---
uid: Cuemon.ArgumentReservedKeywordException
example:
- *content
---

The following example shows how to throw an `ArgumentReservedKeywordException` when a parameter value matches a reserved SQL keyword. It demonstrates the expected validation failure and how to catch the exception with access to the parameter name.

```csharp
using System;
using Cuemon;

namespace MyApp.Validation
{
    public class ReservedKeywordValidator
    {
        private static readonly string[] SqlReservedKeywords = new[]
        {
            "select", "insert", "update", "delete", "from", "where"
        };

        public static void ValidateColumnName(string paramName, string value)
        {
            if (Array.Exists(SqlReservedKeywords,
                    kw => string.Equals(kw, value, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentReservedKeywordException(paramName, value,
                    "Value must not be a reserved SQL keyword.");

        // Usage:
        // try { ValidateColumnName("sortBy", "select"); }
        // catch (ArgumentReservedKeywordException ex) when (ex.ParamName == "sortBy")
        // {
        //     Console.WriteLine($"Validation failed: {ex.Message}");
        // }

}}}
}

```
