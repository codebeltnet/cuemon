using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.AspNetCore.Mvc;
public class BreadcrumbTest : Test
{
    public BreadcrumbTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Breadcrumb_ShouldExposeAssignedPropertyValues()
    {
        var sut = new Breadcrumb
        {
            Label = "Products",
            ActionName = "Details",
            ControllerName = "Catalog"
        };

        Assert.Equal("Products", sut.Label);
        Assert.Equal("Details", sut.ActionName);
        Assert.Equal("Catalog", sut.ControllerName);
    }
}
