using System;
using System.Data;
using System.Linq;
using Codebelt.Extensions.Xunit;
using Microsoft.Data.Sqlite;
using Xunit;

namespace Cuemon.Data
{
    public class InOperatorTest : Test
    {
        public InOperatorTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ShouldCreateSafeResultFromExpressions()
        {
            var sut = new TestInOperator(() => "@p");

            var result = sut.ToSafeResult(new[] { 4, 9 }, args => string.Join(";", args));
            var fromParams = sut.ToSafeResult(1, 2);
            var parameters = result.ToParametersArray().Cast<SqliteParameter>().ToArray();

            Assert.Equal("@p", sut.ExposedPrefix);
            Assert.Equal(new[] { "@p0", "@p1" }, result.Arguments.ToArray());
            Assert.Equal("@p0;@p1", result.ToString());
            Assert.Equal(2, parameters.Length);
            Assert.Equal("@p0", parameters[0].ParameterName);
            Assert.Equal(4L, Convert.ToInt64(parameters[0].Value));
            Assert.Equal("@p1", parameters[1].ParameterName);
            Assert.Equal(9L, Convert.ToInt64(parameters[1].Value));
            Assert.Equal("@p0,@p1", fromParams.ToString());
            Assert.Equal(parameters.Length, result.Parameters.Count());
            Assert.Throws<ArgumentNullException>(() => sut.ToSafeResult((System.Collections.Generic.IEnumerable<int>)null));
        }

        private sealed class TestInOperator : InOperator<int>
        {
            public TestInOperator(Func<string> prefixFactory) : base(prefixFactory)
            {
            }

            public string ExposedPrefix => ParameterPrefix;

            protected override IDbDataParameter ParametersSelector(int expression, int index)
            {
                return new SqliteParameter(string.Concat(ParameterPrefix, index), expression);
            }
        }
    }
}
