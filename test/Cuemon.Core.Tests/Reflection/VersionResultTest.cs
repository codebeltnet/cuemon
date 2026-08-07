using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Reflection;
public class VersionResultTest : Test
{
    public VersionResultTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Ctor_AlphanumericVersion_ShouldSelectAlphanumericVersionStringPath_AlthoughNoAlphaCharactersArePresent()
    {
        var sut1 = new VersionResult("6.0.0.08702");

        Assert.True(sut1.HasAlphanumericVersion);
        Assert.False(sut1.IsSemanticVersion());
        Assert.Equal(sut1.ToVersion(), Version.Parse("6.0.0.08702"));
        Assert.Equal("6.0.0.08702", sut1.ToString());
    }

    [Fact]
    public void Ctor_AlphanumericVersion_ShouldSelectVersionPath()
    {
        var sut1 = new VersionResult("6.0.0.0");

        Assert.False(sut1.HasAlphanumericVersion);
        Assert.False(sut1.IsSemanticVersion());
        Assert.Equal(sut1.ToVersion(), Version.Parse("6.0.0.0"));
        Assert.Equal("6.0.0.0", sut1.ToString());
    }

    [Fact]
    public void Ctor_AlphanumericVersion_ShouldSelectAlphanumericVersionStringPath()
    {
        var sut1 = new VersionResult("6.0.rc.0-alpha-beta-gamma");

        Assert.True(sut1.HasAlphanumericVersion);
        Assert.True(sut1.IsSemanticVersion());
        Assert.Equal(sut1.ToVersion(), Version.Parse("6.0"));
        Assert.Equal("6.0.rc.0-alpha-beta-gamma", sut1.ToString());
    }

    [Fact]
    public void Ctor_AlphanumericVersion_ShouldNotBeVersionCompliant()
    {
        var sut1 = new VersionResult("6.xyz.rc.0-alpha-beta-gamma");

        Assert.True(sut1.HasAlphanumericVersion);
        Assert.True(sut1.IsSemanticVersion());
        Assert.Throws<InvalidOperationException>(() => sut1.ToVersion());
        Assert.Equal("6.xyz.rc.0-alpha-beta-gamma", sut1.ToString());
    }

    [Fact]
    public void Ctor_AlphanumericVersion_ShouldSelectAlphanumericVersionStringPath_ButBeingVersionCompatible()
    {
        var sut1 = new VersionResult("6.0.0.0.rc-alpha-beta-gamma.x.y.z");

        Assert.True(sut1.HasAlphanumericVersion);
        Assert.True(sut1.IsSemanticVersion());
        Assert.Equal(sut1.ToVersion(), Version.Parse("6.0.0.0"));
        Assert.Equal("6.0.0.0.rc-alpha-beta-gamma.x.y.z", sut1.ToString());
    }
}
