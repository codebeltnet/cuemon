using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Cuemon.Diagnostics;
using Cuemon.Extensions.Text.Json.Formatters;
using Cuemon.Resilience;

namespace Cuemon.Extensions.Text.Json.Converters;
/// <summary>
/// Extension methods for the <see cref="JsonConverter"/> class.
/// </summary>
public static class JsonConverterCollectionExtensions
{
    /// <summary>
    /// Removes one or more <see cref="JsonConverter"/> implementations where <see cref="JsonConverter.CanConvert"/> evaluates <c>true</c> in the collection of <paramref name="converters"/>.
    /// </summary>
    /// <typeparam name="T">The type of object or value handled by the <see cref="JsonConverter"/>.</typeparam>
    /// <param name="converters">The collection of <see cref="JsonConverter"/> to extend.</param>
    /// <returns>A reference to <paramref name="converters"/> so that additional calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="converters"/> cannot be null.
    /// </exception>
    public static ICollection<JsonConverter> RemoveAllOf<T>(this ICollection<JsonConverter> converters)
    {
        return RemoveAllOf(converters, typeof(T));
    }

    /// <summary>
    /// Removes one or more <see cref="JsonConverter"/> implementations where <see cref="JsonConverter.CanConvert"/> evaluates <c>true</c> in the collection of <paramref name="converters"/>.
    /// </summary>
    /// <param name="converters">The collection of <see cref="JsonConverter"/> to extend.</param>
    /// <param name="types">The type of objects or values handled by a sequence of <see cref="JsonConverter"/>.</param>
    /// <returns>A reference to <paramref name="converters"/> so that additional calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="converters"/> cannot be null.
    /// </exception>
    public static ICollection<JsonConverter> RemoveAllOf(this ICollection<JsonConverter> converters, params Type[] types)
    {
        Validator.ThrowIfNull(converters);
        Validator.ThrowIfNull(types);
        var rejects = types.SelectMany(type => converters.Where(jc => jc.CanConvert(type))).ToList();
        foreach (var reject in rejects)
        {
            converters.Remove(reject);
        }
        return converters;
    }

    /// <summary>
    /// Adds a <see cref="TransientFaultException"/> JSON converter to the list.
    /// </summary>
    /// <param name="converters">The <see cref="ICollection{JsonConverter}" /> to extend.</param>
    /// <returns>A reference to <paramref name="converters" /> after the operation has completed.</returns>
    public static ICollection<JsonConverter> AddTransientFaultExceptionConverter(this ICollection<JsonConverter> converters)
    {
        converters.Add(new TransientFaultExceptionConverter());
        return converters;
    }

    /// <summary>
    /// Adds a configurable <see cref="DateTime" /> JSON converter to the list.
    /// </summary>
    /// <param name="converters">The <see cref="ICollection{JsonConverter}" /> to extend.</param>
    /// <param name="format">A standard or custom date and time format string.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>A reference to <paramref name="converters" /> after the operation has completed.</returns>
    /// <remarks>If you miss the opportunity to configure DateTime format handling like you could with Newtonsoft.JSON, here is an alternative way. Default is <c>"O"</c> (https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings#table-of-format-specifiers).</remarks>
    /// <example><code>var formatter = new JsonFormatter(o =&gt; o.Settings.Converters.AddDateTimeConverter());</code></example>
    public static ICollection<JsonConverter> AddDateTimeConverter(this ICollection<JsonConverter> converters, string format = "O", CultureInfo provider = null)
    {
        converters.Add(new DateTimeConverter(format, provider));
        return converters;
    }

    /// <summary>
    /// Adds an <see cref="Enum" /> JSON converter to the list.
    /// </summary>
    /// <param name="converters">The <see cref="ICollection{JsonConverter}" /> to extend.</param>
    /// <param name="namingPolicy">The optional naming policy for writing enum values.</param>
    /// <returns>A reference to <paramref name="converters" /> after the operation has completed.</returns>
    /// <remarks>Default implementation will, just like Newtonsoft.Json variant, favor <see cref="StringEnumConverter"/> with a fallback to <see cref="JsonStringEnumConverter"/> using default naming policy from <see cref="JsonFormatterOptions"/>.</remarks>
    public static ICollection<JsonConverter> AddStringEnumConverter(this ICollection<JsonConverter> converters, JsonNamingPolicy namingPolicy = null)
    {
        if (namingPolicy != null)
        {
            converters.Add(new JsonStringEnumConverter(namingPolicy, false));
        }
        else
        {
            try
            {
                converters.Add(new StringEnumConverter());
            }
            catch (NotSupportedException)
            {
                var options = new JsonFormatterOptions().Settings;
                converters.Add(new JsonStringEnumConverter(options.PropertyNamingPolicy, false));
            }
        }
        return converters;
    }

    /// <summary>
    /// Adds a combined <see cref="Enum" /> and <see cref="FlagsAttribute" /> JSON converter to the list.
    /// </summary>
    /// <param name="converters">The <see cref="ICollection{JsonConverter}" /> to extend.</param>
    /// <returns>A reference to <paramref name="converters" /> after the operation has completed.</returns>
    public static ICollection<JsonConverter> AddStringFlagsEnumConverter(this ICollection<JsonConverter> converters)
    {
        converters.Add(new StringFlagsEnumConverter());
        return converters;
    }

