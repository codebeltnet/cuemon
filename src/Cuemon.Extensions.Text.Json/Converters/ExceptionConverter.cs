using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Cuemon.Reflection;
using Cuemon.Runtime.Serialization.Formatters;

namespace Cuemon.Extensions.Text.Json.Converters;
/// <summary>
/// Provides an <see cref="Exception"/> converter that can be configured like the Newtonsoft.JSON equivalent.
/// </summary>
/// <seealso cref="JsonConverter{Exception}" />
public class ExceptionConverter : JsonConverter<Exception>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionConverter"/> class.
    /// </summary>
    /// <param name="includeStackTrace">A value that indicates if the stack of an exception is included in the converted result.</param>
    /// <param name="includeData">A value that indicates if the data of an exception is included in the converted result.</param>
    public ExceptionConverter(bool includeStackTrace = false, bool includeData = false)
    {
        IncludeStackTrace = includeStackTrace;
        IncludeData = includeData;
    }

    /// <summary>
    /// Gets a value indicating whether the data of an exception is included in the converted result.
    /// </summary>
    /// <value><c>true</c> if the data of an exception is included in the converted result; otherwise, <c>false</c>.</value>
    public bool IncludeData { get; }

    /// <summary>
    /// Gets a value indicating whether the stack of an exception is included in the converted result.
    /// </summary>
    /// <value><c>true</c> if the stack of an exception is included in the converted result; otherwise, <c>false</c>.</value>
    public bool IncludeStackTrace { get; }

    /// <summary>
    /// Determines whether this instance can convert the specified object type.
    /// </summary>
    /// <param name="typeToConvert">Type of the object.</param>
    /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
    public override bool CanConvert(Type typeToConvert)
    {
        return typeof(Exception).IsAssignableFrom(typeToConvert);
    }

    /// <summary>
    /// Reads and converts the JSON to type <see cref="Exception"/>.
    /// </summary>
    /// <param name="reader">The <see cref="Utf8JsonReader"/> to read from.</param>
    /// <param name="typeToConvert">The <see cref="Type"/> being converted.</param>
    /// <param name="options">The <see cref="JsonSerializerOptions"/> being used.</param>
    /// <returns>The value that was converted.</returns>
    public override Exception Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var stack = ParseJsonReader(ref reader, typeToConvert, options);
        return Decorator.Enclose(stack).CreateException();
    }

    private static Stack<IList<MemberArgument>> ParseJsonReader(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var stack = new Stack<IList<MemberArgument>>();
        var properties = new List<PropertyInfo>();
        var lastDepth = 1;
        var blueprints = new List<MemberArgument>();
        while (reader.Read())
        {
            if (reader.CurrentDepth != lastDepth && blueprints.Count > 0)
            {
                stack.Push(blueprints);
                blueprints = new List<MemberArgument>();
            }

            switch (reader.TokenType)
            {
                case JsonTokenType.PropertyName:
                    string memberName = MapOrDefault(reader.GetString());
                    if (!reader.Read())
                    {
                        // throw
                    }
                    var property = properties.SingleOrDefault(pi => pi.Name.Equals(memberName, StringComparison.OrdinalIgnoreCase));
                    if (property != null)
                    {
                        if (property.Name == nameof(Exception.InnerException))
                        {
                            blueprints.Add(new MemberArgument(memberName, null));
                        }
                        else
                        {
                            var propertyValue = JsonSerializer.Deserialize(ref reader, property.PropertyType, options);
                            if (propertyValue is JsonElement element)
                            {
                                propertyValue = element.GetRawText();
                            }
                            blueprints.Add(new MemberArgument(memberName, propertyValue));
                        }
                    }
                    else
                    {
                        if (memberName.Equals("type", StringComparison.OrdinalIgnoreCase))
                        {
                            typeToConvert = Formatter.GetType(reader.GetString());
                            properties = typeToConvert.GetProperties(MemberReflection.CreateFlags(o => o.ExcludeStatic = true)).ToList();
                            blueprints.Add(new MemberArgument(memberName, typeToConvert));
                        }
                    }
                    break;
                case JsonTokenType.Comment:
                    break;
                case JsonTokenType.EndObject:
                    break;
                default:
                    break;
                    // throw
            }
            lastDepth = reader.CurrentDepth;
        }

        return stack;
    }

    private static string MapOrDefault(string memberName)
    {
        switch (memberName.ToLowerInvariant())
        {
            case "inner":
                return nameof(Exception.InnerException);
            case "stack":
                return nameof(Exception.StackTrace);
            default:
                return memberName;
        }
    }

    /// <summary>
    /// Writes the <paramref name="value"/> as JSON.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> to write to.</param>
    /// <param name="value">The value to convert.</param>
    /// <param name="options">The <see cref="JsonSerializerOptions"/> being used.</param>
    public override void Write(Utf8JsonWriter writer, Exception value, JsonSerializerOptions options)
    {
        var exceptionType = value.GetType();
        writer.WriteStartObject();
        writer.WriteString(options.SetPropertyName("Type"), exceptionType.FullName);
        WriteExceptionCore(writer, value, IncludeStackTrace, IncludeData, options);
        writer.WriteEndObject();
    }

    private static void WriteExceptionCore(Utf8JsonWriter writer, Exception exception, bool includeStackTrace, bool includeData, JsonSerializerOptions options)
    {
        if (!string.IsNullOrWhiteSpace(exception.Source))
        {
            writer.WriteString(options.SetPropertyName("Source"), exception.Source);
        }

        if (!string.IsNullOrWhiteSpace(exception.Message))
        {
            writer.WriteString(options.SetPropertyName("Message"), exception.Message);
        }

        if (exception.StackTrace != null && includeStackTrace)
        {
            writer.WritePropertyName(options.SetPropertyName("Stack"));
            writer.WriteStartArray();
            var lines = exception.StackTrace.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                writer.WriteStringValue(line.Trim());
            }
            writer.WriteEndArray();
        }

        if (includeData && exception.Data.Count > 0)
        {
            writer.WritePropertyName(options.SetPropertyName("Data"));
            writer.WriteStartObject();
            foreach (DictionaryEntry entry in exception.Data)
            {
                writer.WritePropertyName(options.SetPropertyName(entry.Key.ToString()));
                writer.WriteObject(entry.Value, options);
            }
            writer.WriteEndObject();
        }

        var properties = Decorator.Enclose(exception.GetType()).GetRuntimePropertiesExceptOf<AggregateException>();
        foreach (var property in properties)
        {
            var value = property.GetValue(exception);
            if (value == null) { continue; }
            writer.WritePropertyName(options.SetPropertyName(property.Name));
            writer.WriteObject(value, options);
        }

        WriteInnerExceptions(writer, exception, includeStackTrace, includeData, options);
    }

    private static void WriteInnerExceptions(Utf8JsonWriter writer, Exception exception, bool includeStackTrace, bool includeData, JsonSerializerOptions options)
    {
        var innerExceptions = new List<Exception>();
        if (exception is AggregateException aggregated)
        {
            innerExceptions.AddRange(aggregated.Flatten().InnerExceptions);
        }
        else
        {
            if (exception.InnerException != null) { innerExceptions.Add(exception.InnerException); }
        }
        if (innerExceptions.Count > 0)
        {
            var endElementsToWrite = 0;
            foreach (var inner in innerExceptions)
            {
                writer.WritePropertyName(options.SetPropertyName("Inner"));
                var exceptionType = inner.GetType();
                writer.WriteStartObject();
                writer.WriteString(options.SetPropertyName("Type"), exceptionType.FullName);
                WriteExceptionCore(writer, inner, includeStackTrace, includeData, options);
                endElementsToWrite++;
            }

            for (var i = 0; i < endElementsToWrite; i++) { writer.WriteEndObject(); }
        }
    }
}
