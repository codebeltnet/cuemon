---
uid: Cuemon.Extensions.Xml.DateTimeExtensions
example:
- *content
---

The following example demonstrates how to format DateTime values as XML strings using DateTimeExtensions, supporting UTC, local, round-trip, and unspecified serialization modes.

```csharp
using System;
using System.Xml;
using Cuemon.Extensions.Xml;

namespace MyApp.Xml
{
    public class DateTimeExtensionsExample
    {
        public void Demonstrate()
        {
            DateTime utcNow = DateTime.UtcNow;

            // Format as XML UTC string (appends "Z" for UTC)
            string xmlUtc = utcNow.ToString(XmlDateTimeSerializationMode.Utc);
            Console.WriteLine(xmlUtc);
            // Output: 2026-06-16T12:34:56.789Z

            // Convert local time to XML local string (includes offset)
            DateTime localNow = DateTime.Now;
            string xmlLocal = localNow.ToString(XmlDateTimeSerializationMode.Local);
            Console.WriteLine(xmlLocal);
            // Output: 2026-06-16T14:34:56.789+02:00

            // Round-trip format preserves the Kind information
            string xmlRoundtrip = utcNow.ToString(XmlDateTimeSerializationMode.RoundtripKind);
            Console.WriteLine(xmlRoundtrip);
            // Output: 2026-06-16T12:34:56.789Z

            // Unspecified mode drops any time zone info
            string xmlUnspecified = utcNow.ToString(XmlDateTimeSerializationMode.Unspecified);
            Console.WriteLine(xmlUnspecified);
            // Output: 2026-06-16T12:34:56.789

}}
}

```
