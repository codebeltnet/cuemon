using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.AspNetCore.Authentication.Digest;

public class DigestAuthenticationMiddlewareConstructorTest : Test
{
    public DigestAuthenticationMiddlewareConstructorTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Constructor_ShouldSupportActionSetup()
    {
        var sut = new DigestAuthenticationMiddleware(_ => Task.CompletedTask, o =>
        {
            o.Realm = "digest-realm";
            o.RequireSecureConnection = false;
        });

        Assert.Equal("digest-realm", sut.Options.Realm);
        Assert.False(sut.Options.RequireSecureConnection);
    }
}
