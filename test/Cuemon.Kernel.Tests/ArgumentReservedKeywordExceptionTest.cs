using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon;
public class ArgumentReservedKeywordExceptionTest : Test
{
    public ArgumentReservedKeywordExceptionTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Ctor_ShouldUseParamName()
    {
        var sut = new ArgumentReservedKeywordException("value");

        Assert.Equal("value", sut.ParamName);
    }

    [Fact]
    public void Ctor_ShouldUseCustomMessage()
    {
        var sut = new ArgumentReservedKeywordException("value", "Keyword was reserved.");

        Assert.Equal("value", sut.ParamName);
        Assert.StartsWith("Keyword was reserved.", sut.Message);
    }

    [Fact]
    public void Ctor_ShouldAssignActualValueAndDefaultMessage()
    {
        var sut = new ArgumentReservedKeywordException("value", "select", null);

        Assert.Equal("value", sut.ParamName);
        Assert.Equal("select", sut.ActualValue);
        Assert.StartsWith("Specified argument is a reserved keyword.", sut.Message);
    }

    [Fact]
    public void Ctor_ShouldAssignInnerException()
    {
        var inner = new InvalidOperationException("boom");
        var sut = new ArgumentReservedKeywordException("Keyword was reserved.", inner);

        Assert.Equal("Keyword was reserved.", sut.Message);
        Assert.Same(inner, sut.InnerException);
    }
}
