using Cuemon.Collections.Generic;
using Cuemon.Extensions.AspNetCore.Text.Json.Converters;
using Cuemon.Extensions.Text.Json.Formatters;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;

namespace Cuemon.Extensions.AspNetCore.Text.Json
{
    /// <summary>
    /// A <see cref="ConfigureOptions{TOptions}"/> implementation which will pass <see cref="JsonFormatterOptions"/> to <see cref="JsonOptions"/>.
    /// </summary>
    public class MinimalJsonOptions : ConfigureOptions<JsonOptions>
    {
        /// <summary>
        /// Creates a new <see cref="MinimalJsonOptions"/>.
        /// </summary>
        public MinimalJsonOptions(IOptions<JsonFormatterOptions> formatterOptions) : base(mo =>
        {
            var options = formatterOptions.Value;

            options.Settings.Converters.AddHttpExceptionDescriptorConverter(o => o.SensitivityDetails = options.SensitivityDetails);

            Decorator.Enclose(mo.SerializerOptions.Converters).AddRange(options.Settings.Converters);
            mo.SerializerOptions.AllowOutOfOrderMetadataProperties = options.Settings.AllowOutOfOrderMetadataProperties;
            mo.SerializerOptions.AllowTrailingCommas = options.Settings.AllowTrailingCommas;
            mo.SerializerOptions.DefaultBufferSize = options.Settings.DefaultBufferSize;
            mo.SerializerOptions.Encoder = options.Settings.Encoder;
            mo.SerializerOptions.DictionaryKeyPolicy = options.Settings.DictionaryKeyPolicy;
            mo.SerializerOptions.DefaultIgnoreCondition = options.Settings.DefaultIgnoreCondition;
            mo.SerializerOptions.NumberHandling = options.Settings.NumberHandling;
            mo.SerializerOptions.PreferredObjectCreationHandling = options.Settings.PreferredObjectCreationHandling;
            mo.SerializerOptions.UnknownTypeHandling = options.Settings.UnknownTypeHandling;
            mo.SerializerOptions.UnmappedMemberHandling = options.Settings.UnmappedMemberHandling;
            mo.SerializerOptions.IgnoreReadOnlyProperties = options.Settings.IgnoreReadOnlyProperties;
            mo.SerializerOptions.IgnoreReadOnlyFields = options.Settings.IgnoreReadOnlyFields;
            mo.SerializerOptions.IncludeFields = options.Settings.IncludeFields;
            mo.SerializerOptions.MaxDepth = options.Settings.MaxDepth;
            mo.SerializerOptions.PropertyNamingPolicy = options.Settings.PropertyNamingPolicy;
            mo.SerializerOptions.PropertyNameCaseInsensitive = options.Settings.PropertyNameCaseInsensitive;
            mo.SerializerOptions.ReadCommentHandling = options.Settings.ReadCommentHandling;
            mo.SerializerOptions.WriteIndented = options.Settings.WriteIndented;
            mo.SerializerOptions.IndentCharacter = options.Settings.IndentCharacter;
            mo.SerializerOptions.IndentSize = options.Settings.IndentSize;
            mo.SerializerOptions.ReferenceHandler = options.Settings.ReferenceHandler;
            mo.SerializerOptions.NewLine = options.Settings.NewLine;
            mo.SerializerOptions.RespectNullableAnnotations = options.Settings.RespectNullableAnnotations;
            mo.SerializerOptions.RespectRequiredConstructorParameters = options.Settings.RespectRequiredConstructorParameters;
#if NET10_0_OR_GREATER
            mo.SerializerOptions.AllowDuplicateProperties = options.Settings.AllowDuplicateProperties;
#endif
            if (options.Settings.TypeInfoResolver is not null)
            {
                mo.SerializerOptions.TypeInfoResolver = options.Settings.TypeInfoResolver;
            }
        })
        {
        }
    }
}
