using System;
using System.ComponentModel.DataAnnotations;
using Cuemon.AspNetCore.Http;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.AspNetCore.Http.Throttling;
using Cuemon.Diagnostics;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.AspNetCore.Diagnostics;
public class FaultDescriptorOptionsTest : Test
{
    public FaultDescriptorOptionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void FaultDescriptorOptions_ShouldThrowArgumentNullExceptionForHttpFaultResolvers()
    {
        var sut1 = new FaultDescriptorOptions
        {
            HttpFaultResolvers = null
        };

        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'HttpFaultResolvers == null')", sut2.Message);
        Assert.Equal("FaultDescriptorOptions are not in a valid state. (Parameter 'sut1')", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void FaultDescriptorOptions_ShouldHaveDefaultValues()
    {
        var sut = new FaultDescriptorOptions();

        Assert.Null(sut.RootHelpLink);
        Assert.False(sut.HasRootHelpLink);
        Assert.False(sut.UseBaseException);
        Assert.Collection(sut.HttpFaultResolvers,
            resolver => Assert.True(resolver.TryResolveFault(new BadRequestException(), out _)),
            resolver => Assert.True(resolver.TryResolveFault(new ConflictException(), out _)),
            resolver => Assert.True(resolver.TryResolveFault(new ForbiddenException(), out _)),
            resolver => Assert.True(resolver.TryResolveFault(new GoneException(), out _)),
            resolver => Assert.True(resolver.TryResolveFault(new NotFoundException(), out _)),
            resolver => Assert.True(resolver.TryResolveFault(new MethodNotAllowedException(), out _)),
            resolver => Assert.True(resolver.TryResolveFault(new NotAcceptableException(), out _)),
            resolver => Assert.True(resolver.TryResolveFault(new PayloadTooLargeException(), out _)),
            resolver => Assert.True(resolver.TryResolveFault(new PreconditionFailedException(), out _)),
            resolver => Assert.True(resolver.TryResolveFault(new PreconditionRequiredException(), out _)),
            resolver => Assert.True(resolver.TryResolveFault(new TooManyRequestsException(), out _)),
            resolver => Assert.True(resolver.TryResolveFault(new UnauthorizedException(), out _)),
            resolver => Assert.True(resolver.TryResolveFault(new UnsupportedMediaTypeException(), out _)),
            resolver => Assert.True(resolver.TryResolveFault(new ApiKeyException(400, "UnitTest"), out _)),
            resolver => Assert.True(resolver.TryResolveFault(new ThrottlingException("UnitTest", 1, TimeSpan.FromMinutes(1), DateTime.Today), out _)),
            resolver => Assert.True(resolver.TryResolveFault(new UserAgentException(400, "UnitTest"), out _)),
            resolver => Assert.True(resolver.TryResolveFault(new ValidationException(), out _)),
            resolver => Assert.True(resolver.TryResolveFault(new FormatException(), out _)),
            resolver => Assert.True(resolver.TryResolveFault(new ArgumentException(), out _)));
        Assert.NotNull(sut.ExceptionDescriptorResolver);
        Assert.Null(sut.ExceptionCallback);
        Assert.NotNull(sut.RequestEvidenceProvider);
        Assert.False(sut.SensitivityDetails.HasFlag(FaultSensitivityDetails.Evidence));
        Assert.False(sut.SensitivityDetails.HasFlag(FaultSensitivityDetails.Failure));
        Assert.False(sut.SensitivityDetails.HasFlag(FaultSensitivityDetails.Data));
        Assert.False(sut.SensitivityDetails.HasFlag(FaultSensitivityDetails.StackTrace));
    }
}
