---
uid: Cuemon.Data.SqlClient.SqlDataManager
example:
- *content
---

The following example demonstrates how to use `SqlDataManager` to execute commands against Microsoft SQL Server.

```csharp
// Program.cs
using System;
using Cuemon.Data.SqlClient;

var manager = new SqlDataManager(o =>
{
    o.ConnectionString = "Server=.;Database=AdventureWorks;Trusted_Connection=True;TrustServerCertificate=True;";
});

Console.WriteLine("SqlDataManager configured for the AdventureWorks database.");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="10f0fcc57e29c9d0b098b708"><a href="~/run-actions/10f0fcc57e29c9d0b098b708/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/10f0fcc57e29c9d0b098b708/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