    /// <summary>
    /// Adds an <see cref="ExceptionDescriptor" /> JSON converter to the list.
    /// </summary>
    /// <param name="converters">The <see cref="ICollection{JsonConverter}" /> to extend.</param>
    /// <param name="setup">The <see cref="ExceptionDescriptorOptions" /> which may be configured.</param>
    /// <param name="afterWriteErrorStartObject">The delegate that is invoked just after writing JSON start object (<c>Error</c>).</param>
    /// <param name="beforeWriteEndObject">The delegate that is invoked just before writing the JSON end object.</param>
    /// <returns>A reference to <paramref name="converters" /> after the operation has completed.</returns>
    public static ICollection<JsonConverter> AddExceptionDescriptorConverterOf<T>(this ICollection<JsonConverter> converters, Action<ExceptionDescriptorOptions> setup = null, Utf8JsonWriterAction<T> afterWriteErrorStartObject = null, Utf8JsonWriterAction<T> beforeWriteEndObject = null) where T : ExceptionDescriptor
    {
        converters.Add(DynamicJsonConverter.Create<T>(type => type == typeof(T), (writer, descriptor, serializerOptions) =>
        {
            var options = Patterns.Configure(setup);
            writer.WriteStartObject();
            writer.WritePropertyName(serializerOptions.SetPropertyName("Error"));
            writer.WriteStartObject();
            afterWriteErrorStartObject?.Invoke(writer, descriptor, serializerOptions);
            writer.WriteString(serializerOptions.SetPropertyName("Code"), descriptor.Code);
            writer.WriteString(serializerOptions.SetPropertyName("Message"), descriptor.Message);
            if (descriptor.HelpLink != null)
            {
                writer.WriteString(serializerOptions.SetPropertyName("HelpLink"), descriptor.HelpLink.OriginalString);
            }
            if (options.SensitivityDetails.HasFlag(FaultSensitivityDetails.Failure))
            {
                writer.WritePropertyName(serializerOptions.SetPropertyName("Failure"));
                new ExceptionConverter(options.SensitivityDetails.HasFlag(FaultSensitivityDetails.StackTrace), options.SensitivityDetails.HasFlag(FaultSensitivityDetails.Data)).Write(writer, descriptor.Failure, serializerOptions);
            }
            writer.WriteEndObject();
            if (options.SensitivityDetails.HasFlag(FaultSensitivityDetails.Evidence) && descriptor.Evidence.Any())
            {
                writer.WritePropertyName(serializerOptions.SetPropertyName("Evidence"));
                writer.WriteStartObject();
                foreach (var evidence in descriptor.Evidence)
                {
                    writer.WritePropertyName(serializerOptions.SetPropertyName(evidence.Key));
                    writer.WriteObject(evidence.Value, serializerOptions);
                }
                writer.WriteEndObject();
            }
            beforeWriteEndObject?.Invoke(writer, descriptor, serializerOptions);
            writer.WriteEndObject();
        }));
        return converters;
    }

    /// <summary>
    /// Adds an <see cref="Exception" /> JSON converter to the list.
    /// </summary>
    /// <param name="converters">The <see cref="ICollection{JsonConverter}" /> to extend.</param>
    /// <param name="includeStackTrace">The value that determine whether the stack of an exception is included in the converted result.</param>
    /// <param name="includeData">The value that determine whether the data of an exception is included in the converted result.</param>
    /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
    public static ICollection<JsonConverter> AddExceptionConverter(this ICollection<JsonConverter> converters, bool includeStackTrace, bool includeData)
    {
        converters.Add(new ExceptionConverter(includeStackTrace, includeData));
        return converters;
    }

    /// <summary>
    /// Adds a <see cref="Failure"/> JSON converter to the list.
    /// </summary>
    /// <param name="converters">The <see cref="ICollection{JsonConverter}" /> to extend.</param>
    /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
    public static ICollection<JsonConverter> AddFailureConverter(this ICollection<JsonConverter> converters)
    {
        converters.Add(DynamicJsonConverter.Create<Failure>((writer, failure, options) =>
        {
            new ExceptionConverter(failure.GetUnderlyingSensitivity().HasFlag(FaultSensitivityDetails.StackTrace), failure.GetUnderlyingSensitivity().HasFlag(FaultSensitivityDetails.Data)).Write(writer, failure.GetUnderlyingException(), options);
        }));
        return converters;
    }

    /// <summary>
    /// Adds an <see cref="DataPair" /> JSON converter to the list.
    /// </summary>
    /// <param name="converters">The <see cref="ICollection{JsonConverter}" /> to extend.</param>
    /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
    public static ICollection<JsonConverter> AddDataPairConverter(this ICollection<JsonConverter> converters)
    {
        converters.Add(DynamicJsonConverter.Create<DataPair>((writer, dp, options) =>
        {
            writer.WriteStartObject();
            writer.WriteString(options.SetPropertyName("Name"), dp.Name);
            if (dp.HasValue)
            {
                var value = (dp.Type == typeof(Uri)) ? Decorator.Enclose(dp.Value).ChangeTypeOrDefault<Uri>().OriginalString : dp.Value;
                writer.WritePropertyName(options.SetPropertyName("Value"));
                writer.WriteObject(value, options);
            }
            writer.WriteString(options.SetPropertyName("Type"), Decorator.Enclose(dp.Type).ToFriendlyName());
            writer.WriteEndObject();
        }));
        return converters;
    }
}
