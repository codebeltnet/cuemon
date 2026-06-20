---
uid: Cuemon.Data
summary: *content
---
Abstract away ADO.NET plumbing with a higher-level data access layer that includes data readers, data managers, statement builders, and data transfer objects. Use this namespace when you need to connect to data sources, execute commands, and manipulate results without writing raw ADO.NET code. Start with `DataManager` as the main entry point for executing commands, and `DataReader` for wrapping `IDataReader`. For building SQL statements programmatically, use `QueryBuilder`.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Complements: [System.Data namespace](https://docs.microsoft.com/en-us/dotnet/api/system.Data) 🔗

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IDecorator<IDataReader>|⬇️|`ToStream`, `ToEncodedString`, `ToEncodedStringAsync`|
|IDecorator<DbType>|⬇️|`ToType`|
