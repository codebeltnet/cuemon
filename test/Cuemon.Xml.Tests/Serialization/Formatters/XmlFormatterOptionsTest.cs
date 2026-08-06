using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Diagnostics;
using Codebelt.Extensions.Xunit;
using Cuemon.Reflection;
using Cuemon.Xml.Serialization.Converters;
using Xunit;

namespace Cuemon.Xml.Serialization.Formatters;
public class XmlFormatterOptionsTest : Test
{
    public XmlFormatterOptionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void XmlFormatterOptions_SettingsIsNull_ShouldThrowInvalidOperationException()
    {
        var sut1 = new XmlFormatterOptions()
        {
            Settings = null
        };
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'Settings == null')", sut2.Message);
        Assert.StartsWith("XmlFormatterOptions are not in a valid state.", sut3.Message);
        Assert.Contains("sut1", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void XmlFormatterOptions_SupportedMediaTypesIsNull_ShouldThrowInvalidOperationException()
    {
        var sut1 = new XmlFormatterOptions()
        {
            SupportedMediaTypes = null
        };
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'SupportedMediaTypes == null')", sut2.Message);
        Assert.StartsWith("XmlFormatterOptions are not in a valid state.", sut3.Message);
        Assert.Contains("sut1", sut3.Message);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void XmlFormatterOptions_ShouldHaveDefaultValues()
    {
        var sut = new XmlFormatterOptions();

        Assert.NotNull(sut.Settings);
        Assert.Equal(FaultSensitivityDetails.None, sut.SensitivityDetails);
        Assert.NotNull(sut.SupportedMediaTypes);
        Assert.False(sut.SynchronizeWithXmlConvert);
    }

    [Fact]
    public void DefaultConverters_ShouldHaveSameAmountOfDefaultConverters()
    {
        var defaultConverters = new List<XmlConverter>();
        XmlFormatterOptions.DefaultConverters(defaultConverters);

        var x = new XmlFormatterOptions();
        var y = new XmlFormatterOptions();
        var bootstrapInvocationList = XmlFormatterOptions.DefaultConverters.GetInvocationList().Length;

        x.GetType().GetMethod("RefreshWithConverterDependencies", MemberReflection.Everything).Invoke(x, new object[] { });
        y.GetType().GetMethod("RefreshWithConverterDependencies", MemberReflection.Everything).Invoke(y, new object[] { });

        Assert.Equal(6, defaultConverters.Count);
        Assert.Equal(1, bootstrapInvocationList);
        Assert.Equal(2, x.Settings.Converters.Count - defaultConverters.Count);
        Assert.Equal(2, y.Settings.Converters.Count - defaultConverters.Count);

        Assert.Equal(x.Settings.Converters.Count, y.Settings.Converters.Count);

        Assert.Equal(XmlFormatterOptions.DefaultMediaType, x.SupportedMediaTypes.First());
    }
}
