using System.Text.Json;
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

            var settings = new JsonSerializerOptions(options.Settings);
            settings.Converters.AddHttpExceptionDescriptorConverter(o => o.SensitivityDetails = options.SensitivityDetails);

            Decorator.Enclose(mo.SerializerOptions.Converters).AddRange(settings.Converters);
            mo.SerializerOptions.AllowOutOfOrderMetadataProperties = settings.AllowOutOfOrderMetadataProperties;
            mo.SerializerOptions.AllowTrailingCommas = settings.AllowTrailingCommas;
            mo.SerializerOptions.DefaultBufferSize = settings.DefaultBufferSize;
            mo.SerializerOptions.Encoder = settings.Encoder;
            mo.SerializerOptions.DictionaryKeyPolicy = settings.DictionaryKeyPolicy;
            mo.SerializerOptions.DefaultIgnoreCondition = settings.DefaultIgnoreCondition;
            mo.SerializerOptions.NumberHandling = settings.NumberHandling;
            mo.SerializerOptions.PreferredObjectCreationHandling = settings.PreferredObjectCreationHandling;
            mo.SerializerOptions.UnknownTypeHandling = settings.UnknownTypeHandling;
            mo.SerializerOptions.UnmappedMemberHandling = settings.UnmappedMemberHandling;
            mo.SerializerOptions.IgnoreReadOnlyProperties = settings.IgnoreReadOnlyProperties;
            mo.SerializerOptions.IgnoreReadOnlyFields = settings.IgnoreReadOnlyFields;
            mo.SerializerOptions.IncludeFields = settings.IncludeFields;
            mo.SerializerOptions.MaxDepth = settings.MaxDepth;
            mo.SerializerOptions.PropertyNamingPolicy = settings.PropertyNamingPolicy;
            mo.SerializerOptions.PropertyNameCaseInsensitive = settings.PropertyNameCaseInsensitive;
            mo.SerializerOptions.ReadCommentHandling = settings.ReadCommentHandling;
            mo.SerializerOptions.WriteIndented = settings.WriteIndented;
            mo.SerializerOptions.IndentCharacter = settings.IndentCharacter;
            mo.SerializerOptions.IndentSize = settings.IndentSize;
            mo.SerializerOptions.ReferenceHandler = settings.ReferenceHandler;
            mo.SerializerOptions.NewLine = settings.NewLine;
            mo.SerializerOptions.RespectNullableAnnotations = settings.RespectNullableAnnotations;
            mo.SerializerOptions.RespectRequiredConstructorParameters = settings.RespectRequiredConstructorParameters;
#if NET10_0_OR_GREATER
            mo.SerializerOptions.AllowDuplicateProperties = settings.AllowDuplicateProperties;
#endif
            if (settings.TypeInfoResolver is not null)
            {
                mo.SerializerOptions.TypeInfoResolver = settings.TypeInfoResolver;
            }
        })
        {
        }
    }
}
