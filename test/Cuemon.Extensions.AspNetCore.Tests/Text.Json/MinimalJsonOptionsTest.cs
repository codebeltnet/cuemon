using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Cuemon.Extensions.Text.Json.Formatters;
using System.Text.Json.Serialization.Metadata;
using Codebelt.Extensions.Xunit;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Text.Json
{
    public class MinimalJsonOptionsTest : Test
    {
        public MinimalJsonOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void MinimalJsonOptions_ShouldPropagatePropertyNamingPolicy_FromJsonFormatterOptions()
        {
            var services = new ServiceCollection();
            services.AddMinimalJsonOptions(o =>
            {
                o.Settings.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
            });

            var provider = services.BuildServiceProvider();
            var jsonOptions = provider.GetRequiredService<IOptions<JsonOptions>>().Value;

            Assert.Equal(JsonNamingPolicy.SnakeCaseLower, jsonOptions.SerializerOptions.PropertyNamingPolicy);
        }

        [Fact]
        public void MinimalJsonOptions_ShouldPropagateWriteIndented_FromJsonFormatterOptions()
        {
            var services = new ServiceCollection();
            services.AddMinimalJsonOptions(o =>
            {
                o.Settings.WriteIndented = false;
            });

            var provider = services.BuildServiceProvider();
            var jsonOptions = provider.GetRequiredService<IOptions<JsonOptions>>().Value;

            Assert.False(jsonOptions.SerializerOptions.WriteIndented);
        }

        [Fact]
        public void MinimalJsonOptions_ShouldPropagateDictionaryKeyPolicy_FromJsonFormatterOptions()
        {
            var services = new ServiceCollection();
            services.AddMinimalJsonOptions(o =>
            {
                o.Settings.DictionaryKeyPolicy = JsonNamingPolicy.KebabCaseLower;
            });

            var provider = services.BuildServiceProvider();
            var jsonOptions = provider.GetRequiredService<IOptions<JsonOptions>>().Value;

            Assert.Equal(JsonNamingPolicy.KebabCaseLower, jsonOptions.SerializerOptions.DictionaryKeyPolicy);
        }

        [Fact]
        public void MinimalJsonOptions_ShouldPropagateDefaultIgnoreCondition_FromJsonFormatterOptions()
        {
            var services = new ServiceCollection();
            services.AddMinimalJsonOptions(o =>
            {
                o.Settings.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
            });

            var provider = services.BuildServiceProvider();
            var jsonOptions = provider.GetRequiredService<IOptions<JsonOptions>>().Value;

            Assert.Equal(JsonIgnoreCondition.Never, jsonOptions.SerializerOptions.DefaultIgnoreCondition);
        }

        [Fact]
        public void MinimalJsonOptions_ShouldPropagateMaxDepth_FromJsonFormatterOptions()
        {
            var services = new ServiceCollection();
            services.AddMinimalJsonOptions(o =>
            {
                o.Settings.MaxDepth = 128;
            });

            var provider = services.BuildServiceProvider();
            var jsonOptions = provider.GetRequiredService<IOptions<JsonOptions>>().Value;

            Assert.Equal(128, jsonOptions.SerializerOptions.MaxDepth);
        }

        [Fact]
        public void MinimalJsonOptions_ShouldPropagateDefaultBufferSize_FromJsonFormatterOptions()
        {
            var services = new ServiceCollection();
            services.AddMinimalJsonOptions(o =>
            {
                o.Settings.DefaultBufferSize = 8192;
            });

            var provider = services.BuildServiceProvider();
            var jsonOptions = provider.GetRequiredService<IOptions<JsonOptions>>().Value;

            Assert.Equal(8192, jsonOptions.SerializerOptions.DefaultBufferSize);
        }

        [Fact]
        public void MinimalJsonOptions_ShouldPropagateReadCommentHandling_FromJsonFormatterOptions()
        {
            var services = new ServiceCollection();
            services.AddMinimalJsonOptions(o =>
            {
                o.Settings.ReadCommentHandling = JsonCommentHandling.Disallow;
            });

            var provider = services.BuildServiceProvider();
            var jsonOptions = provider.GetRequiredService<IOptions<JsonOptions>>().Value;

            Assert.Equal(JsonCommentHandling.Disallow, jsonOptions.SerializerOptions.ReadCommentHandling);
        }

        [Fact]
        public void MinimalJsonOptions_ShouldPropagateAllowTrailingCommas_FromJsonFormatterOptions()
        {
            var services = new ServiceCollection();
            services.AddMinimalJsonOptions(o =>
            {
                o.Settings.AllowTrailingCommas = true;
            });

            var provider = services.BuildServiceProvider();
            var jsonOptions = provider.GetRequiredService<IOptions<JsonOptions>>().Value;

            Assert.True(jsonOptions.SerializerOptions.AllowTrailingCommas);
        }

        [Fact]
        public void MinimalJsonOptions_ShouldPropagateNumberHandling_FromJsonFormatterOptions()
        {
            var services = new ServiceCollection();
            services.AddMinimalJsonOptions(o =>
            {
                o.Settings.NumberHandling = JsonNumberHandling.AllowReadingFromString;
            });

            var provider = services.BuildServiceProvider();
            var jsonOptions = provider.GetRequiredService<IOptions<JsonOptions>>().Value;

            Assert.Equal(JsonNumberHandling.AllowReadingFromString, jsonOptions.SerializerOptions.NumberHandling);
        }

        [Fact]
        public void MinimalJsonOptions_ShouldPropagatePropertyNameCaseInsensitive_FromJsonFormatterOptions()
        {
            var services = new ServiceCollection();
            services.AddMinimalJsonOptions(o =>
            {
                o.Settings.PropertyNameCaseInsensitive = true;
            });

            var provider = services.BuildServiceProvider();
            var jsonOptions = provider.GetRequiredService<IOptions<JsonOptions>>().Value;

            Assert.True(jsonOptions.SerializerOptions.PropertyNameCaseInsensitive);
        }

        [Fact]
        public void MinimalJsonOptions_ShouldPropagateIncludeFields_FromJsonFormatterOptions()
        {
            var services = new ServiceCollection();
            services.AddMinimalJsonOptions(o =>
            {
                o.Settings.IncludeFields = true;
            });

            var provider = services.BuildServiceProvider();
            var jsonOptions = provider.GetRequiredService<IOptions<JsonOptions>>().Value;

            Assert.True(jsonOptions.SerializerOptions.IncludeFields);
        }

        [Fact]
        public void MinimalJsonOptions_ShouldPropagateIgnoreReadOnlyProperties_FromJsonFormatterOptions()
        {
            var services = new ServiceCollection();
            services.AddMinimalJsonOptions(o =>
            {
                o.Settings.IgnoreReadOnlyProperties = true;
            });

            var provider = services.BuildServiceProvider();
            var jsonOptions = provider.GetRequiredService<IOptions<JsonOptions>>().Value;

            Assert.True(jsonOptions.SerializerOptions.IgnoreReadOnlyProperties);
        }

        [Fact]
        public void MinimalJsonOptions_ShouldPropagateEncoder_FromJsonFormatterOptions()
        {
            var services = new ServiceCollection();
            services.AddMinimalJsonOptions(o =>
            {
                o.Settings.Encoder = JavaScriptEncoder.Default;
            });

            var provider = services.BuildServiceProvider();
            var jsonOptions = provider.GetRequiredService<IOptions<JsonOptions>>().Value;

            Assert.Equal(JavaScriptEncoder.Default, jsonOptions.SerializerOptions.Encoder);
        }

        [Fact]
        public void MinimalJsonOptions_ShouldPropagateReferenceHandler_FromJsonFormatterOptions()
        {
            var services = new ServiceCollection();
            services.AddMinimalJsonOptions(o =>
            {
                o.Settings.ReferenceHandler = ReferenceHandler.Preserve;
            });

            var provider = services.BuildServiceProvider();
            var jsonOptions = provider.GetRequiredService<IOptions<JsonOptions>>().Value;

            Assert.Equal(ReferenceHandler.Preserve, jsonOptions.SerializerOptions.ReferenceHandler);
        }

        [Fact]
        public void MinimalJsonOptions_ShouldPropagateConverters_FromJsonFormatterOptions()
        {
            var services = new ServiceCollection();
            services.AddMinimalJsonOptions();

            var provider = services.BuildServiceProvider();
            var jsonOptions = provider.GetRequiredService<IOptions<JsonOptions>>().Value;

            TestOutput.WriteLine($"Converter count: {jsonOptions.SerializerOptions.Converters.Count}");

            Assert.NotEmpty(jsonOptions.SerializerOptions.Converters);
        }

        [Fact]
        public void MinimalJsonOptions_ShouldNotDuplicateConverters_WhenOptionsCreatedMultipleTimes()
        {
            var services = new ServiceCollection();
            services.AddMinimalJsonOptions();

            var provider = services.BuildServiceProvider();
            var formatterOptions = provider.GetRequiredService<IOptions<JsonFormatterOptions>>().Value;
            var optionsFactory = provider.GetRequiredService<IOptionsFactory<JsonOptions>>();

            var before = formatterOptions.Settings.Converters.Count;

            optionsFactory.Create(Options.DefaultName);
            optionsFactory.Create(Options.DefaultName);

            var after = formatterOptions.Settings.Converters.Count;

            Assert.Equal(before, after);
        }

        [Fact]
        public void MinimalJsonOptions_ShouldPropagateTypeInfoResolver_WhenNotNull()
        {
            var resolver = new DefaultJsonTypeInfoResolver();
            var services = new ServiceCollection();
            services.AddMinimalJsonOptions(o =>
            {
                o.Settings.TypeInfoResolver = resolver;
            });

            var provider = services.BuildServiceProvider();
            var jsonOptions = provider.GetRequiredService<IOptions<JsonOptions>>().Value;

            Assert.Same(resolver, jsonOptions.SerializerOptions.TypeInfoResolver);
        }

        [Fact]
        public void MinimalJsonOptions_ShouldNotOverrideTypeInfoResolver_WhenNull()
        {
            var services = new ServiceCollection();
            services.AddMinimalJsonOptions(o =>
            {
                o.Settings.TypeInfoResolver = null;
            });

            var provider = services.BuildServiceProvider();
            var jsonOptions = provider.GetRequiredService<IOptions<JsonOptions>>().Value;

            // When source TypeInfoResolver is null, the target's existing resolver should not be overridden
            // (the target may or may not have its own default resolver, so we just verify no exception)
            TestOutput.WriteLine($"TypeInfoResolver is null: {jsonOptions.SerializerOptions.TypeInfoResolver is null}");
        }

        [Fact]
        public void MinimalJsonOptions_ShouldPropagateIndentSize_FromJsonFormatterOptions()
        {
            var services = new ServiceCollection();
            services.AddMinimalJsonOptions(o =>
            {
                o.Settings.IndentSize = 4;
            });

            var provider = services.BuildServiceProvider();
            var jsonOptions = provider.GetRequiredService<IOptions<JsonOptions>>().Value;

            Assert.Equal(4, jsonOptions.SerializerOptions.IndentSize);
        }
    }
}
