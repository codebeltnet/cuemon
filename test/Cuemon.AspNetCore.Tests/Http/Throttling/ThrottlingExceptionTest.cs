using System;
using Cuemon.Extensions.IO;
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions.Text.Json.Formatters;
using Cuemon.Xml.Serialization.Formatters;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Cuemon.AspNetCore.Http.Throttling;
public class ThrottlingExceptionTest : Test
{
    public ThrottlingExceptionTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ThrottlingException_ShouldBeSerializable_Json()
    {
        var reset = DateTime.Today.AddDays(1);
        var sut1 = new ThrottlingException("Throttling rate limit quota violation. Quota limit exceeded.", 100, TimeSpan.FromHours(1), reset);
        var sut2 = new JsonFormatter();
        var sut3 = sut2.Serialize(sut1);
        var sut4 = sut3.ToEncodedString(o => o.LeaveOpen = true);

        TestOutput.WriteLine(sut4);

        var original = sut2.Deserialize<ThrottlingException>(sut3);

        sut3.Dispose();

        Assert.Equal(sut1.StatusCode, original.StatusCode);
        Assert.Equal(sut1.ReasonPhrase, original.ReasonPhrase);
        Assert.Equal(StatusCodes.Status429TooManyRequests, sut1.StatusCode);
        Assert.Equal(sut1.Message, original.Message);
        Assert.Equal(sut1.Delta, original.Delta);
        Assert.Equal(sut1.Reset, original.Reset);
        Assert.Equal(sut1.RateLimit, original.RateLimit);
        Assert.Equal(sut1.ToString(), original.ToString());

        Assert.Equal($$"""
                         {
                           "type": "Cuemon.AspNetCore.Http.Throttling.ThrottlingException",
                           "message": "Throttling rate limit quota violation. Quota limit exceeded.",
                           "rateLimit": 100,
                           "delta": "01:00:00",
                           "reset": "{{reset:O}}",
                           "headers": {},
                           "statusCode": 429,
                           "reasonPhrase": "Too Many Requests"
                         }
                         """.ReplaceLineEndings(), sut4);
    }

    [Fact]
    public void ThrottlingException_ShouldBeSerializable_Xml()
    {
        var reset = DateTime.Today.AddDays(1);
        var sut1 = new ThrottlingException("Throttling rate limit quota violation. Quota limit exceeded.", 100, TimeSpan.FromHours(1), reset);
        var sut2 = new XmlFormatter(o => o.Settings.Writer.Indent = true);
        var sut3 = sut2.Serialize(sut1);
        var sut4 = sut3.ToEncodedString(o => o.LeaveOpen = true);

        TestOutput.WriteLine(sut4);

        var original = sut2.Deserialize<ThrottlingException>(sut3);

        sut3.Dispose();

        Assert.Equal(sut1.StatusCode, original.StatusCode);
        Assert.Equal(sut1.ReasonPhrase, original.ReasonPhrase);
        Assert.Equal(StatusCodes.Status429TooManyRequests, sut1.StatusCode);
        Assert.Equal(sut1.Message, original.Message);
        Assert.Equal(sut1.Delta, original.Delta);
        Assert.Equal(sut1.Reset, original.Reset);
        Assert.Equal(sut1.RateLimit, original.RateLimit);
        Assert.Equal(sut1.ToString(), original.ToString());

        Assert.Equal($$"""
                         <?xml version="1.0" encoding="utf-8"?>
                         <ThrottlingException namespace="Cuemon.AspNetCore.Http.Throttling">
                         	<Message>Throttling rate limit quota violation. Quota limit exceeded.</Message>
                         	<RateLimit>100</RateLimit>
                         	<Delta>01:00:00</Delta>
                         	<Reset>{{reset:O}}</Reset>
                         	<Headers />
                         	<StatusCode>429</StatusCode>
                         	<ReasonPhrase>Too Many Requests</ReasonPhrase>
                         </ThrottlingException>
                         """.ReplaceLineEndings(), sut4);
    }
}
