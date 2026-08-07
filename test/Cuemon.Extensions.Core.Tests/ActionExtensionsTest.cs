using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions;
public class ActionExtensionsTest : Test
{
    public ActionExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Configure_ShouldInitializeClassEqually()
    {
        var sut1 = new DelimitedStringOptions();
        var sut2 = new Action<DelimitedStringOptions>(o => { }).Configure();
        var sut3 = new Action<DelimitedStringOptions>(o =>
        {
            o.Qualifier = "x";
            o.Delimiter = "y";
        }).Configure();
        var sut4 = new DelimitedStringOptions()
        {
            Delimiter = "y",
            Qualifier = "x"
        };

        Assert.Equal(sut1.Delimiter, sut2.Delimiter);
        Assert.Equal(sut1.Qualifier, sut2.Qualifier);
        Assert.NotEqual(sut1.Delimiter, sut3.Delimiter);
        Assert.NotEqual(sut1.Qualifier, sut3.Qualifier);
        Assert.Equal(sut3.Delimiter, sut4.Delimiter);
        Assert.Equal(sut3.Qualifier, sut4.Qualifier);
    }

    [Fact]
    public void ConfigureInstance_ShouldInitializeClassEqually()
    {
        var sut1 = new DelimitedStringOptions();
        var sut2 = new Action<DelimitedStringOptions>(o => { }).CreateInstance();
        var sut3 = new Action<DelimitedStringOptions>(o =>
        {
            o.Qualifier = "x";
            o.Delimiter = "y";
        }).CreateInstance();
        var sut4 = new DelimitedStringOptions()
        {
            Delimiter = "y",
            Qualifier = "x"
        };

        Assert.Equal(sut1.Delimiter, sut2.Delimiter);
        Assert.Equal(sut1.Qualifier, sut2.Qualifier);
        Assert.NotEqual(sut1.Delimiter, sut3.Delimiter);
        Assert.NotEqual(sut1.Qualifier, sut3.Qualifier);
        Assert.Equal(sut3.Delimiter, sut4.Delimiter);
        Assert.Equal(sut3.Qualifier, sut4.Qualifier);
    }
}
