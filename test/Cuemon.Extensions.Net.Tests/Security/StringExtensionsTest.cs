using System;
using System.Security;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.Net.Security;
public class StringExtensionsTest : Test
{
    private static readonly byte[] Secret = Decorator.Enclose("1234").ToByteArray();

    public StringExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ToSignedUri_ShouldSignUriAndVerifyUri()
    {
        var uriString = "https://www.google.com/search?q=cuemon&rlz=1C1GCEU_enDK858DK858&oq=cuemon&aqs=chrome..69i57j69i59j35i39j69i60l3j69i65l2.3047j0j9&sourceid=chrome&ie=UTF-8";
        var md5Header = "53068c5376dc5a934f1a40b41025148e";
        var signedUri = uriString.ToSignedUri(Secret, DateTime.UtcNow, DateTime.UtcNow.AddDays(1));
        var signedUriWithMd5 = uriString.ToSignedUri(Secret, DateTime.UtcNow, DateTime.UtcNow.AddDays(1), o => o.ContentMd5Header = md5Header);

        TestOutput.WriteLine(signedUri.OriginalString);
        TestOutput.WriteLine(signedUriWithMd5.OriginalString);

        Assert.NotEqual(signedUri, signedUriWithMd5);

        signedUri.ValidateSignedUri(Secret);
        signedUriWithMd5.ValidateSignedUri(Secret, o => o.ContentMd5Header = md5Header);

        Assert.Throws<SecurityException>(() =>
        {
            var su = new UriBuilder(signedUri);
            su.Query = su.Query + "&tampered=1";
            su.Uri.ValidateSignedUri(Secret);
        });

        signedUri = uriString.ToSignedUri(Secret, DateTime.Today.AddDays(1));
        Assert.Throws<SecurityException>(() =>
        {
            signedUri.ValidateSignedUri(Secret);
        });

        signedUri = uriString.ToSignedUri(Secret, expiry: DateTime.Today.AddDays(-1));
        Assert.Throws<SecurityException>(() =>
        {
            signedUri.ValidateSignedUri(Secret);
        });
    }
}
