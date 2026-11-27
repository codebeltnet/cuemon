---
uid: Cuemon.Data.SqlClient.SqlDataManager
example:
- *content
---

The following example demonstrates how to configure `SqlDataManager` with a Microsoft SQL Server connection string, inspect its transient fault handling callback, and clone the manager instance.

```csharp
// Program.cs
using System;
using Cuemon.Data.SqlClient;

var manager = new SqlDataManager(o =>
{
    o.ConnectionString = "Server=.;Database=AdventureWorks;Trusted_Connection=True;TrustServerCertificate=True;";
});

var cloned = (SqlDataManager)manager.Clone();

Console.WriteLine($"Connection string: {manager.Options.ConnectionString}");
Console.WriteLine($"Transient fault callback configured: {manager.TransientFaultHandlingOptionsCallback is not null}");
Console.WriteLine($"Cloned connection string matches: {cloned.Options.ConnectionString == manager.Options.ConnectionString}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="a25d8ef50be5b6e478d9908f"><a href="~/run-actions/a25d8ef50be5b6e478d9908f/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/a25d8ef50be5b6e478d9908f/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
