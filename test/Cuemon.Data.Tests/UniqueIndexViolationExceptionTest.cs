using Cuemon.Extensions.IO;
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions.Text.Json.Formatters;
using Xunit;

namespace Cuemon.Data;
public class UniqueIndexViolationExceptionTest : Test
{
    public UniqueIndexViolationExceptionTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void UniqueIndexViolationException_ShouldBeSerializable_Json()
    {
        var random = Generate.RandomString(10);
        var sut1 = new UniqueIndexViolationException(random);
        var sut2 = new JsonFormatter();
        var sut3 = sut2.Serialize(sut1);
        var sut4 = sut3.ToEncodedString(o => o.LeaveOpen = true);

        TestOutput.WriteLine(sut4);

        var original = sut2.Deserialize<UniqueIndexViolationException>(sut3);

        sut3.Dispose();

        Assert.Equal(sut1.Message, original.Message);
        Assert.Equal(sut1.ToString(), original.ToString());

        Assert.Equal($$"""
                           {
                             "type": "Cuemon.Data.UniqueIndexViolationException",
                             "message": "{{random}}"
                           }
                           """.ReplaceLineEndings(), sut4);
    }
}
