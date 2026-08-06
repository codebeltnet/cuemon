using System.Net.Http.Headers;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Net.Http;
public class MediaTypeHeaderTest : Test
{
    public MediaTypeHeaderTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void VerifyThatMediaTypeHeaderIsTheSameWhenDoingAToString() // time-to-market; test that SupportedMediaTypes will still work
    {
        var sut1 = "application/json";
        var sut2 = new MediaTypeHeaderValue(sut1);

        Assert.Equal(sut1, sut2.ToString());
    }
}
