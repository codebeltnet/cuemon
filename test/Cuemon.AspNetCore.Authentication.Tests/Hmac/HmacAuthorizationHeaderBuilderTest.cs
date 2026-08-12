using System;
using System.Globalization;
using Cuemon.Collections.Generic;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting.AspNetCore;
using Cuemon.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cuemon.AspNetCore.Authentication.Hmac;
public class HmacAuthorizationHeaderBuilderTest : Test
{
    public HmacAuthorizationHeaderBuilderTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Build_ShouldGenerateValidAuthorizationHeader()
    {
        using var mw = WebHostTestFactory.Create();
        var context = mw.Host.Services.GetRequiredService<IHttpContextAccessor>().HttpContext;

        var timestamp = DateTime.Parse("2022-07-10T12:50:42.2737531Z", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);

        context.Request.Headers.Add(HttpHeaderNames.Host, "api.cuemon.net");
        context.Request.Headers.Add(HttpHeaderNames.Date, timestamp.ToString("R"));

        var hb = new HmacAuthorizationHeaderBuilder()
            .AddFromRequest(context.Request)
            .AddClientId("AKIAIOSFODNN7EXAMPLE")
            .AddClientSecret("wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY")
            .AddCredentialScope("some-limiting-scope");

        var h = hb.Build();

        TestOutput.WriteLine("-- HEADER --");
        TestOutput.WriteLine("");
        TestOutput.WriteLine(h.ToString());
        TestOutput.WriteLine("");
        TestOutput.WriteLine("-- BUILDER --");
        TestOutput.WriteLine("");
        TestOutput.WriteLine(hb.ToString());
        TestOutput.WriteLine("");

        Assert.Equal(h, HmacAuthorizationHeader.Create(HmacFields.Scheme, h.ToString()), DynamicEqualityComparer.Create<AuthorizationHeader>(header => Generate.HashCode32(header.ToString()), (h1, h2) => h1.ToString() == h2.ToString()));
        Assert.Equal(@"HMAC Credential=AKIAIOSFODNN7EXAMPLE/some-limiting-scope, SignedHeaders=host;date, Signature=ae1fa2ff4e715d92fd91f2d2027a587377662d0c40fa47a7b9155d5aa6b0e308", h.ToString());
        Assert.Equal(@"httpRequestMethod=GET

canonicalUri=/

canonicalQueryString=

canonicalHeaders=date:Sun, 10 Jul 2022 12:50:42 GMT
host:api.cuemon.net


requestPayload=e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855

serverDateTime=2022-07-10T12:50:42.0000000Z

clientId=AKIAIOSFODNN7EXAMPLE

clientSecret=wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY

credentialScope=some-limiting-scope

signedHeaders=date;host

canonicalRequest=GET
/

date:Sun, 10 Jul 2022 12:50:42 GMT
host:api.cuemon.net

date;host
e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855

stringToSign=Sha256
2022-07-10T12:50:42.0000000Z
some-limiting-scope
429a1ad3362fa07ed327865abc6462ea6c622422c8b249e2ef04d4d0a9ddb56f", hb.ToString(), ignoreLineEndingDifferences: true);
    }
}
