---
uid: Cuemon.ArgumentReservedKeywordException
example:
- *content
---

The following example shows how to throw an `ArgumentReservedKeywordException` when a parameter value matches a reserved SQL keyword. It demonstrates the expected validation failure and how to catch the exception with access to the parameter name.

```csharp
// Program.cs
using System;
using Cuemon;

var reservedKeywords = new[] { "select", "insert", "update", "delete", "from", "where" };
try
{
    var columnName = "select";
    if (Array.Exists(reservedKeywords, keyword => string.Equals(keyword, columnName, StringComparison.OrdinalIgnoreCase)))
    {
        throw new ArgumentReservedKeywordException("sortBy", columnName, "Value must not be a reserved SQL keyword.");
    }
}
catch (ArgumentReservedKeywordException ex)
{
    Console.WriteLine(ex.Message);
    Console.WriteLine(ex.ParamName);
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="380d9162584a0d1f996f0f51"><a href="~/run-actions/380d9162584a0d1f996f0f51/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/380d9162584a0d1f996f0f51/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
