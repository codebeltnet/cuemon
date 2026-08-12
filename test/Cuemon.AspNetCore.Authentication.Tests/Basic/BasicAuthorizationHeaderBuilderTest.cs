using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.AspNetCore.Authentication.Basic;
public class BasicAuthorizationHeaderBuilderTest : Test
{
    public BasicAuthorizationHeaderBuilderTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Build_ShouldThrowArgumentException_ColonInUserName()
    {
        var bb = new BasicAuthorizationHeaderBuilder()
            .AddUserName("Ag:ent")
            .AddPassword("Test");

        var ae = Assert.Throws<ArgumentException>(() => bb.Build());
        Assert.Contains("Colon is not allowed as part of the", ae.Message);
    }
}
