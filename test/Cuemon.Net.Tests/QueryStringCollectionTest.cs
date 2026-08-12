using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Net;
public class QueryStringCollectionTest : Test
{
    public QueryStringCollectionTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Class_ShouldHaveKeysAndValuesRulesetAsQueryString()
    {
        var query = new QueryStringCollection("?a=1&b=2&c=3&a=2");
        var queryString = query.ToString();


        Assert.Contains("a=1", queryString);
        Assert.Contains("a=2", queryString);
        Assert.Contains("b=2", queryString);
        Assert.Contains("c=3", queryString);

        TestOutput.WriteLine(query.ToString());

    }
}
