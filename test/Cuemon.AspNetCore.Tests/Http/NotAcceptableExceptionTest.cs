using Cuemon.Extensions.IO;
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions.Text.Json.Formatters;
using Cuemon.Xml.Serialization.Formatters;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Cuemon.AspNetCore.Http;
public class NotAcceptableExceptionTest : Test
{
    public NotAcceptableExceptionTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Ctor_ShouldBeSerializableAndHaveCorrectStatusCodeOf406_Json()
    {
        var sut1 = new NotAcceptableException();
        var sut2 = new JsonFormatter();
        var sut3 = sut2.Serialize(sut1);
        var sut4 = sut3.ToEncodedString(o => o.LeaveOpen = true);

        TestOutput.WriteLine(sut4);

        var original = sut2.Deserialize<NotAcceptableException>(sut3);

        sut3.Dispose();

        Assert.Equal(sut1.StatusCode, original.StatusCode);
        Assert.Equal(sut1.ReasonPhrase, original.ReasonPhrase);
        Assert.Equal(sut1.Message, original.Message);
        Assert.Equal(StatusCodes.Status406NotAcceptable, sut1.StatusCode);
        Assert.Equal(sut1.ToString(), original.ToString());

        Assert.Equal("""
                         {
                           "type": "Cuemon.AspNetCore.Http.NotAcceptableException",
                           "message": "The resource identified by the request is only capable of generating response entities which have content characteristics not acceptable according to the accept headers sent in the request.",
                           "headers": {},
                           "statusCode": 406,
                           "reasonPhrase": "Not Acceptable"
                         }
                         """.ReplaceLineEndings(), sut4);
    }

    [Fact]
    public void Ctor_ShouldBeSerializableAndHaveCorrectStatusCodeOf406_Xml()
    {
        var sut1 = new NotAcceptableException();
        var sut2 = new XmlFormatter(o => o.Settings.Writer.Indent = true);
        var sut3 = sut2.Serialize(sut1);
        var sut4 = sut3.ToEncodedString(o => o.LeaveOpen = true);

        TestOutput.WriteLine(sut4);

        var original = sut2.Deserialize<NotAcceptableException>(sut3);

        sut3.Dispose();

        Assert.Equal(sut1.StatusCode, original.StatusCode);
        Assert.Equal(sut1.ReasonPhrase, original.ReasonPhrase);
        Assert.Equal(sut1.Message, original.Message);
        Assert.Equal(StatusCodes.Status406NotAcceptable, sut1.StatusCode);
        Assert.Equal(sut1.ToString(), original.ToString());

        Assert.Equal("""
                         <?xml version="1.0" encoding="utf-8"?>
                         <NotAcceptableException namespace="Cuemon.AspNetCore.Http">
                         	<Message>The resource identified by the request is only capable of generating response entities which have content characteristics not acceptable according to the accept headers sent in the request.</Message>
                         	<Headers />
                         	<StatusCode>406</StatusCode>
                         	<ReasonPhrase>Not Acceptable</ReasonPhrase>
                         </NotAcceptableException>
                         """.ReplaceLineEndings(), sut4);
    }
}
