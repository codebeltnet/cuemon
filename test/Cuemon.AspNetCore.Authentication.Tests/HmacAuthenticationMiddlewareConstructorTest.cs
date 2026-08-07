using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.AspNetCore.Authentication.Hmac;

public class HmacAuthenticationMiddlewareConstructorTest : Test
{
    public HmacAuthenticationMiddlewareConstructorTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Constructor_ShouldSupportActionSetup()
    {
        var sut = new HmacAuthenticationMiddleware(_ => Task.CompletedTask, o =>
        {
            o.AuthenticationScheme = "hmac-test";
            o.RequireSecureConnection = false;
        });

        Assert.Equal("hmac-test", sut.Options.AuthenticationScheme);
        Assert.False(sut.Options.RequireSecureConnection);
    }
}
