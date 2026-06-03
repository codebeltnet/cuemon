using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.Text.Json
{
    public class JsonSerializerOptionsExtensionsTest : Test
    {
        public JsonSerializerOptionsExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Clone_ShouldCopySettings_AndApplySetup()
        {
            var sut = new JsonSerializerOptions()
            {
                AllowTrailingCommas = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };
            sut.Converters.Add(new JsonStringEnumConverter());

            var clone = sut.Clone(o =>
            {
                o.WriteIndented = true;
                o.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

            Assert.NotSame(sut, clone);
            Assert.True(clone.AllowTrailingCommas);
            Assert.True(clone.WriteIndented);
            Assert.Equal(JsonIgnoreCondition.WhenWritingNull, clone.DefaultIgnoreCondition);
            Assert.Equal(sut.PropertyNamingPolicy, clone.PropertyNamingPolicy);
            Assert.Single(clone.Converters);

            clone.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

            Assert.Single(sut.Converters);
            Assert.Equal(2, clone.Converters.Count);
        }

        [Fact]
        public void Clone_ShouldThrowArgumentNullException_WhenOptionsIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => JsonSerializerOptionsExtensions.Clone(null));

            Assert.Equal("options", exception.ParamName);
        }

        [Theory]
        [InlineData(true, "pascalCase")]
        [InlineData(false, "PascalCase")]
        public void SetPropertyName_ShouldHonorNamingPolicy(bool useCamelCase, string expected)
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = useCamelCase ? JsonNamingPolicy.CamelCase : null
            };

            var result = options.SetPropertyName("PascalCase");

            Assert.Equal(expected, result);
        }
    }
}
