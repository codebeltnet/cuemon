using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.AspNetCore.Http.Headers;
public class UserAgentSentinelOptionsTest : Test
{
    public UserAgentSentinelOptionsTest()
    {
    }

    [Fact]
    public void UserAgentSentinelOptions_ResponseHandlerIsNull_ShouldThrowInvalidOperationException()
    {
        var sut1 = new UserAgentSentinelOptions
        {
            ResponseHandler = null
        };
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'ResponseHandler == null')", sut2.Message);
        Assert.Equal("UserAgentSentinelOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void UserAgentSentinelOptions_AllowedUserAgentsIsNull_ShouldThrowInvalidOperationException()
    {
        var sut1 = new UserAgentSentinelOptions
        {
            AllowedUserAgents = null
        };
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'AllowedUserAgents == null')", sut2.Message);
        Assert.Equal("UserAgentSentinelOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void UserAgentSentinelOptions_ShouldHaveDefaultValues()
    {
        var sut = new UserAgentSentinelOptions();

        Assert.NotNull(sut.AllowedUserAgents);
        Assert.Equal("The requirements of the request was not met.", sut.BadRequestMessage);
        Assert.Equal("The User-Agent specified was rejected.", sut.ForbiddenMessage);
        Assert.NotNull(sut.ResponseHandler);
        Assert.False(sut.RequireUserAgentHeader);
        Assert.False(sut.ValidateUserAgentHeader);
        Assert.False(sut.UseGenericResponse);
    }
}
