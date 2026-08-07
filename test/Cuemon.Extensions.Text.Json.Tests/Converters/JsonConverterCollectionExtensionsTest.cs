using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Cuemon.Diagnostics;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Text.Json.Formatters;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.Text.Json.Converters;
public class JsonConverterCollectionExtensionsTest : Test
{
    public JsonConverterCollectionExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void AddStringEnumConverter_ShouldAddStringEnumConverterToConverterCollection_WithPascalCase()
    {
        var sut1 = DayOfWeek.Friday;
        var sut2 = new JsonFormatterOptions();
        sut2.Settings.PropertyNamingPolicy = null; // set PascalCase
        sut2.Settings.Converters.Clear();
        sut2.Settings.Converters.AddStringEnumConverter();

        Assert.Collection(sut2.Settings.Converters.Where(jc => jc.CanConvert(sut1.GetType())).ToList(), jc =>
        {
            var jf = new JsonFormatter(sut2);

            var result = jf.Serialize(sut1);

            var json = result.ToEncodedString();

            Assert.True(jc.CanConvert(typeof(DayOfWeek)));
            Assert.Equal("\"Friday\"", json);

            TestOutput.WriteLine(json);
        });
    }

    [Fact]
    public void AddStringFlagsEnumConverter_ShouldAddStringFlagsEnumConverterToConverterCollection_WithPascalCase()
    {
        var sut1 = GuidFormats.N | GuidFormats.X;
        var sut2 = new JsonFormatterOptions();
        sut2.Settings.Converters.Clear();
        sut2.Settings.Converters.AddStringFlagsEnumConverter();
        sut2.Settings.PropertyNamingPolicy = null;

        Assert.Collection(sut2.Settings.Converters.Where(jc => jc.CanConvert(sut1.GetType())).ToList(), jc =>
        {
            var jf = new JsonFormatter(sut2);

            var result = jf.Serialize(sut1);

            var json = result.ToEncodedString(o => o.LeaveOpen = true);

            Assert.True(jc.CanConvert(typeof(GuidFormats)));
            Assert.Contains("[", json);
            Assert.Contains("\"N\",", json);
            Assert.Contains("\"X\"", json);
            Assert.Contains("]", json);

            var sut3 = jf.Deserialize<GuidFormats>(result);

            Assert.Equal(sut1, sut3);

            TestOutput.WriteLine(json);
        });
    }

    [Fact]
    public void AddStringEnumConverter_ShouldAddStringEnumConverterToConverterCollection()
    {
        var sut1 = DayOfWeek.Friday;
        var sut2 = new JsonFormatterOptions();
        sut2.Settings.Converters.Clear();
        sut2.Settings.Converters.AddStringEnumConverter();

        Assert.Collection(sut2.Settings.Converters.Where(jc => jc.CanConvert(sut1.GetType())).ToList(), jc =>
        {
            var jf = new JsonFormatter(sut2);

            var result = jf.Serialize(sut1);

            var json = result.ToEncodedString();

            Assert.True(jc.CanConvert(typeof(DayOfWeek)));
            Assert.Equal("\"friday\"", json);

            TestOutput.WriteLine(json);
        });
    }

    [Fact]
    public void AddStringFlagsEnumConverter_ShouldAddStringFlagsEnumConverterToConverterCollection()
    {
        var sut1 = GuidFormats.N | GuidFormats.X;
        var sut2 = new JsonFormatterOptions();
        sut2.Settings.Converters.Clear();
        sut2.Settings.Converters.AddStringFlagsEnumConverter();

        Assert.Collection(sut2.Settings.Converters.Where(jc => jc.CanConvert(sut1.GetType())).ToList(), jc =>
        {
            var jf = new JsonFormatter(sut2);

            var result = jf.Serialize(sut1);

            var json = result.ToEncodedString(o => o.LeaveOpen = true);

            Assert.True(jc.CanConvert(typeof(GuidFormats)));
            Assert.Contains("[", json);
            Assert.Contains("\"n\",", json);
            Assert.Contains("\"x\"", json);
            Assert.Contains("]", json);

            var sut3 = jf.Deserialize<GuidFormats>(result);

            Assert.Equal(sut1, sut3);

            TestOutput.WriteLine(json);
        });
    }

