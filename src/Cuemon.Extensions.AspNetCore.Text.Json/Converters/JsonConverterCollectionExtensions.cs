using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Diagnostics;
using Cuemon.Extensions.Text.Json;
using Cuemon.Extensions.Text.Json.Converters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Cuemon.Extensions.AspNetCore.Text.Json.Converters;
/// <summary>
/// Extension methods for the <see cref="JsonConverter"/> class.
/// </summary>
public static class JsonConverterCollectionExtensions
{
    /// <summary>
    /// Adds an <see cref="IHeaderDictionary"/> JSON converter to the collection.
    /// </summary>
    /// <param name="converters">The collection of <see cref="JsonConverter"/> to extend.</param>  
    /// <returns>A reference to <paramref name="converters"/> so that additional calls can be chained.</returns>  
    public static ICollection<JsonConverter> AddHeaderDictionaryConverter(this ICollection<JsonConverter> converters)
    {
        converters.Add(DynamicJsonConverter.Create<IHeaderDictionary>((writer, value, options) =>
        {
            writer.WriteStartObject();
            foreach (var kvp in value)
            {
                writer.WritePropertyName(options.SetPropertyName(kvp.Key));
                writer.WriteStringValue(kvp.Value);
            }
            writer.WriteEndObject();
        }, (ref reader, _, _) =>
        {
            var dictionary = new HeaderDictionary();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    break;
                }
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    var key = reader.GetString()!;
                    reader.Read();
                    var value = reader.GetString();
                    dictionary.Add(key, value);
                }
            }
            return dictionary;
        }));
        return converters;
    }

    /// <summary>
    /// Adds a <see cref="ProblemDetails"/> JSON converter to the collection.
    /// </summary>
    /// <param name="converters">The collection of <see cref="JsonConverter"/> to extend.</param> 
    /// <returns>A reference to <paramref name="converters"/> so that additional calls can be chained.</returns>
    public static ICollection<JsonConverter> AddProblemDetailsConverter(this ICollection<JsonConverter> converters)
    {
        converters.Add(DynamicJsonConverter.Create<ProblemDetails>(WriteProblemDetails));
        converters.Add(DynamicJsonConverter.Create<IDecorator<ProblemDetails>>((writer, dpd, options) => WriteProblemDetails(writer, dpd.Inner, options)));
        return converters;
    }

    private static void WriteProblemDetails(Utf8JsonWriter writer, ProblemDetails pd, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        if (pd.Type != null) { writer.WriteString(options.SetPropertyName(nameof(ProblemDetails.Type)), pd.Type); }
        if (pd.Title != null) { writer.WriteString(options.SetPropertyName(nameof(ProblemDetails.Title)), pd.Title); }
        if (pd.Status.HasValue) { writer.WriteNumber(options.SetPropertyName(nameof(ProblemDetails.Status)), pd.Status.Value); }
        if (pd.Detail != null) { writer.WriteString(options.SetPropertyName(nameof(ProblemDetails.Detail)), pd.Detail); }
        if (pd.Instance != null) { writer.WriteString(options.SetPropertyName(nameof(ProblemDetails.Instance)), pd.Instance); }

        foreach (var extension in pd.Extensions.Where(kvp => kvp.Value != null))
        {
            writer.WritePropertyName(options.SetPropertyName(extension.Key));
            writer.WriteObject(extension.Value, options);
        }

        writer.WriteEndObject();
    }

    /// <summary>
    /// Adds an <see cref="HttpExceptionDescriptor"/> JSON converter to the collection.
    /// </summary>
    /// <param name="converters">The collection of <see cref="JsonConverter"/> to extend.</param> 
    /// <param name="setup">The <see cref="ExceptionDescriptorOptions"/> which may be configured.</param>
    /// <returns>A reference to <paramref name="converters"/> so that additional calls can be chained.</returns>
    public static ICollection<JsonConverter> AddHttpExceptionDescriptorConverter(this ICollection<JsonConverter> converters, Action<ExceptionDescriptorOptions> setup = null)
    {
        converters.AddExceptionDescriptorConverterOf<HttpExceptionDescriptor>(setup, (writer, descriptor, options) =>
        {
            if (descriptor.Instance != null) { writer.WriteString(options.SetPropertyName("Instance"), descriptor.Instance.OriginalString); }
            writer.WriteNumber(options.SetPropertyName("Status"), descriptor.StatusCode);
        }, (writer, descriptor, options) =>
        {
            if (!string.IsNullOrWhiteSpace(descriptor.CorrelationId))
            {
                writer.WriteString(options.SetPropertyName("CorrelationId"), descriptor.CorrelationId);
            }
            if (!string.IsNullOrWhiteSpace(descriptor.RequestId))
            {
                writer.WriteString(options.SetPropertyName("RequestId"), descriptor.RequestId);
            }
            if (!string.IsNullOrWhiteSpace(descriptor.TraceId))
            {
                writer.WriteString(options.SetPropertyName("TraceId"), descriptor.TraceId);
            }
        });
        return converters;
    }

    /// <summary>
    /// Adds an <see cref="StringValues"/> JSON converter to the collection.
    /// </summary>
    /// <param name="converters">The collection of <see cref="JsonConverter"/> to extend.</param> 
    /// <returns>A reference to <paramref name="converters"/> so that additional calls can be chained.</returns>
    public static ICollection<JsonConverter> AddStringValuesConverter(this ICollection<JsonConverter> converters)
    {
        converters.Add(DynamicJsonConverter.Create<StringValues>((writer, values, _) =>
        {
            if (values.Count <= 1)
            {
                writer.WriteStringValue(values.ToString());
            }
            else
            {
                writer.WriteStartArray();
                foreach (var value in values) { writer.WriteStringValue(value); }
                writer.WriteEndArray();
            }
        }));
        return converters;
    }
}
