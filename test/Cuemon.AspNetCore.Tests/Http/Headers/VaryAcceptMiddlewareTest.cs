using System;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Cuemon.Extensions.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Cuemon.AspNetCore.Http.Headers
{
    public class VaryAcceptMiddlewareTest : Test
    {
        public VaryAcceptMiddlewareTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task InvokeAsync_ShouldAddVaryAcceptHeaderToEveryResponse()
        {
            var varyHeaderValue = string.Empty;

            await WebHostTestFactory.RunAsync(pipelineSetup: app =>
            {
                app.UseVaryAccept();

                app.Use(async (context, next) =>
                {
                    await context.Response.WriteAsync("Hello.");
                    await next();
                });

                app.Run(context =>
                {
                    varyHeaderValue = context.Response.Headers[HeaderNames.Vary];
                    TestOutput.WriteLine(varyHeaderValue);
                    return Task.CompletedTask;
                });
            });

            Assert.Equal(HeaderNames.Accept, varyHeaderValue);
        }

        [Fact]
        public async Task InvokeAsync_ShouldDelegateToNextRequestDelegate()
        {
            var nextInvoked = false;

            await WebHostTestFactory.RunAsync(pipelineSetup: app =>
            {
                app.UseVaryAccept();

                app.Run(context =>
                {
                    nextInvoked = true;
                    return Task.CompletedTask;
                });
            });

            Assert.True(nextInvoked);
        }

        [Fact]
        public async Task InvokeAsync_ShouldAppendAcceptToExistingVaryHeaderWithoutDuplication()
        {
            var varyHeaderValue = string.Empty;

            await WebHostTestFactory.RunAsync(pipelineSetup: app =>
            {
                app.Use(async (context, next) =>
                {
                    context.Response.Headers[HeaderNames.Vary] = HeaderNames.AcceptEncoding;
                    await next();
                });

                app.UseVaryAccept();

                app.Run(async context =>
                {
                    await context.Response.WriteAsync("test");
                    varyHeaderValue = context.Response.Headers[HeaderNames.Vary];
                    TestOutput.WriteLine(varyHeaderValue);
                });
            });

            Assert.Contains(HeaderNames.AcceptEncoding, varyHeaderValue);
            Assert.Contains(HeaderNames.Accept, varyHeaderValue);

            var acceptCount = 0;
            foreach (var part in varyHeaderValue.Split(','))
            {
                if (part.Trim().Equals(HeaderNames.Accept, StringComparison.OrdinalIgnoreCase))
                {
                    acceptCount++;
                }
            }
            Assert.Equal(1, acceptCount);
        }

        [Fact]
        public async Task InvokeAsync_ShouldNotDuplicateAcceptWhenAlreadyPresentInVaryHeader()
        {
            var varyHeaderValue = string.Empty;

            await WebHostTestFactory.RunAsync(pipelineSetup: app =>
            {
                app.Use(async (context, next) =>
                {
                    context.Response.Headers[HeaderNames.Vary] = HeaderNames.Accept;
                    await next();
                });

                app.UseVaryAccept();

                app.Run(async context =>
                {
                    await context.Response.WriteAsync("test");
                    varyHeaderValue = context.Response.Headers[HeaderNames.Vary];
                    TestOutput.WriteLine(varyHeaderValue);
                });
            });

            Assert.Equal(HeaderNames.Accept, varyHeaderValue);
        }
    }
}
