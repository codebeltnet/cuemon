using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Resilience;

public class LatencyExceptionTest : Test
{
    public LatencyExceptionTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Ctor_ShouldCreateInstance_WhenCalledWithoutParameters()
    {
        var sut = new LatencyException();

        Assert.NotNull(sut);
        Assert.Null(sut.InnerException);
        Assert.False(string.IsNullOrWhiteSpace(sut.Message));
    }

    [Fact]
    public void Ctor_ShouldSetMessage_WhenCalledWithMessage()
    {
        var message = Guid.NewGuid().ToString();

        var sut = new LatencyException(message);

        Assert.Equal(message, sut.Message);
        Assert.Null(sut.InnerException);
    }

    [Fact]
    public void Ctor_ShouldSetMessageAndInnerException_WhenCalledWithMessageAndInnerException()
    {
        var message = Guid.NewGuid().ToString();
        var innerException = new InvalidOperationException();

        var sut = new LatencyException(message, innerException);

        Assert.Equal(message, sut.Message);
        Assert.Same(innerException, sut.InnerException);
    }
}
