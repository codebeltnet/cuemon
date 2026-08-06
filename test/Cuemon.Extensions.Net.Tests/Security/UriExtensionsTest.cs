using System;
using System.Security;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.Net.Security;
public class UriExtensionsTest : Test
{
    private static readonly byte[] Secret = Decorator.Enclose("1234").ToByteArray();

    public UriExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void UriExtensions_ShouldSignAndValidateUris()
    {
        var location = new Uri("https://example.com/search?q=cuemon");
        var signed = location.ToSignedUri(Secret, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow.AddMinutes(1));

        signed.ValidateSignedUri(Secret);

        Assert.NotEqual(location, signed);
        Assert.Throws<ArgumentNullException>(() => UriExtensions.ToSignedUri(null, Secret));
        Assert.Throws<ArgumentNullException>(() => location.ToSignedUri(null));
        Assert.Throws<ArgumentNullException>(() => UriExtensions.ValidateSignedUri(null, Secret));
        Assert.Throws<ArgumentNullException>(() => signed.ValidateSignedUri(null));
        Assert.Throws<SecurityException>(() => new Uri("https://example.com/?q=cuemon").ValidateSignedUri(Secret));
    }
}
