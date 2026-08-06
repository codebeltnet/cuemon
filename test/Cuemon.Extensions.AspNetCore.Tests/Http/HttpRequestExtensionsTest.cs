using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cuemon.Data.Integrity;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Cuemon.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Http;
public class HttpRequestExtensionsTest : Test
{
    public HttpRequestExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [InlineData("GET")]
    [InlineData("HEAD")]
    [InlineData("get")]
    [InlineData("head")]
    [InlineData("POST")]
    [InlineData("PUT")]
    [InlineData("TRACE")]
    public async Task IsGetOrHeadMethod_ShouldRecognizeGetOrHeadMethod(string method)
    {
        bool sut = false;
        await WebHostTestFactory.RunAsync(pipelineSetup: app =>
        {
            app.Run(context =>
            {
                context.Request.Method = method;
                sut = context.Request.IsGetOrHeadMethod();
                return Task.CompletedTask;
            });
        });
        Condition.FlipFlop(method.Equals("GET", StringComparison.OrdinalIgnoreCase) || method.Equals("HEAD", StringComparison.OrdinalIgnoreCase), () => Assert.True(sut), () => Assert.False(sut));
    }

    [Theory]
    [MemberData(nameof(GetComputedChecksums))]
    public async Task IsClientSideResourceCached_ShouldCompareIfNoneMatchToChecksumBuilder(string checksum)
    {
        var sut = CacheValidatorFactory.CreateValidator(typeof(HttpRequestExtensionsTest).Assembly);
        bool isClientSideResourceCached = false;
        int statusCdoe = 100;
        await WebHostTestFactory.RunAsync(pipelineSetup: app =>
        {
            app.Use(async (context, next) =>
            {
                context.Request.Headers.AddOrUpdateHeader(HeaderNames.IfNoneMatch, $"\"{checksum}\"");
                isClientSideResourceCached = context.Request.IsClientSideResourceCached(sut);
                context.Response.StatusCode = isClientSideResourceCached ? 304 : 200;
                await next();
            });
            app.Run(context =>
            {
                statusCdoe = context.Response.StatusCode;
                return Task.CompletedTask;
            });
        });

        Condition.FlipFlop(checksum.Equals("xxxxxxxxxxxxxxxx"), () =>
        {
            Assert.False(isClientSideResourceCached);
            Assert.Equal(200, statusCdoe);
        }, () =>
        {
            Assert.True(isClientSideResourceCached);
            Assert.Equal(304, statusCdoe);
        });
    }

    public static IEnumerable<object[]> GetComputedChecksums()
    {
        var sut1 = Generate.HashCode64(typeof(HttpRequestExtensionsTest).Assembly.Location);
        var sut2 = HashFactory.CreateFnv64().ComputeHash(Convertible.GetBytes(sut1)).ToHexadecimalString();
        var parameters = new List<object[]>()
        {
            new object[]
            {
                sut2
            },
            new object[]
            {
                "xxxxxxxxxxxxxxxx"
            }
        };

        return parameters;
    }

    [Theory]
    [InlineData("Thu, 01 Jan 1970 00:00:00 GMT")]
    [InlineData("Mon, 29 Mar 2021 00:00:00 GMT")]
    public async Task IsClientSideResourceCached_ShouldCompareIfModifiedSinceToDateTime(string modified)
    {
        var sut = DateTime.Parse("Mon, 29 Mar 2021 00:00:00 GMT").AddDays(-1);
        bool isClientSideResourceCached = false;
        int statusCdoe = 100;
        await WebHostTestFactory.RunAsync(pipelineSetup: app =>
        {
            app.Use(async (context, next) =>
            {
                context.Request.Headers.AddOrUpdateHeader(HeaderNames.IfModifiedSince, $"{modified}");
                isClientSideResourceCached = context.Request.IsClientSideResourceCached(sut);
                context.Response.StatusCode = isClientSideResourceCached ? 304 : 200;
                await next();
            });
            app.Run(context =>
            {
                statusCdoe = context.Response.StatusCode;
                return Task.CompletedTask;
            });
        });

        Condition.FlipFlop(modified.Equals("Thu, 01 Jan 1970 00:00:00 GMT"), () =>
        {
            Assert.False(isClientSideResourceCached);
            Assert.Equal(200, statusCdoe);
        }, () =>
        {
            Assert.True(isClientSideResourceCached);
            Assert.Equal(304, statusCdoe);
        });
    }
}
