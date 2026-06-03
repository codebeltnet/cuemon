using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Cuemon.Diagnostics;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.Text.Json.Converters
{
    public class JsonConverterCollectionExtensionsCoverageTest : Test
    {
        public JsonConverterCollectionExtensionsCoverageTest(ITestOutputHelper output) : base(output)
        {
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
}
