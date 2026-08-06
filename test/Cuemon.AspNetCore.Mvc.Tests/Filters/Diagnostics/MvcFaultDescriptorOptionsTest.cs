using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.AspNetCore.Mvc.Filters.Diagnostics;
public class MvcFaultDescriptorOptionsTest : Test
{
    public MvcFaultDescriptorOptionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void MvcFaultDescriptorOptions_ShouldHaveDefaultValues()
    {
        var sut = new MvcFaultDescriptorOptions();

        Assert.False(sut.MarkExceptionHandled);
        Assert.NotNull(sut.HttpFaultResolvers);
        Assert.NotNull(sut.ExceptionDescriptorResolver);
    }

    [Fact]
    public void MvcFaultDescriptorOptions_ShouldAllowMarkExceptionHandledToBeChanged()
    {
        var sut = new MvcFaultDescriptorOptions()
        {
            MarkExceptionHandled = true
        };

        Assert.True(sut.MarkExceptionHandled);
    }
}
