using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.Text.Json.Converters;
public class StringFlagsEnumConverterTest : Test
{
    public StringFlagsEnumConverterTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Write_ShouldWriteNullArray_WhenValueIsNull()
    {
        var sut = new StringFlagsEnumConverter();
        var options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = null
        };
        var converter = sut.CreateConverter(typeof(GuidFormats), options);

        var json = InvokeWrite(converter, null, options);

        Assert.Equal("[null]", json);
    }

    [Fact]
    public void Write_ShouldWriteNothing_WhenEnumDoesNotHaveFlagsAttribute()
    {
        var sut = new StringFlagsEnumConverter();
        var options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = null
        };
        var converter = sut.CreateConverter(typeof(DayOfWeek), options);

        var json = InvokeWrite(converter, DayOfWeek.Friday, options);

        Assert.False(sut.CanConvert(typeof(DayOfWeek)));
        Assert.Equal(string.Empty, json);
    }

    private static string InvokeWrite(JsonConverter converter, Enum value, JsonSerializerOptions options)
    {
        using (var stream = new MemoryStream())
        {
            using (var writer = new Utf8JsonWriter(stream))
            {
                var method = converter.GetType().GetMethod("Write", BindingFlags.Instance | BindingFlags.Public);
                method.Invoke(converter, new object[] { writer, value, options });
                writer.Flush();
            }
            return Encoding.UTF8.GetString(stream.ToArray());
        }
    }
}