    [Theory]
    [InlineData(FaultSensitivityDetails.All)]
    [InlineData(FaultSensitivityDetails.None)]
    public void AddExceptionDescriptorConverter_ShouldAddExceptionDescriptorConverterToConverterCollection_AndMakeUseOfIncludeOptions(FaultSensitivityDetails sensitivityDetails)
    {
        InsufficientMemoryException ime = null;
        try
        {
            throw new InsufficientMemoryException();
        }
        catch (InsufficientMemoryException e)
        {
            ime = e;
        }

        var sut1 = new ExceptionDescriptor(ime, "NoMemory", "System has exhausted memory.", new Uri("https://docs.microsoft.com/en-us/dotnet/api/system.insufficientmemoryexception"));
        sut1.AddEvidence("CorrelationId", Guid.Empty, correlationId => correlationId.ToString("N"));

        var sut2 = new JsonFormatterOptions()
        {
            SensitivityDetails = sensitivityDetails
        };

        sut2.Settings.Converters.AddExceptionDescriptorConverterOf<ExceptionDescriptor>(o =>
        {
            o.SensitivityDetails = sensitivityDetails;
        });

        Assert.Collection(sut2.Settings.Converters.Where(jc => jc.CanConvert(typeof(ExceptionDescriptor))).ToList(), jc =>
        {
            var jf = new JsonFormatter(sut2);

            var result = jf.Serialize(sut1);

            var json = result.ToEncodedString();

            Assert.True(jc.CanConvert(typeof(ExceptionDescriptor)));
            Assert.Contains("\"error\":", json);
            Assert.Contains("\"code\": \"NoMemory\"", json);
            Assert.Contains("\"message\": \"System has exhausted memory.\"", json);
            Assert.Contains("\"helpLink\": \"https://docs.microsoft.com/en-us/dotnet/api/system.insufficientmemoryexception\"", json);

            Condition.FlipFlop(sensitivityDetails.HasFlag(FaultSensitivityDetails.Failure), () =>
            {
                Assert.Contains("\"failure\":", json);
                Assert.Contains("\"type\": \"System.InsufficientMemoryException\"", json);
                Assert.Contains("\"source\": \"Cuemon.Extensions.Text.Json.Tests\"", json);
                Assert.Contains("\"message\": \"Insufficient memory to continue the execution of the program.\"", json);
            }, () =>
            {
                Assert.DoesNotContain("\"failure\":", json);
                Assert.DoesNotContain("\"type\": \"System.InsufficientMemoryException\"", json);
                Assert.DoesNotContain("\"source\": \"Cuemon.Extensions.Text.Json.Tests\"", json);
                Assert.DoesNotContain("\"message\": \"Insufficient memory to continue the execution of the program.\"", json);
            });

            Condition.FlipFlop(sensitivityDetails.HasFlag(FaultSensitivityDetails.StackTrace), () =>
            {
                Assert.Contains("\"stack\":", json);
                Assert.Contains("\"at Cuemon.Extensions.Text.Json.Converters.JsonConverterCollectionExtensionsTest.AddExceptionDescriptorConverter_ShouldAddExceptionDescriptorConverterToConverterCollection", json);
            }, () =>
            {
                Assert.DoesNotContain("\"stack\":", json);
                Assert.DoesNotContain("\"at Cuemon.Extensions.Text.Json.Converters.JsonConverterCollectionExtensionsTest.AddExceptionDescriptorConverter_ShouldAddExceptionDescriptorConverterToConverterCollection", json);
            });

            Condition.FlipFlop(sensitivityDetails.HasFlag(FaultSensitivityDetails.Evidence), () =>
            {
                Assert.Contains("\"evidence\":", json);
                Assert.Contains("\"correlationId\": \"00000000000000000000000000000000\"", json);
            }, () =>
            {
                Assert.DoesNotContain("\"evidence\":", json);
                Assert.DoesNotContain("\"correlationId\": \"00000000000000000000000000000000\"", json);
            });

            TestOutput.WriteLine(json);
        });
    }

