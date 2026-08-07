using Cuemon.Extensions.IO;
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions.Text.Json.Formatters;
using Cuemon.Xml.Serialization.Formatters;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Cuemon.AspNetCore.Http;
public class ForbiddenExceptionTest : Test
{
    public ForbiddenExceptionTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Ctor_ShouldBeSerializableAndHaveCorrectStatusCodeOf403_Json()
    {
        var sut1 = new ForbiddenException();
        var sut2 = new JsonFormatter();
        var sut3 = sut2.Serialize(sut1);
        var sut4 = sut3.ToEncodedString(o => o.LeaveOpen = true);

        TestOutput.WriteLine(sut4);

        var original = sut2.Deserialize<ForbiddenException>(sut3);

        sut3.Dispose();

        Assert.Equal(sut1.StatusCode, original.StatusCode);
        Assert.Equal(sut1.ReasonPhrase, original.ReasonPhrase);
        Assert.Equal(sut1.Message, original.Message);
        Assert.Equal(StatusCodes.Status403Forbidden, sut1.StatusCode);
        Assert.Equal(sut1.ToString(), original.ToString());

        Assert.Equal("""
                         {
                           "type": "Cuemon.AspNetCore.Http.ForbiddenException",
                           "message": "The server understood the request, but is refusing to fulfill it.",
                           "headers": {},
                           "statusCode": 403,
                           "reasonPhrase": "Forbidden"
                         }
                         """.ReplaceLineEndings(), sut4);
    }

    [Fact]
    public void Ctor_ShouldBeSerializableAndHaveCorrectStatusCodeOf403_Xml()
    {
        var sut1 = new ForbiddenException();
        var sut2 = new XmlFormatter(o => o.Settings.Writer.Indent = true);
        var sut3 = sut2.Serialize(sut1);
        var sut4 = sut3.ToEncodedString(o => o.LeaveOpen = true);

        TestOutput.WriteLine(sut4);

        var original = sut2.Deserialize<ForbiddenException>(sut3);

        sut3.Dispose();

        Assert.Equal(sut1.StatusCode, original.StatusCode);
        Assert.Equal(sut1.ReasonPhrase, original.ReasonPhrase);
        Assert.Equal(sut1.Message, original.Message);
        Assert.Equal(StatusCodes.Status403Forbidden, sut1.StatusCode);
        Assert.Equal(sut1.ToString(), original.ToString());

        Assert.Equal("""
                         <?xml version="1.0" encoding="utf-8"?>
                         <ForbiddenException namespace="Cuemon.AspNetCore.Http">
                         	<Message>The server understood the request, but is refusing to fulfill it.</Message>
                         	<Headers />
                         	<StatusCode>403</StatusCode>
                         	<ReasonPhrase>Forbidden</ReasonPhrase>
                         </ForbiddenException>
                         """.ReplaceLineEndings(), sut4);
    }
}
