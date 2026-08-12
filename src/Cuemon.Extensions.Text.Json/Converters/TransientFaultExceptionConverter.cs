using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Cuemon.Reflection;
using Cuemon.Resilience;
using Cuemon.Runtime.Serialization.Formatters;

namespace Cuemon.Extensions.Text.Json.Converters;
/// <summary>
/// Converts a <see cref="TransientFaultException"/> to or from JSON.
/// </summary>
/// <seealso cref="JsonConverter{T}" />
public class TransientFaultExceptionConverter : JsonConverter<TransientFaultException>
{
    /// <summary>
    /// Reads and converts the JSON to type <see cref="TransientFaultException"/>.
    /// </summary>
    /// <param name="reader">The <see cref="Utf8JsonReader"/> to read from.</param>
    /// <param name="typeToConvert">The <see cref="Type"/> being converted.</param>
    /// <param name="options">The <see cref="JsonSerializerOptions"/> being used.</param>
    /// <returns>The value that was converted.</returns>
    public override TransientFaultException Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jo = JsonNode.Parse(ref reader)!.AsObject();
        var message = jo["message"]?.GetValue<string>();
        var evidenceJson = jo["evidence"];
        var innerExceptionJson = jo["inner"];
        Exception innerException = null;
        TransientFaultEvidence evidence = null;

        if (evidenceJson != null)
        {
            var attempts = evidenceJson["attempts"].GetValue<int>();
            var recoveryWaitTime = Decorator.Enclose(evidenceJson["recoveryWaitTime"].GetValue<string>()).ChangeType<TimeSpan>();
            var totalRecoveryWaitTime = Decorator.Enclose(evidenceJson["totalRecoveryWaitTime"].GetValue<string>()).ChangeType<TimeSpan>();
            var latency = Decorator.Enclose(evidenceJson["latency"].GetValue<string>()).ChangeType<TimeSpan>();
            var caller = evidenceJson["descriptor"]["caller"].GetValue<string>();
            var methodName = evidenceJson["descriptor"]["methodName"].GetValue<string>();
            var parameters = evidenceJson["descriptor"]["parameters"].AsArray().Select(node => node.GetValue<string>()).ToArray();
            var arguments = evidenceJson["descriptor"]["arguments"].AsArray().Select(node => node.GetValue<string>()).ToArray();
            evidence = new TransientFaultEvidence(attempts, recoveryWaitTime, totalRecoveryWaitTime, latency, new MethodSignature(caller, methodName, parameters, arguments));
        }

        if (innerExceptionJson != null)
        {
            var converter = options.Converters.FirstOrDefault(converter => converter.CanConvert(typeof(Exception))) as JsonConverter<Exception>; ;
            var innerExceptionReader = new Utf8JsonReader(new ReadOnlySpan<byte>(Decorator.Enclose(innerExceptionJson.ToJsonString()).ToByteArray()));
            innerException = converter?.Read(ref innerExceptionReader, Formatter.GetType(innerExceptionJson["type"].GetValue<string>()), options);
        }

        return new TransientFaultException(message, innerException, evidence);
    }

    /// <summary>
    /// Writes the <paramref name="value"/> as JSON.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> to write to.</param>
    /// <param name="value">The value to convert.</param>
    /// <param name="options">The <see cref="JsonSerializerOptions"/> being used.</param>
    public override void Write(Utf8JsonWriter writer, TransientFaultException value, JsonSerializerOptions options)
    {
        var converter = options.Converters.FirstOrDefault(converter => converter.CanConvert(typeof(Exception))) as JsonConverter<Exception>;
        converter?.Write(writer, value, options);
    }
}
