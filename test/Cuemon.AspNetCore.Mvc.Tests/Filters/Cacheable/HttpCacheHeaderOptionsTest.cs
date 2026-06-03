using System;
using System.IO;
using System.Linq;
using System.Text;
using Cuemon.Data.Integrity;
using Cuemon.Security;
using Codebelt.Extensions.Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Cuemon.AspNetCore.Mvc.Filters.Cacheable
{
    public class HttpEntityTagHeaderOptionsTest : Test
    {
        public HttpEntityTagHeaderOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void HttpEntityTagHeaderOptions_ShouldHaveDefaultValues()
        {
            var sut = new HttpEntityTagHeaderOptions();

            Assert.False(sut.UseEntityTagResponseParser);
            Assert.True(sut.HasEntityTagProvider);
            Assert.True(sut.HasEntityTagResponseParser);
        }

        [Fact]
        public void EntityTagProvider_ShouldAddEntityTagHeader()
        {
            var sut = new HttpEntityTagHeaderOptions();
            var context = new DefaultHttpContext();
            var integrity = new FakeEntityDataIntegrity(new byte[] { 1, 2, 3 }, EntityDataIntegrityValidation.Strong);

            context.Request.Method = HttpMethods.Get;

            sut.EntityTagProvider(integrity, context);

            Assert.True(context.Response.Headers.ContainsKey(HeaderNames.ETag));
            Assert.False(string.IsNullOrWhiteSpace(context.Response.Headers[HeaderNames.ETag].ToString()));
        }

        [Fact]
        public void EntityTagResponseParser_ShouldAddEntityTagHeader()
        {
            var sut = new HttpEntityTagHeaderOptions();
            var context = new DefaultHttpContext();
            using (var body = new MemoryStream(Encoding.UTF8.GetBytes("payload")))
            {
                context.Request.Method = HttpMethods.Get;

                sut.EntityTagResponseParser(body, context.Request, context.Response);

                Assert.True(context.Response.Headers.ContainsKey(HeaderNames.ETag));
                Assert.False(string.IsNullOrWhiteSpace(context.Response.Headers[HeaderNames.ETag].ToString()));
            }
        }
    }

    public class HttpLastModifiedHeaderOptionsTest : Test
    {
        public HttpLastModifiedHeaderOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void HttpLastModifiedHeaderOptions_ShouldHaveDefaultValues()
        {
            var sut = new HttpLastModifiedHeaderOptions();

            Assert.True(sut.HasLastModifiedProvider);
        }

        [Fact]
        public void LastModifiedProvider_ShouldAddLastModifiedHeader()
        {
            var sut = new HttpLastModifiedHeaderOptions();
            var context = new DefaultHttpContext();
            var timestamp = new FakeEntityDataTimestamp(DateTime.Parse("2024-01-01T00:00:00Z"), DateTime.Parse("2024-01-02T00:00:00Z"));

            context.Request.Method = HttpMethods.Get;

            sut.LastModifiedProvider(timestamp, context);

            Assert.True(context.Response.Headers.ContainsKey(HeaderNames.LastModified));
            Assert.Contains("Tue, 02 Jan 2024", context.Response.Headers[HeaderNames.LastModified].ToString());
        }
    }

    internal sealed class FakeEntityDataIntegrity : IEntityDataIntegrity
    {
        public FakeEntityDataIntegrity(byte[] checksum, EntityDataIntegrityValidation validation)
        {
            Checksum = new HashResult(checksum);
            Validation = validation;
        }

        public EntityDataIntegrityValidation Validation { get; }

        public HashResult Checksum { get; }
    }

    internal sealed class FakeEntityDataTimestamp : IEntityDataTimestamp
    {
        public FakeEntityDataTimestamp(DateTime created, DateTime? modified)
        {
            Created = created;
            Modified = modified;
        }

        public DateTime Created { get; }

        public DateTime? Modified { get; }
    }
}
