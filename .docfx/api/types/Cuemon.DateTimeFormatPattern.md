---
uid: Cuemon.DateTimeFormatPattern
example:
- *content
---

The following example demonstrates how to use `DateTimeFormatPattern` to select a format pattern for date and time display.

```csharp
// Program.cs
using System;
using Cuemon;

var shortDate = DateTimeFormatPattern.ShortDate;
var longDateTime = DateTimeFormatPattern.LongDateTime;

var now = DateTime.Now;
Console.WriteLine($"Selected pattern: {shortDate}");
Console.WriteLine($"Selected pattern: {longDateTime}");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="0b5c231f4e845e9b3a082eb2"><a href="~/run-actions/0b5c231f4e845e9b3a082eb2/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/0b5c231f4e845e9b3a082eb2/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
