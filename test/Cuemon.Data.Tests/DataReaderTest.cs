using System;
using System.Collections.Specialized;
using System.Data;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Data
{
    public class DataReaderTest : Test
    {
        public DataReaderTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ShouldExposeIDataReaderMembers()
        {
            var sut = new TestDataReader();

            Assert.True(sut.Read());
            Assert.Equal(1, sut.RowCount);
            Assert.True(sut.Contains("Boolean"));
            Assert.Equal(true, sut["Boolean"]);
            Assert.Equal(true, sut[0]);
            Assert.Equal(12, sut.FieldCount);
            Assert.Equal("Boolean", sut.GetName(0));
            Assert.Equal(string.Empty, sut.GetName(99));
            Assert.Equal(0, sut.GetOrdinal("boolean"));
            Assert.Throws<ArgumentNullException>(() => sut.GetOrdinal(null));
            Assert.Throws<ArgumentOutOfRangeException>(() => sut.GetOrdinal("missing"));
            Assert.True(sut.GetBoolean(0));
            Assert.Equal((byte)8, sut.GetByte(1));
            Assert.Equal('X', sut.GetChar(2));
            Assert.Equal(new DateTime(2024, 5, 6, 7, 8, 9, DateTimeKind.Utc), sut.GetDateTime(3));
            Assert.Equal(10.5m, sut.GetDecimal(4));
            Assert.Equal(12.5d, sut.GetDouble(5));
            Assert.Equal(typeof(Guid), sut.GetFieldType(6));
            Assert.Equal(14.5f, sut.GetFloat(7));
            Assert.Equal(Guid.Parse("11111111-1111-1111-1111-111111111111"), sut.GetGuid(6));
            Assert.Equal((short)16, sut.GetInt16(8));
            Assert.Equal(32, sut.GetInt32(9));
            Assert.Equal(64L, sut.GetInt64(10));
            Assert.Equal("alpha", sut.GetString(11));
            Assert.Equal(true, sut.GetValue(0));
            Assert.False(sut.IsDBNull(0));
            Assert.Equal(0L, sut.GetBytes(0, 0, Array.Empty<byte>(), 0, 0));
            Assert.Equal(0L, ((IDataRecord)sut).GetChars(0, 0, Array.Empty<char>(), 0, 0));
            Assert.Throws<NotSupportedException>(() => ((IDataRecord)sut).GetData(0));
            Assert.Equal(typeof(string).ToString(), ((IDataRecord)sut).GetDataTypeName(0));
            Assert.Equal(0, sut.Depth);
            Assert.Contains("Boolean=True", sut.ToString());
            Assert.Throws<ArgumentNullException>(() => sut.GetValues(null));

            var values = new object[sut.FieldCount];
            Assert.Equal(sut.FieldCount, sut.GetValues(values));
            Assert.Equal("alpha", values[11]);

            var reader = (IDataReader)sut;
            Assert.Equal(-1, reader.RecordsAffected);
            Assert.Null(reader.GetSchemaTable());
            Assert.False(reader.NextResult());
            reader.Close();
            Assert.True(reader.IsClosed);
        }

        private sealed class TestDataReader : DataReader<IOrderedDictionary>
        {
            private readonly IOrderedDictionary[] _rows;
            private int _position = -1;

            protected override void OnDisposeManagedResources()
            {
            }

            public TestDataReader()
            {
                _rows = new IOrderedDictionary[]
                {
                    new OrderedDictionary(StringComparer.OrdinalIgnoreCase)
                    {
                        { "Boolean", true },
                        { "Byte", (byte)8 },
                        { "Char", 'X' },
                        { "DateTime", new DateTime(2024, 5, 6, 7, 8, 9, DateTimeKind.Utc) },
                        { "Decimal", 10.5m },
                        { "Double", 12.5d },
                        { "Guid", Guid.Parse("11111111-1111-1111-1111-111111111111") },
                        { "Single", 14.5f },
                        { "Int16", (short)16 },
                        { "Int32", 32 },
                        { "Int64", 64L },
                        { "String", "alpha" }
                    }
                };
            }

            public override int RowCount { get; protected set; }

            protected override IOrderedDictionary NullRead => null;

            protected override IOrderedDictionary ReadNext(IOrderedDictionary columns)
            {
                return columns;
            }

            public override bool Read()
            {
                _position++;
                if (_position >= _rows.Length) { return false; }
                SetFields(_rows[_position]);
                RowCount++;
                return true;
            }
        }
    }
}
