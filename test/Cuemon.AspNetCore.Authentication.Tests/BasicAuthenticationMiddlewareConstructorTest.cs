using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.AspNetCore.Authentication.Basic;

public class BasicAuthenticationMiddlewareConstructorTest : Test
{
    public BasicAuthenticationMiddlewareConstructorTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Constructor_ShouldSupportActionSetup()
    {
        var sut = new BasicAuthenticationMiddleware(_ => Task.CompletedTask, o =>
        {
            o.Realm = "basic-realm";
            o.RequireSecureConnection = false;
        });

        Assert.Equal("basic-realm", sut.Options.Realm);
        Assert.False(sut.Options.RequireSecureConnection);
    }
}