    [Fact]
    public void AddDataPairConverter_ShouldAddDataPairConverterToConverterCollection()
    {
        var sut1 = new DataPair<int>("AnswerToEverything", 42);
        var sut2 = new JsonFormatterOptions();
        sut2.Settings.Converters.Clear();
        sut2.Settings.Converters.AddDataPairConverter();

        Assert.Collection(sut2.Settings.Converters.Where(jc => jc.CanConvert(typeof(DataPair))).ToList(), jc =>
        {
            var jf = new JsonFormatter(sut2);

            var result = jf.Serialize(sut1);

            var json = result.ToEncodedString();

            Assert.True(jc.CanConvert(typeof(DataPair)));
            Assert.Contains("\"name\": \"AnswerToEverything\"", json);
            Assert.Contains("\"value\": 42", json);
            Assert.Contains("\"type\": \"Int32\"", json);

            TestOutput.WriteLine(json);
        });
    }

    [Fact]
    public void RemoveAllOf_ShouldRemoveMatchingConverters_FromGenericType()
    {
        var sut = new List<JsonConverter>()
        {
            new StringEnumConverter(),
            new StringFlagsEnumConverter()
        };

        var result = sut.RemoveAllOf<DayOfWeek>();

        Assert.Same(sut, result);
        Assert.Single(sut);
        Assert.IsType<StringFlagsEnumConverter>(Assert.Single(sut));
    }

    [Fact]
    public void RemoveAllOf_ShouldRemoveMatchingConverters_FromTypeCollection()
    {
        var sut = new List<JsonConverter>()
        {
            new StringEnumConverter(),
            new StringFlagsEnumConverter()
        };

        var result = sut.RemoveAllOf(typeof(DayOfWeek), typeof(GuidFormats));

        Assert.Same(sut, result);
        Assert.Empty(sut);
    }

    [Fact]
    public void RemoveAllOf_ShouldThrowArgumentNullException_WhenConvertersIsNull()
    {
        ICollection<JsonConverter> sut = null;
        var exception = Assert.Throws<ArgumentNullException>(() => sut.RemoveAllOf(typeof(DayOfWeek)));

        Assert.Equal("converters", exception.ParamName);
    }

    [Fact]
    public void RemoveAllOf_ShouldThrowArgumentNullException_WhenTypesIsNull()
    {
        ICollection<JsonConverter> sut = new List<JsonConverter>();
        var exception = Assert.Throws<ArgumentNullException>(() => JsonConverterCollectionExtensions.RemoveAllOf(sut, null));

        Assert.Equal("types", exception.ParamName);
    }

    [Fact]
    public void AddTransientFaultExceptionConverter_ShouldAddConverterToCollection()
    {
        ICollection<JsonConverter> sut = new List<JsonConverter>();

        var result = sut.AddTransientFaultExceptionConverter();

        Assert.Same(sut, result);
        Assert.IsType<TransientFaultExceptionConverter>(Assert.Single(sut));
    }

    [Fact]
    public void AddFailureConverter_ShouldAddConverterToCollection_AndSerializeFailure()
    {
        var options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = null
        };
        options.Converters.AddFailureConverter();
        var sut = new Failure(new InvalidOperationException("Broken"), FaultSensitivityDetails.None);

        var json = JsonSerializer.Serialize(sut, options);

        Assert.Contains("\"Type\":\"System.InvalidOperationException\"", json);
        Assert.Contains("\"Message\":\"Broken\"", json);
    }

    [Fact]
    public void AddExceptionConverter_ShouldAddConfiguredConverterToCollection()
    {
        ICollection<JsonConverter> sut = new List<JsonConverter>();

        var result = sut.AddExceptionConverter(true, true);

        Assert.Same(sut, result);
        var converter = Assert.IsType<ExceptionConverter>(Assert.Single(sut));
        Assert.True(converter.IncludeStackTrace);
        Assert.True(converter.IncludeData);
    }
}
