using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Cuemon.AspNetCore.Mvc.Filters.Cacheable
{
    public class HttpEntityTagHeaderFilterTest : Test
    {
        public HttpEntityTagHeaderFilterTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task OnResultExecutionAsync_ShouldUseResponseParserAndRestoreCacheableObjectValue()
        {
            var actionContext = new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor());
            var payload = CacheableFactory.Create("payload", o =>
            {
                o.TimestampProvider = _ => DateTime.UtcNow;
                o.ChecksumProvider = _ => Encoding.UTF8.GetBytes("payload");
            });
            var result = new ObjectResult(payload);
            var originalValue = result.Value;
            var sut = new HttpEntityTagHeaderFilter(o =>
            {
                o.EntityTagProvider = null;
                o.UseEntityTagResponseParser = true;
            });
            var context = new ResultExecutingContext(actionContext, new List<IFilterMetadata>(), result, null);
            var responseBody = new MemoryStream();

            context.HttpContext.Request.Method = HttpMethods.Get;
            context.HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            context.HttpContext.Response.Body = responseBody;

            await sut.OnResultExecutionAsync(context, async () =>
            {
                Assert.Equal("payload", result.Value);
                await using (var writer = new StreamWriter(context.HttpContext.Response.Body, Encoding.UTF8, 1024, true))
                {
                    await writer.WriteAsync("body");
                    await writer.FlushAsync();
                }
                return new ResultExecutedContext(actionContext, new List<IFilterMetadata>(), result, null);
            });

            Assert.Same(originalValue, result.Value);
            Assert.True(context.HttpContext.Response.Headers.ContainsKey(HeaderNames.ETag));
            responseBody.Position = 0;
            using (var reader = new StreamReader(responseBody, Encoding.UTF8, true, 1024, true))
            {
                Assert.Equal("body", reader.ReadToEnd());
            }
        }

        [Fact]
        public async Task OnResultExecutionAsync_ShouldPreserveStatusCode304WhenUsingResponseParser()
        {
            var actionContext = new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor());
            var result = new ObjectResult("payload");
            var sut = new HttpEntityTagHeaderFilter(o =>
            {
                o.EntityTagProvider = null;
                o.UseEntityTagResponseParser = true;
            });
            var context = new ResultExecutingContext(actionContext, new List<IFilterMetadata>(), result, null);
            var responseBody = new MemoryStream();

            context.HttpContext.Request.Method = HttpMethods.Get;
            context.HttpContext.Response.StatusCode = StatusCodes.Status304NotModified;
            context.HttpContext.Response.Body = responseBody;

            await sut.OnResultExecutionAsync(context, async () =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status200OK;
                await using (var writer = new StreamWriter(context.HttpContext.Response.Body, Encoding.UTF8, 1024, true))
                {
                    await writer.WriteAsync("ignored");
                    await writer.FlushAsync();
                }
                return new ResultExecutedContext(actionContext, new List<IFilterMetadata>(), result, null);
            });

            Assert.Equal(StatusCodes.Status304NotModified, context.HttpContext.Response.StatusCode);
            Assert.True(context.HttpContext.Response.Headers.ContainsKey(HeaderNames.ETag));
            Assert.Equal(0, responseBody.Length);
        }
    }
}
