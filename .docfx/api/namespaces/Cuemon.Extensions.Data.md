---
uid: Cuemon.Extensions.Data
summary: *content
---
Convert `IDataReader` results into column or row collections, map `DbType` values to CLR types, and embed parameterized `QueryFormat` values safely. Use this namespace when you need streamlined ADO.NET data access without repetitive mapping code. Start with `ToColumns` or `ToRows` on `IDataReader` to consume query results as structured collections.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Complements: [Cuemon.Data namespace](/api/dotnet/Cuemon.Data.html) 📘

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IDataReader|⬇️|`ToColumns`, `ToRows`|
|DbType|⬇️|`ToType`|
|QueryFormat|⬇️|`Embed`|
