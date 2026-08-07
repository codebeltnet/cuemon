---
uid: Cuemon.Extensions.Data.DbTypeExtensions
example:
- *content
---

The following example demonstrates converting <see cref="DbType"/> values to their equivalent <see cref="Type"/> using the [ToType](https://docs.cuemon.net/api/extensions/dotnet/Cuemon.Extensions.Data.DbTypeExtensions.html#Cuemon_Extensions_Data_DbTypeExtensions_ToType_System_Data_DbType_) extension method.

```csharp
using System;
using System.Data;
using Cuemon.Extensions.Data;

namespace MyApp.Examples;

public class DbTypeExtensionsExample
{
    public static void Main()
    {
        // Access the extension method via the declaring type explicitly
        var stringType = DbTypeExtensions.ToType(DbType.String);
        var int32Type = DbTypeExtensions.ToType(DbType.Int32);
        var dateTimeType = DbTypeExtensions.ToType(DbType.DateTime);

        Console.WriteLine($"DbType.String   -> {stringType}");    // System.String
        Console.WriteLine($"DbType.Int32    -> {int32Type}");      // System.Int32
        Console.WriteLine($"DbType.DateTime -> {dateTimeType}");   // System.DateTime

        // Equivalent form using extension method syntax on DbType value
        var extended = DbType.Decimal.ToType();
        Console.WriteLine($"DbType.Decimal  -> {extended}");      // System.Decimal

}
}

```
