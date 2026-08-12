using Cuemon.Extensions.Reflection.Assets;
using Codebelt.Extensions.Xunit;
using Cuemon.Reflection;
using Xunit;

namespace Cuemon.Extensions.Reflection;
public class PropertyInfoExtensionsTest : Test
{
    public PropertyInfoExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void IsAutoProperty_ShouldDetectIfAPropertyIsAutoOrNot()
    {
        var sut1 = new ClassWithAttributeDecorations();
        var sut2 = sut1.GetType();

        Assert.False(sut2.GetProperty("Value", MemberReflection.Everything).IsAutoProperty(), "sut2.GetProperty('Value', MemberReflection.Everything).IsAutoProperty()");
        Assert.True(sut2.GetProperty("ValueAlternative", MemberReflection.Everything).IsAutoProperty(), "sut2.GetProperty('ValueAlternative', MemberReflection.Everything).IsAutoProperty()");
    }
}
