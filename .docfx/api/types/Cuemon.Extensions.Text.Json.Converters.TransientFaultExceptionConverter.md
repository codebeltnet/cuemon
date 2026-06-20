---
uid: Cuemon.Extensions.Text.Json.Converters.TransientFaultExceptionConverter
example:
- *content
---

`TransientFaultExceptionConverter` serializes and deserializes `TransientFaultException` instances including their `TransientFaultEvidence` with attempt count, recovery wait times, latency, and method signature. This example creates a `TransientFaultException` with evidence (`Attempts = 3`, `RecoveryWaitTime = 2s`, etc.) and a method descriptor, then configures a `JsonFormatter` with the converter and serializes it to JSON. The deserialization part reads the JSON back through the same formatter, reconstructing the exception with its `Evidence` properties intact. Console output displays the round-tripped message (`"Failed to connect after 3 retries."`) and evidence attempt count (`3`).

```csharp
using System;
using System.IO;
using System.Text;
using Cuemon.Extensions.Text.Json.Converters;
using Cuemon.Extensions.Text.Json;
using Cuemon.Extensions.Text.Json.Formatters;
using Cuemon.Reflection;
using Cuemon.Resilience;

namespace MyApp.Examples;

public class TransientFaultExceptionConverterExample
{
    public void SerializeTransientFaultException()
    {
        // Create a transient fault exception with evidence
        var evidence = new TransientFaultEvidence(
            attempts: 3,
            recoveryWaitTime: TimeSpan.FromSeconds(2),
            totalRecoveryWaitTime: TimeSpan.FromSeconds(6),
            latency: TimeSpan.FromMilliseconds(500),
            descriptor: new MethodSignature(
                "MyService",
                "ConnectAsync",
                new[] { "connectionString" },
                new object[] { "server=db;timeout=30" }));

        var exception = new TransientFaultException(
            "Failed to connect after 3 retries.",
            new TimeoutException("Connection timed out."),
            evidence);

        // Configure JSON formatter with the converter
        var options = new JsonFormatterOptions();
        options.Settings.Converters.Add(new TransientFaultExceptionConverter());

        // Serialize to JSON
        var formatter = new JsonFormatter(options);
        using (var stream = formatter.Serialize(exception))
        using (var reader = new StreamReader(stream))
        {
            string json = reader.ReadToEnd();
            Console.WriteLine(json);
            // The output includes exception details, inner exception, and evidence:
            // {
            //   "type": "Cuemon.Resilience.TransientFaultException",
            //   "message": "Failed to connect after 3 retries.",
            //   "evidence": {
            //     "attempts": 3,
            //     ...
            //   }
            // }
        }
    }

    public void DeserializeTransientFaultException()
    {
        string json = @"{
  ""type"": ""Cuemon.Resilience.TransientFaultException"",
  ""message"": ""Failed to connect after 3 retries."",
  ""evidence"": {
    ""attempts"": 3,
    ""recoveryWaitTime"": ""00:00:02"",
    ""totalRecoveryWaitTime"": ""00:00:06"",
    ""latency"": ""00:00:00.500"",
    ""descriptor"": {
      ""caller"": ""MyService"",
      ""methodName"": ""ConnectAsync"",
      ""parameters"": [""connectionString""],
      ""arguments"": [""server=db;timeout=30""]
}";

        var options = new JsonFormatterOptions();
        options.Settings.Converters.Add(new TransientFaultExceptionConverter());

        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
        {
            var formatter = new JsonFormatter(options);
            var exception = formatter.Deserialize<TransientFaultException>(stream);

            Console.WriteLine(exception.Message);
            Console.WriteLine($"Evidence - Attempts: {exception.Evidence.Attempts}");
        }
    }
}
```
