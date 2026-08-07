using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Cuemon.Diagnostics;
using Codebelt.Extensions.Xunit;
using Cuemon.Reflection;
using Xunit;

namespace Cuemon.Extensions.Text.Json.Formatters;
public class JsonFormatterOptionsTest : Test
{
    public JsonFormatterOptionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void JsonFormatterOptions_SettingsIsNull_ShouldThrowInvalidOperationException()
    {
        var sut1 = new JsonFormatterOptions()
        {
            Settings = null
        };
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'Settings == null')", sut2.Message);
        Assert.StartsWith("JsonFormatterOptions are not in a valid state.", sut3.Message, StringComparison.Ordinal);
        Assert.Equal("sut1", sut3.ParamName);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void JsonFormatterOptions_SupportedMediaTypesIsNull_ShouldThrowInvalidOperationException()
    {
        var sut1 = new JsonFormatterOptions()
        {
            SupportedMediaTypes = null
        };
        var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
        var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

        Assert.Equal("Operation is not valid due to the current state of the object. (Expression 'SupportedMediaTypes == null')", sut2.Message);
        Assert.StartsWith("JsonFormatterOptions are not in a valid state.", sut3.Message, StringComparison.Ordinal);
        Assert.Equal("sut1", sut3.ParamName);
        Assert.IsType<InvalidOperationException>(sut3.InnerException);
    }

    [Fact]
    public void JsonFormatterOptions_ShouldHaveDefaultValues()
    {
        var sut = new JsonFormatterOptions();

        Assert.NotNull(sut.Settings);
        Assert.NotNull(sut.SupportedMediaTypes);
        Assert.Equal(FaultSensitivityDetails.None, sut.SensitivityDetails);
    }

    [Fact]
    public void DefaultConverters_ShouldHaveSameAmountOfDefaultConverters()
    {
        var defaultConverters = new List<JsonConverter>();
        JsonFormatterOptions.DefaultConverters(defaultConverters);

        var x = new JsonFormatterOptions();
        var y = new JsonFormatterOptions();
        var bootstrapInvocationList = JsonFormatterOptions.DefaultConverters.GetInvocationList().Length;

        x.GetType().GetMethod("RefreshWithConverterDependencies", MemberReflection.Everything).Invoke(x, new object[] { });
        y.GetType().GetMethod("RefreshWithConverterDependencies", MemberReflection.Everything).Invoke(y, new object[] { });

        Assert.Equal(5, defaultConverters.Count);
        Assert.Equal(1, bootstrapInvocationList);
        Assert.Equal(2, x.Settings.Converters.Count - defaultConverters.Count);
        Assert.Equal(2, y.Settings.Converters.Count - defaultConverters.Count);

        Assert.Equal(x.Settings.Converters.Count, y.Settings.Converters.Count);

        Assert.Equal(JsonFormatterOptions.DefaultMediaType, x.SupportedMediaTypes.First());
    }
}
