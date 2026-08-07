using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Text;
public class UriStringOptionsTest : Test
{
    public UriStringOptionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void UriStringOptions_ShouldThrowArgumentNullException_ForSchemes()
    {
        var sut1 = new UriStringOptions()
        {
            Schemes = null
        };

        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'Schemes == null')", sut2.Message);
        Assert.StartsWith("UriStringOptions are not in a valid state.", sut3.Message);
        Assert.Contains("sut1", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void UriStringOptions_ShouldHaveDefaultValues()
    {
        var sut = new UriStringOptions();

        Assert.NotNull(sut.Schemes);
        Assert.Equal(UriKind.Absolute, sut.Kind);
    }
}
