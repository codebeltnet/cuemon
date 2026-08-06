using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.Text.Json;
public class DynamicJsonConverterTest : Test
{
    public DynamicJsonConverterTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Create_ShouldSerializeAndDeserializeUsingGenericDelegates()
    {
        var value = Guid.NewGuid();
        var options = new JsonSerializerOptions();
        options.Converters.Add(DynamicJsonConverter.Create<Guid>(
            (writer, guid, serializerOptions) => writer.WriteStringValue(guid.ToString("N")),
            (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions serializerOptions) => Guid.ParseExact(reader.GetString(), "N")));

        var json = JsonSerializer.Serialize(value, options);
        var result = JsonSerializer.Deserialize<Guid>(json, options);

        Assert.Equal($"\"{value:N}\"", json);
        Assert.Equal(value, result);
    }

    [Fact]
    public void Create_ShouldThrowNotImplementedException_WhenWriterDelegateIsNull()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(DynamicJsonConverter.Create<Guid>(reader: (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions serializerOptions) => Guid.Parse(reader.GetString())));

        var exception = Assert.Throws<NotImplementedException>(() => JsonSerializer.Serialize(Guid.Empty, options));

        Assert.Equal("Delegate writer is null.", exception.Message);
    }

    [Fact]
    public void Create_ShouldThrowNotImplementedException_WhenReaderDelegateIsNull()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(DynamicJsonConverter.Create<Guid>((writer, guid, serializerOptions) => writer.WriteStringValue(guid.ToString("D"))));

        var exception = Assert.Throws<NotImplementedException>(() => JsonSerializer.Deserialize<Guid>("\"00000000-0000-0000-0000-000000000000\"", options));

        Assert.Equal("Delegate reader is null.", exception.Message);
    }

    [Fact]
    public void Create_ShouldThrowArgumentNullException_WhenPredicateIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => DynamicJsonConverter.Create<Guid>(null, (writer, guid, serializerOptions) => writer.WriteStringValue(guid.ToString("D"))));

        Assert.Equal("predicate", exception.ParamName);
    }

    [Fact]
    public void Create_ShouldCreateConverterFactory_FromType()
    {
        var options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = null
        };
        options.Converters.Add(DynamicJsonConverter.Create(typeof(DayOfWeek), (typeToConvert, serializerOptions) => new JsonStringEnumConverter().CreateConverter(typeToConvert, serializerOptions)));

        var json = JsonSerializer.Serialize(DayOfWeek.Friday, options);

        Assert.Equal("\"Friday\"", json);
    }

    [Fact]
    public void Create_ShouldThrowArgumentNullException_WhenTypeIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => DynamicJsonConverter.Create((Type)null, (typeToConvert, serializerOptions) => new JsonStringEnumConverter().CreateConverter(typeToConvert, serializerOptions)));

        Assert.Equal("typeToConvert", exception.ParamName);
    }

    [Fact]
    public void Create_ShouldUseFactoryPredicate()
    {
        var sut = DynamicJsonConverter.Create(type => type == typeof(DayOfWeek), (typeToConvert, serializerOptions) => new JsonStringEnumConverter().CreateConverter(typeToConvert, serializerOptions));

        Assert.True(sut.CanConvert(typeof(DayOfWeek)));
        Assert.False(sut.CanConvert(typeof(Guid)));
    }

    [Fact]
    public void Create_ShouldThrowArgumentNullException_WhenFactoryPredicateIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => DynamicJsonConverter.Create((Func<Type, bool>)null, (typeToConvert, serializerOptions) => new JsonStringEnumConverter().CreateConverter(typeToConvert, serializerOptions)));

        Assert.Equal("predicate", exception.ParamName);
    }

    [Fact]
    public void Create_ShouldThrowArgumentNullException_WhenConverterFactoryIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => DynamicJsonConverter.Create(type => type == typeof(Guid), (Func<Type, JsonSerializerOptions, JsonConverter>)null));

        Assert.Equal("converterFactory", exception.ParamName);
    }
}
