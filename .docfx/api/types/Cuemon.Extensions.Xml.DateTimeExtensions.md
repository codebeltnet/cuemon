---
uid: Cuemon.Extensions.Xml.DateTimeExtensions
example:
- *content
---

`DateTimeExtensions` in the `Xml` namespace formats `DateTime` values as XML strings using `XmlDateTimeSerializationMode` enum values. This example creates `DateTime.UtcNow` and `DateTime.Now`, then calls `ToString` with `XmlDateTimeSerializationMode.Utc` (appends `Z`), `Local` (includes timezone offset like `+02:00`), `RoundtripKind` (preserves `Kind` information), and `Unspecified` (omits timezone info). Console output for each mode shows how time zone information is represented or omitted, such as `2026-06-16T12:34:56.789Z` for UTC and `2026-06-16T14:34:56.789+02:00` for local.

```csharp
// Program.cs
using System;
using System.Xml;
using Cuemon.Extensions.Xml;

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
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="266e4575eee346409f229e66"><a href="~/run-actions/266e4575eee346409f229e66/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/266e4575eee346409f229e66/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
