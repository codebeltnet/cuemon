using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cuemon.AspNetCore.Mvc;
public class GoneResultTest : Test
{
    public GoneResultTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task ExecuteResultAsync_ShouldReturnStatusCode303AndLocationUri()
    {
        using (var host = WebHostTestFactory.Create())
        {
            var context = host.Host.Services.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var sut = new GoneResult();
            var ac = new ActionContext(context, new RouteData(), new ActionDescriptor());

            Assert.Equal(StatusCodes.Status410Gone, sut.StatusCode);

            await sut.ExecuteResultAsync(ac);

            Assert.Equal(sut.StatusCode, context.Response.StatusCode);
        }

    }
}
