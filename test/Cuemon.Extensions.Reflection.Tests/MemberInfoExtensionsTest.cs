using System;
using System.ComponentModel;
using System.Xml.Serialization;
using Cuemon.Extensions.Reflection.Assets;
using Codebelt.Extensions.Xunit;
using Cuemon.Reflection;
using Xunit;

namespace Cuemon.Extensions.Reflection;
public class MemberInfoExtensionsTest : Test
{
    public MemberInfoExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void HasAttribute_ShouldDetectSpecifiedAttributes()
    {
        var sut1 = new ClassWithAttributeDecorations();
        var sut2 = sut1.GetType();

        Assert.True(sut2.HasAttributes(typeof(CLSCompliantAttribute)), "sut2.HasAttributes(typeof(CLSCompliantAttribute))");
        Assert.False(sut2.HasAttributes(typeof(ObsoleteAttribute)), "sut2.HasAttributes(typeof(ObsoleteAttribute))");

        Assert.True(sut2.GetField("_value", MemberReflection.Everything).HasAttributes(typeof(ContextStaticAttribute)), "sut2.GetField('_value', MemberReflection.Everything).HasAttributes(typeof(ContextStaticAttribute))");
        Assert.False(sut2.GetField("_value", MemberReflection.Everything).HasAttributes(typeof(ObsoleteAttribute)), "sut2.GetField('_value', MemberReflection.Everything).HasAttributes(typeof(ObsoleteAttribute))");

        Assert.True(sut2.GetProperty("Value", MemberReflection.Everything).HasAttributes(typeof(XmlElementAttribute)), "sut2.GetProperty('Value', MemberReflection.Everything).HasAttributes(typeof(XmlElementAttribute))");
        Assert.False(sut2.GetProperty("Value", MemberReflection.Everything).HasAttributes(typeof(ObsoleteAttribute)), "sut2.GetProperty('Value', MemberReflection.Everything).HasAttributes(typeof(ObsoleteAttribute))");

        Assert.True(sut2.GetMethod("Test", MemberReflection.Everything).HasAttributes(typeof(DescriptionAttribute)), "sut2.GetMethod('Test', MemberReflection.Everything).HasAttributes(typeof(TheoryAttribute))");
        Assert.False(sut2.GetMethod("Test", MemberReflection.Everything).HasAttributes(typeof(ObsoleteAttribute)), "sut2.GetMethod('Test', MemberReflection.Everything).HasAttributes(typeof(ObsoleteAttribute))");
    }
}
