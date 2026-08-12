using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Cuemon.Reflection;
using Xunit;

namespace Cuemon.Data.Assets;
internal static class SqliteDatabase
{
    internal static void Create(DataManager manager, ITestOutputHelper output)
    {

        manager.Execute(new DataStatement("""
                                                      CREATE TABLE Product (
                                                      	ProductID INTEGER PRIMARY KEY,
                                                      	Name TEXT NOT NULL,
                                                      	ProductNumber TEXT NOT NULL,
                                                      	MakeFlag INTEGER NOT NULL,
                                                      	FinishedGoodsFlag INTEGER NOT NULL,
                                                      	Color TEXT NULL,
                                                      	SafetyStockLevel INTEGER NOT NULL,
                                                      	ReorderPoint INTEGER NOT NULL,
                                                      	StandardCost REAL NOT NULL,
                                                      	ListPrice REAL NOT NULL,
                                                      	Size TEXT NULL,
                                                      	SizeUnitMeasureCode TEXT NULL,
                                                      	WeightUnitMeasureCode TEXT NULL,
                                                      	Weight REAL NULL,
                                                      	DaysToManufacture INTEGER NOT NULL,
                                                      	ProductLine TEXT NULL,
                                                      	Class TEXT NULL,
                                                      	Style TEXT NULL,
                                                      	ProductSubcategoryID INTEGER NULL,
                                                      	ProductModelID INTEGER NULL,
                                                      	SellStartDate TEXT NOT NULL,
                                                      	SellEndDate TEXT NULL,
                                                      	DiscontinuedDate TEXT NULL,
                                                      	RowGuid TEXT NOT NULL,
                                                      	ModifiedDate TEXT NOT NULL);
                                                      """));

        using var file = Decorator.Enclose(typeof(DsvDataReaderTest).Assembly).GetManifestResources("AdventureWorks2022_Product.csv", ManifestResourceMatch.ContainsName).Values.Single();
        using var reader = new DsvDataReader(new StreamReader(file), setup: o =>
        {
            o.FormatProvider = CultureInfo.GetCultureInfo("da-DK");
            o.Delimiter = ";";
        });
        while (reader.Read())
        {
            var statement = new DataStatement($$"""
                                                            INSERT INTO Product
                                                            ([ProductID]
                                                            ,[Name]
                                                            ,[ProductNumber]
                                                            ,[MakeFlag]
                                                            ,[FinishedGoodsFlag]
                                                            ,[Color]
                                                            ,[SafetyStockLevel]
                                                            ,[ReorderPoint]
                                                            ,[StandardCost]
                                                            ,[ListPrice]
                                                            ,[Size]
                                                            ,[SizeUnitMeasureCode]
                                                            ,[WeightUnitMeasureCode]
                                                            ,[Weight]
                                                            ,[DaysToManufacture]
                                                            ,[ProductLine]
                                                            ,[Class]
                                                            ,[Style]
                                                            ,[ProductSubcategoryID]
                                                            ,[ProductModelID]
                                                            ,[SellStartDate]
                                                            ,[SellEndDate]
                                                            ,[DiscontinuedDate]
                                                            ,[RowGuid]
                                                            ,[ModifiedDate])
                                                            VALUES
                                                            ({{reader.GetInt32(0)}},
                                                            {{StringOrNull(reader.GetString(1))}},
                                                            {{StringOrNull(reader.GetString(2))}},
                                                            {{reader.GetInt32(3)}},
                                                            {{reader.GetInt32(4)}},
                                                            {{StringOrNull(reader.GetString(5))}},
                                                            {{reader.GetInt32(6)}},
                                                            {{reader.GetInt32(7)}},
                                                            {{reader.GetDecimal(8).ToString("F", CultureInfo.InvariantCulture)}},
                                                            {{reader.GetDecimal(9).ToString("F", CultureInfo.InvariantCulture)}},
                                                            {{StringOrNull(reader.GetString(10))}},
                                                            {{StringOrNull(reader.GetString(11))}},
                                                            {{StringOrNull(reader.GetString(12))}},
                                                            {{(reader.GetString(13) == "NULL" ? "NULL" : reader.GetDecimal(13).ToString("F", CultureInfo.InvariantCulture))}},
                                                            {{reader.GetInt32(14)}},
                                                            {{StringOrNull(reader.GetString(15))}},
                                                            {{StringOrNull(reader.GetString(16))}},
                                                            {{StringOrNull(reader.GetString(17))}},
                                                            {{(reader.GetString(18) == "NULL" ? "NULL" : reader.GetInt32(18))}},
                                                            {{(reader.GetString(19) == "NULL" ? "NULL" : reader.GetInt32(19))}},
                                                            {{StringOrNull(reader.GetDateTime(20).ToString("O", CultureInfo.InvariantCulture))}},
                                                            {{(reader.GetString(21) == "NULL" ? "NULL" : StringOrNull(reader.GetDateTime(21).ToString("O", CultureInfo.InvariantCulture)))}},
                                                            {{(reader.GetString(22) == "NULL" ? "NULL" : StringOrNull(reader.GetDateTime(22).ToString("O", CultureInfo.InvariantCulture)))}},
                                                            {{StringOrNull(reader.GetString(23))}},
                                                            {{StringOrNull(reader.GetDateTime(24).ToString("O", CultureInfo.InvariantCulture))}})
                                                            """);

            try
            {
                manager.Execute(statement);
            }
            catch (Exception e)
            {
                output.WriteLine(statement.Text);
                throw;
            }
        }
    }

    private static string StringOrNull(string value)
    {
        if (value == "NULL") { return "NULL"; }
        if (value.Length >= 2 && value.StartsWith("\"") && value.EndsWith("\"")) { value = value.Substring(1, value.Length - 2); }
        return $"'{value.Replace("'", "''")}'";
    }
}
