using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon;
public class UnsuccessfulValueTest : Test
{
    public UnsuccessfulValueTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Ctor_SucceededShouldBeFalse()
    {
        var sut = new UnsuccessfulValue();

        Assert.False(sut.Succeeded);
        Assert.Null(sut.Failure);
    }

    [Fact]
    public void Ctor_SucceededShouldBeFalseWithDefaultResult()
    {
        var sut = new UnsuccessfulValue<Guid>();

        Assert.False(sut.Succeeded);
        Assert.Equal(default, sut.Result);
        Assert.Null(sut.Failure);
    }

    [Fact]
    public void Ctor_SucceededShouldBeFalseWithFailure()
    {
        var sut = new UnsuccessfulValue<Guid>(new AccessViolationException());

        Assert.False(sut.Succeeded);
        Assert.Equal(default, sut.Result);
        Assert.IsType<AccessViolationException>(sut.Failure);
    }

    [Fact]
    public void Ctor_SucceededShouldBeFalseWithExpectedResult()
    {
        var value = Guid.NewGuid();
        var sut = new UnsuccessfulValue<Guid>(value);

        Assert.False(sut.Succeeded);
        Assert.Equal(value, sut.Result);
        Assert.Null(sut.Failure);
    }
}
