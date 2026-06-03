using System;
using System.ComponentModel;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Data
{
    public class QueryBuilderTest : Test
    {
        public QueryBuilderTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ShouldEncodeFragmentsAndBuildQueryText()
        {
            Assert.Equal("Id,Name", QueryBuilder.EncodeFragment(QueryFormat.Delimited, new[] { "Id", "Name" }));
            Assert.Equal("'Id','Name'", QueryBuilder.EncodeFragment(QueryFormat.DelimitedString, new[] { "Id", "Name" }));
            Assert.Equal("[Id],[Name]", QueryBuilder.EncodeFragment(QueryFormat.DelimitedSquareBracket, new[] { "Id", "Name" }));
            Assert.Equal("Id", QueryBuilder.EncodeFragment(QueryFormat.Delimited, new[] { "Id", "Id" }, true));
            Assert.Throws<ArgumentNullException>(() => QueryBuilder.EncodeFragment(QueryFormat.Delimited, null));
            Assert.Throws<ArgumentException>(() => QueryBuilder.EncodeFragment(QueryFormat.Delimited, Array.Empty<string>()));
            Assert.Throws<InvalidEnumArgumentException>(() => QueryBuilder.EncodeFragment((QueryFormat)999, new[] { "Id" }));

            var defaultBuilder = new DefaultTestQueryBuilder();
            var twoArgumentBuilder = new TwoArgumentTestQueryBuilder("Products");
            var sut = new TestQueryBuilder("Products");
            sut.ReadLimit = 25;
            sut.EnableDirtyReads = true;
            sut.EnableReadLimit = true;
            sut.EnableTableAndColumnEncapsulation = true;
            sut.AppendRaw("SELECT ").AppendFormatted("{0}", "*");

            Assert.Equal(string.Empty, defaultBuilder.GetQuery(QueryType.Select));
            Assert.Equal("Select:Products", twoArgumentBuilder.GetQuery(QueryType.Select));
            Assert.Equal(25, sut.ReadLimit);
            Assert.True(sut.EnableDirtyReads);
            Assert.True(sut.EnableReadLimit);
            Assert.True(sut.EnableTableAndColumnEncapsulation);
            Assert.Equal("SELECT *", sut.ToString());
            Assert.Equal("Select:Products", sut.GetQuery(QueryType.Select));
            Assert.Equal("Update:ArchivedProducts", sut.GetQuery(QueryType.Update, "ArchivedProducts"));
            Assert.Equal("Products", sut.TableName);
            Assert.Single(sut.KeyColumns);
            Assert.Single(sut.Columns);
            Assert.Throws<ArgumentOutOfRangeException>(() => sut.ReadLimit = 0);
            Assert.Equal(0, (int)QueryFormat.Delimited);
            Assert.Equal(4, (int)QueryType.Exists);
        }

        private sealed class DefaultTestQueryBuilder : QueryBuilder
        {
            public override string GetQuery(QueryType queryType, string tableName)
            {
                return ToString();
            }
        }

        private sealed class TwoArgumentTestQueryBuilder : QueryBuilder
        {
            public TwoArgumentTestQueryBuilder(string tableName) : base(tableName, new System.Collections.Generic.Dictionary<string, string>()
            {
                { "Id", "Id" }
            })
            {
            }

            public override string GetQuery(QueryType queryType, string tableName)
            {
                return $"{queryType}:{tableName ?? TableName}";
            }
        }

        private sealed class TestQueryBuilder : QueryBuilder
        {
            public TestQueryBuilder(string tableName) : base(tableName, new System.Collections.Generic.Dictionary<string, string>()
            {
                { "Id", "Id" }
            }, new System.Collections.Generic.Dictionary<string, string>()
            {
                { "Name", "Name" }
            })
            {
            }

            public TestQueryBuilder AppendRaw(string queryFragment)
            {
                Append(queryFragment);
                return this;
            }

            public TestQueryBuilder AppendFormatted(string queryFragment, params object[] args)
            {
                Append(queryFragment, args);
                return this;
            }

            public override string GetQuery(QueryType queryType, string tableName)
            {
                return $"{queryType}:{tableName ?? TableName}";
            }
        }
    }
}
