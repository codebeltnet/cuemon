using System;
using System.Collections;
using System.Data;
using System.Linq;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Data
{
    public class DataTransferTest : Test
    {
        public DataTransferTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ShouldExposeRowsColumnsAndTypedValues()
        {
            using var reader = CreateDataTable().CreateDataReader();

            var rows = DataTransfer.GetRows(reader);
            var first = rows[0];
            var second = rows[1];
            var columns = first.Columns;
            var enumerable = (IEnumerable)rows;

            Assert.Equal(2, rows.Count);
            Assert.Equal(new[] { "Id", "Name", "Created", "Notes" }, rows.ColumnNames.ToArray());
            Assert.True(rows.Contains(first));
            Assert.Equal(0, rows.IndexOf(first));
            Assert.True(enumerable.GetEnumerator().MoveNext());

            Assert.Equal(1, first.Number);
            Assert.Equal(4, columns.Count);
            Assert.Equal("Id", columns[0].Name);
            Assert.Equal(typeof(int), columns[0].DataType);
            Assert.Equal("Id", columns[0].ToString());
            Assert.Same(columns[0], columns["Id"]);
            Assert.Null(columns["Missing"]);

            Assert.Equal(1, first[(DataTransferColumn)columns[0]]);
            Assert.Equal("Alice", first["Name"]);
            Assert.Null(first[(DataTransferColumn)null]);
            Assert.Null(first["Missing"]);
            Assert.Null(first[-1]);
            Assert.Equal(1, first.As<int>(0));
            Assert.Equal(1, first.As<int>(columns[0]));
            Assert.Equal(new DateTime(2024, 1, 2, 3, 4, 5, DateTimeKind.Utc), first.As<DateTime>("Created"));
            Assert.Throws<TypeArgumentOutOfRangeException>(() => first.As<Guid>(0));
            Assert.Null(second["Notes"]);

            var rowText = first.ToString();
            TestOutput.WriteLine(rowText);
            Assert.Contains("Id=1 [Int32]", rowText);
            Assert.Contains("Name=Alice [String]", rowText);
        }

        [Fact]
        public void ShouldValidateReaderArguments()
        {
            Assert.Throws<ArgumentNullException>(() => DataTransfer.GetRows(null));
            Assert.Throws<ArgumentNullException>(() => DataTransfer.GetColumns(null));

            using var closedReader = CreateDataTable().CreateDataReader();
            closedReader.Close();

            Assert.Throws<ArgumentException>(() => DataTransfer.GetRows(closedReader));
            Assert.Throws<ArgumentException>(() => DataTransfer.GetColumns(closedReader));
        }

        [Fact]
        public void ShouldReturnColumns_WhenReaderHasBeenRead()
        {
            using var reader = CreateDataTable().CreateDataReader();

            Assert.True(reader.Read());

            var columns = DataTransfer.GetColumns(reader);

            Assert.Equal(4, columns.Count);
            Assert.Equal("Created", columns[2].Name);
            Assert.Equal(typeof(DateTime), columns[2].DataType);
        }

        private static DataTable CreateDataTable()
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Created", typeof(DateTime));
            table.Columns.Add("Notes", typeof(string));
            table.Rows.Add(1, "Alice", new DateTime(2024, 1, 2, 3, 4, 5, DateTimeKind.Utc), "First");
            table.Rows.Add(2, "Bob", new DateTime(2024, 2, 3, 4, 5, 6, DateTimeKind.Utc), DBNull.Value);
            return table;
        }
    }
}
