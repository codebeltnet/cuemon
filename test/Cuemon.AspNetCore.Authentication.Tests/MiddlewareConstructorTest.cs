using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.AspNetCore.Authentication.Basic
{
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
}

namespace Cuemon.AspNetCore.Authentication.Digest
{
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
}

namespace Cuemon.AspNetCore.Authentication.Hmac
{
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
}
