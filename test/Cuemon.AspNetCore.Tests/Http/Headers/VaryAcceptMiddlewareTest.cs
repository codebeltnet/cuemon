using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
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
                app.UseMiddleware<VaryAcceptMiddleware>();

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
                app.UseMiddleware<VaryAcceptMiddleware>();

                app.Run(context =>
                {
                    nextInvoked = true;
                    return Task.CompletedTask;
                });
            });

            Assert.True(nextInvoked);
        }
    }
}
