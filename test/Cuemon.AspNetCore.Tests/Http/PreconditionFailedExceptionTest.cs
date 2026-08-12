using Cuemon.Extensions.IO;
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions.Text.Json.Formatters;
using Cuemon.Xml.Serialization.Formatters;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Cuemon.AspNetCore.Http;
public class PreconditionFailedExceptionTest : Test
{
    public PreconditionFailedExceptionTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Ctor_ShouldBeSerializableAndHaveCorrectStatusCodeOf412_Json()
    {
        var sut1 = new PreconditionFailedException();
        var sut2 = new JsonFormatter();
        var sut3 = sut2.Serialize(sut1);
        var sut4 = sut3.ToEncodedString(o => o.LeaveOpen = true);

        TestOutput.WriteLine(sut4);

        var original = sut2.Deserialize<PreconditionFailedException>(sut3);

        sut3.Dispose();

        Assert.Equal(sut1.StatusCode, original.StatusCode);
        Assert.Equal(sut1.ReasonPhrase, original.ReasonPhrase);
        Assert.Equal(sut1.Message, original.Message);
        Assert.Equal(StatusCodes.Status412PreconditionFailed, sut1.StatusCode);
        Assert.Equal(sut1.ToString(), original.ToString());

        Assert.Equal("""
                         {
                           "type": "Cuemon.AspNetCore.Http.PreconditionFailedException",
                           "message": "The precondition given in one or more of the request-header fields evaluated to false when it was tested on the server.",
                           "headers": {},
                           "statusCode": 412,
                           "reasonPhrase": "Precondition Failed"
                         }
                         """.ReplaceLineEndings(), sut4);
    }

    [Fact]
    public void Ctor_ShouldBeSerializableAndHaveCorrectStatusCodeOf412_Xml()
    {
        var sut1 = new PreconditionFailedException();
        var sut2 = new XmlFormatter(o => o.Settings.Writer.Indent = true);
        var sut3 = sut2.Serialize(sut1);
        var sut4 = sut3.ToEncodedString(o => o.LeaveOpen = true);

        TestOutput.WriteLine(sut4);

        var original = sut2.Deserialize<PreconditionFailedException>(sut3);

        sut3.Dispose();

        Assert.Equal(sut1.StatusCode, original.StatusCode);
        Assert.Equal(sut1.ReasonPhrase, original.ReasonPhrase);
        Assert.Equal(sut1.Message, original.Message);
        Assert.Equal(StatusCodes.Status412PreconditionFailed, sut1.StatusCode);
        Assert.Equal(sut1.ToString(), original.ToString());

        Assert.Equal("""
                         <?xml version="1.0" encoding="utf-8"?>
                         <PreconditionFailedException namespace="Cuemon.AspNetCore.Http">
                         	<Message>The precondition given in one or more of the request-header fields evaluated to false when it was tested on the server.</Message>
                         	<Headers />
                         	<StatusCode>412</StatusCode>
                         	<ReasonPhrase>Precondition Failed</ReasonPhrase>
                         </PreconditionFailedException>
                         """.ReplaceLineEndings(), sut4);
    }
}
