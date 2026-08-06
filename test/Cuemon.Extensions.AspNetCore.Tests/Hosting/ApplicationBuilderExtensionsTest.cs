using System.Threading.Tasks;
using Cuemon.AspNetCore.Hosting;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Xunit;
using Environments = Cuemon.Extensions.Hosting.Environments;

namespace Cuemon.Extensions.AspNetCore.Hosting;
public class ApplicationBuilderExtensionsTest : Test
{
    public ApplicationBuilderExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task UseHostingEnvironment_ShouldAddHostingEnvironmentMiddleware_WithDefaultHostingEnvironmentOptions()
    {
        HostingEnvironmentOptions sutOptions = null;
        IHeaderDictionary sut = null;
        IHostEnvironment environment = null;
        await WebHostTestFactory.RunAsync(pipelineSetup: app =>
        {
            app.UseHostingEnvironment();
            app.Run(context =>
            {
                sutOptions = app.ApplicationServices.GetRequiredService<IOptions<HostingEnvironmentOptions>>()?.Value;
                environment = app.ApplicationServices.GetRequiredService<IHostEnvironment>();
                sut = context.Response.Headers;
                return Task.CompletedTask;
            });
        }, hostSetup: hb => hb.UseEnvironment(Environments.LocalDevelopment));

        Assert.Contains(sut, pair => pair.Key == sutOptions.HeaderName);
        Assert.Equal(Environments.LocalDevelopment, sut[sutOptions.HeaderName]);
        Assert.False(sutOptions.SuppressHeaderPredicate(environment));
    }
}
