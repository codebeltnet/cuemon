using System;
using Codebelt.Extensions.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cuemon.Extensions.DependencyInjection;
public class TypeForwardServiceOptionsTest : Test
{
    public TypeForwardServiceOptionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void ValidateOptions_ShouldThrowInvalidOperationExceptionForNestedTypePredicate()
    {
        var sut1 = new TypeForwardServiceOptions
        {
            NestedTypePredicate = null
        };

        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'NestedTypePredicate == null')", sut2.Message);
    }

    [Fact]
    public void ValidateOptions_ShouldThrowInvalidOperationExceptionForNestedTypeSelector()
    {
        var sut1 = new TypeForwardServiceOptions
        {
            NestedTypeSelector = null
        };

        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'NestedTypeSelector == null')", sut2.Message);
    }

    [Fact]
    public void ValidateOptions_ShouldHaveDefaultValues()
    {
        var sut = new TypeForwardServiceOptions();

        Assert.NotNull(sut.NestedTypePredicate);
        Assert.NotNull(sut.NestedTypeSelector);
        Assert.True(sut.UseNestedTypeForwarding);
        Assert.Equal(ServiceLifetime.Transient, sut.Lifetime);
    }
}
