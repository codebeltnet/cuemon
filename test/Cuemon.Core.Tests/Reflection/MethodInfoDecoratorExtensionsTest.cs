using Cuemon.Assets;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Reflection;
public class MethodInfoDecoratorExtensionsTest : Test
{
    public MethodInfoDecoratorExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void IsOverridden_ShouldBeTrueWhenClassHasOverridenMethod()
    {
        var b = new ClassBase();
        var bmi = b.GetType().GetMethod("GetSomeNumber");
        var d = new ClassDerived();
        var dmi = d.GetType().GetMethod("GetSomeNumber");

        Assert.Equal(int.MinValue, b.GetSomeNumber());
        Assert.False(Decorator.Enclose(bmi).IsOverridden());

        Assert.Equal(int.MaxValue, d.GetSomeNumber());
        Assert.True(Decorator.Enclose(dmi).IsOverridden());
    }
}
