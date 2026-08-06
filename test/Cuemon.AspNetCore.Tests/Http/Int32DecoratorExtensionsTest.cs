using Codebelt.Extensions.Xunit;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Cuemon.AspNetCore.Http;
public class Int32DecoratorExtensionsTest : Test
{
    public Int32DecoratorExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void StatusCodeChecks_ShouldIdentifyMatchingRanges()
    {
        Assert.True(Decorator.Enclose(StatusCodes.Status100Continue).IsInformationStatusCode());
        Assert.True(Decorator.Enclose(StatusCodes.Status200OK).IsSuccessStatusCode());
        Assert.True(Decorator.Enclose(StatusCodes.Status302Found).IsRedirectionStatusCode());
        Assert.True(Decorator.Enclose(StatusCodes.Status304NotModified).IsNotModifiedStatusCode());
        Assert.True(Decorator.Enclose(StatusCodes.Status404NotFound).IsClientErrorStatusCode());
        Assert.True(Decorator.Enclose(StatusCodes.Status500InternalServerError).IsServerErrorStatusCode());
    }

    [Fact]
    public void StatusCodeChecks_ShouldReturnFalseOutsideMatchingRanges()
    {
        Assert.False(Decorator.Enclose(StatusCodes.Status200OK).IsInformationStatusCode());
        Assert.False(Decorator.Enclose(StatusCodes.Status302Found).IsSuccessStatusCode());
        Assert.False(Decorator.Enclose(StatusCodes.Status404NotFound).IsRedirectionStatusCode());
        Assert.False(Decorator.Enclose(StatusCodes.Status200OK).IsNotModifiedStatusCode());
        Assert.False(Decorator.Enclose(StatusCodes.Status500InternalServerError).IsClientErrorStatusCode());
        Assert.False(Decorator.Enclose(StatusCodes.Status404NotFound).IsServerErrorStatusCode());
    }
}
