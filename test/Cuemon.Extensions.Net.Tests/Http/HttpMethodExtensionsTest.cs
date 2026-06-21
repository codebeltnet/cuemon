using System;
using System.Net.Http;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.Net.Http
{
    public class HttpMethodExtensionsTest : Test
    {
        public HttpMethodExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void HttpMethodExtensions_ShouldConvertMethods()
        {
            Assert.Equal(Cuemon.Net.Http.HttpMethods.Get, HttpMethod.Get.ToHttpMethod());
            Assert.Equal(Cuemon.Net.Http.HttpMethods.Post, HttpMethod.Post.ToHttpMethod());
            Assert.Equal(Cuemon.Net.Http.HttpMethods.Put, HttpMethod.Put.ToHttpMethod());
            Assert.Equal(Cuemon.Net.Http.HttpMethods.Delete, HttpMethod.Delete.ToHttpMethod());
            Assert.Equal(Cuemon.Net.Http.HttpMethods.Patch, new HttpMethod("PATCH").ToHttpMethod());
            Assert.Throws<ArgumentNullException>(() => HttpMethodExtensions.ToHttpMethod(null));
        }
    }
}
