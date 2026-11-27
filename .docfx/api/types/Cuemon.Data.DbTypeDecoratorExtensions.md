---
uid: Cuemon.Data.DbTypeDecoratorExtensions
example:
- *content
---

The following example demonstrates how to use the `ToType` extension method to resolve the equivalent `System.Type` for a `DbType` value.

```csharp
using System;
using System.Data;
using Cuemon;
using Cuemon.Data;

namespace MyApp.Examples;

public class DbTypeDecoratorExtensionsExample
{
    public static void Main()
    {
        DbType[] types = { DbType.Int32, DbType.String, DbType.DateTime, DbType.Boolean };

        foreach (var dbType in types)
        {
            Type clrType = Decorator.Enclose(dbType).ToType();
            Console.WriteLine("DbType.{0} -> {1}", dbType, clrType);

        // Output:
        // DbType.Int32 -> System.Int32
        // DbType.String -> System.String
        // DbType.DateTime -> System.DateTime
        // DbType.Boolean -> System.Boolean

}}
}

```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="c49d8919a7e1fe62796c9966"><a href="~/run-actions/c49d8919a7e1fe62796c9966/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/c49d8919a7e1fe62796c9966/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
