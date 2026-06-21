using System.IO;
using System.Text;
using Codebelt.Extensions.Xunit;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Cuemon.AspNetCore.Diagnostics
{
    public class HttpRequestEvidenceTest : Test
    {
        public HttpRequestEvidenceTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Ctor_ShouldCaptureRequestDetails_ForRegularFormRequests()
        {
            var context = new DefaultHttpContext();
            context.Request.Scheme = "https";
            context.Request.Host = new HostString("example.test");
            context.Request.Path = "/submit";
            context.Request.QueryString = new QueryString("?page=1");
            context.Request.Method = HttpMethods.Post;
            context.Request.ContentType = "application/x-www-form-urlencoded";
            context.Request.Headers["X-Test"] = "true";
            context.Request.Headers["Cookie"] = "session=abc";
            context.Request.Form = new FormCollection(new System.Collections.Generic.Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
            {
                { "name", "cuemon" }
            });
            context.Items[HttpRequestEvidence.HttpContextItemsKeyForCapturedRequestBody] = new MemoryStream(Encoding.UTF8.GetBytes("payload"));

            var sut = new HttpRequestEvidence(context.Request);

            Assert.Equal("https://example.test/submit?page=1", sut.Location);
            Assert.Equal(HttpMethods.Post, sut.Method);
            Assert.Equal("true", sut.Headers["X-Test"]);
            Assert.Equal("1", sut.Query["page"]);
            Assert.Equal("cuemon", sut.Form["name"]);
            Assert.Equal("abc", sut.Cookies["session"]);
            Assert.Equal("payload", sut.Body);
        }

        [Fact]
        public void Ctor_ShouldSuppressFormAndBody_ForMultipartRequests()
        {
            var context = new DefaultHttpContext();
            context.Request.Scheme = "https";
            context.Request.Host = new HostString("example.test");
            context.Request.Path = "/upload";
            context.Request.Method = HttpMethods.Post;
            context.Request.ContentType = "multipart/form-data; boundary=abc123";
            context.Request.Form = new FormCollection(new System.Collections.Generic.Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
            {
                { "name", "cuemon" }
            });
            context.Items[HttpRequestEvidence.HttpContextItemsKeyForCapturedRequestBody] = new MemoryStream(Encoding.UTF8.GetBytes("payload"));

            var sut = new HttpRequestEvidence(context.Request);

            Assert.Equal("https://example.test/upload", sut.Location);
            Assert.Null(sut.Form);
            Assert.Null(sut.Body);
        }
    }
}
