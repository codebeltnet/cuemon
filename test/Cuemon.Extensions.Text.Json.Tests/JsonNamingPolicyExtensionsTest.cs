using System.Text.Json;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.Text.Json;
public class JsonNamingPolicyExtensionsTest : Test
{
    public JsonNamingPolicyExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void DefaultOrConvertName_ShouldReturnOriginalName_WhenPolicyIsNull()
    {
        JsonNamingPolicy sut = null;

        var result = sut.DefaultOrConvertName("PascalCase");

        Assert.Equal("PascalCase", result);
    }
}
