using System;
using Codebelt.Extensions.Xunit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Authentication;
public class AuthorizationResponseHandlerOptionsTest : Test
{
    public AuthorizationResponseHandlerOptionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ValidateOptions_ShouldThrowInvalidOperationException_WhenFallbackResponseHandlerIsNull()
    {
        var options = new AuthorizationResponseHandlerOptions
        {
            FallbackResponseHandler = null
        };

        Assert.Throws<InvalidOperationException>(() => options.ValidateOptions());
    }

    [Fact]
    public void ValidateOptions_ShouldThrowInvalidOperationException_WhenAuthorizationFailureHandlerIsNull()
    {
        var options = new AuthorizationResponseHandlerOptions
        {
            AuthorizationFailureHandler = null
        };

        Assert.Throws<InvalidOperationException>(() => options.ValidateOptions());
    }

    [Fact]
    public void ValidateOptions_ShouldNotThrow_WhenAllRequiredPropertiesAreSet()
    {
        var options = new AuthorizationResponseHandlerOptions();
        var ex = Record.Exception(() => options.ValidateOptions());
        Assert.Null(ex);
    }

    [Fact]
    public void AuthorizationFailureHandler_ShouldReturnForbiddenException_WhenFailureIsNull()
    {
        var options = new AuthorizationResponseHandlerOptions();
        var result = options.AuthorizationFailureHandler(null);
        Assert.NotNull(result);
        Assert.IsAssignableFrom<Exception>(result);
    }

    [Fact]
    public void AuthorizationFailureHandler_ShouldReturnForbiddenException_WhenFailureHasFailureReasonWithMessage()
    {
        var options = new AuthorizationResponseHandlerOptions();
        var failure = AuthorizationFailure.Failed(new[] { new AuthorizationFailureReason(null, "Access denied due to policy.") });
        var result = options.AuthorizationFailureHandler(failure);
        Assert.NotNull(result);
        Assert.Contains("Access denied due to policy.", result.Message);
    }

    [Fact]
    public void AddInMemoryDigestAuthenticationNonceTracker_ShouldThrowArgumentNullException_WhenServicesIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddInMemoryDigestAuthenticationNonceTracker(null));
    }

    [Fact]
    public void AddAuthorizationResponseHandler_ShouldThrowArgumentNullException_WhenServicesIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddAuthorizationResponseHandler(null));
    }
}
