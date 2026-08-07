using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon;
public class TypeArgumentExceptionTest : Test
{
    public TypeArgumentExceptionTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Ctor_ShouldUseDefaultMessageAndParamName()
    {
        var sut = new TypeArgumentException("TValue");

        Assert.Equal("TValue", sut.ParamName);
        Assert.StartsWith("Value does not fall within the expected range.", sut.Message);
    }

    [Fact]
    public void Ctor_ShouldUseCustomMessageAndParamName()
    {
        var sut = new TypeArgumentException("TValue", "Invalid type argument.");

        Assert.Equal("TValue", sut.ParamName);
        Assert.StartsWith("Invalid type argument.", sut.Message);
    }

    [Fact]
    public void Ctor_ShouldAssignInnerException()
    {
        var inner = new InvalidOperationException("boom");
        var sut = new TypeArgumentException("Invalid type argument.", inner);

        Assert.Equal("Invalid type argument.", sut.Message);
        Assert.Same(inner, sut.InnerException);
    }
}
