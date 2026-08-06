using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Codebelt.Extensions.Xunit;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Http;
public class HeaderDictionaryExtensionsTest : Test
{
    public HeaderDictionaryExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void AddOrUpdateHeader_ShouldAddHeader()
    {
        var sut = new HeaderDictionary();

        sut.AddOrUpdateHeader("X-Test-Header", "0");

        Assert.Equal("0", sut["X-Test-Header"]);
    }

    [Fact]
    public void AddOrUpdateHeader_ShouldUpdateHeader()
    {
        var sut = new HeaderDictionary();

        sut.Add("X-Test-Header", "0");
        sut.AddOrUpdateHeader("X-Test-Header", "1");

        Assert.Equal("1", sut["X-Test-Header"]);
    }

    [Fact]
    public void AddOrUpdateHeaders_ShouldMakeAnExactCopyOfHeaders()
    {
        var rm = new HttpResponseMessage();
        rm.Headers.Location = new Uri("https://docs.cuemon.net/");
        rm.Headers.Date = DateTimeOffset.UnixEpoch;
        rm.Headers.ETag = EntityTagHeaderValue.Any;

        var sut1 = new HeaderDictionary();
        var sut2 = rm.Headers;

        sut1.AddOrUpdateHeaders(sut2);

        Assert.Equal(sut1.Count, sut2.Count());
        Assert.Equal(sut1["Location"], sut2.Location.OriginalString);
        Assert.Equal(sut1["Date"], sut2.Date.Value.ToString("R"));
        Assert.Equal(sut1["ETag"], sut2.ETag.Tag);
    }
}
