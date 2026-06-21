using System.Net;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.Net
{
    public class HttpStatusCodeExtensionsTest : Test
    {
        public HttpStatusCodeExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Theory]
        [InlineData((HttpStatusCode)99, false, false, false, false, false)]
        [InlineData((HttpStatusCode)100, true, false, false, false, false)]
        [InlineData((HttpStatusCode)199, true, false, false, false, false)]
        [InlineData((HttpStatusCode)200, false, true, false, false, false)]
        [InlineData((HttpStatusCode)299, false, true, false, false, false)]
        [InlineData((HttpStatusCode)300, false, false, true, false, false)]
        [InlineData((HttpStatusCode)399, false, false, true, false, false)]
        [InlineData((HttpStatusCode)400, false, false, false, true, false)]
        [InlineData((HttpStatusCode)499, false, false, false, true, false)]
        [InlineData((HttpStatusCode)500, false, false, false, false, true)]
        [InlineData((HttpStatusCode)599, false, false, false, false, true)]
        [InlineData((HttpStatusCode)600, false, false, false, false, false)]
        public void HttpStatusCodeExtensions_ShouldMatchExpectedRanges(HttpStatusCode statusCode, bool information, bool success, bool redirection, bool clientError, bool serverError)
        {
            Assert.Equal(information, statusCode.IsInformationStatusCode());
            Assert.Equal(success, statusCode.IsSuccessStatusCode());
            Assert.Equal(redirection, statusCode.IsRedirectionStatusCode());
            Assert.Equal(clientError, statusCode.IsClientErrorStatusCode());
            Assert.Equal(serverError, statusCode.IsServerErrorStatusCode());
        }
    }
}
