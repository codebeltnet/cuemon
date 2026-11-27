---
uid: Cuemon.AspNetCore.Http.Throttling.ThrottleQuota
example:
- *content
---

The following example demonstrates how to create `ThrottleQuota` instances using different time units and a `TimeSpan` directly. It then tracks request usage with `ThrottleRequest`, including incrementing and refreshing the window.

```csharp
// Program.cs
using System;
using Cuemon;
using Cuemon.AspNetCore.Http.Throttling;

// Allow 100 requests per 1 minute window
var quotaPerMinute = new ThrottleQuota(100, 1, TimeUnit.Minutes);
Console.WriteLine($"Rate limit: {quotaPerMinute.RateLimit}");
Console.WriteLine($"Window: {quotaPerMinute.Window.TotalMinutes} min");

// Allow 1000 requests per 1 hour window (using TimeSpan directly)
var quotaPerHour = new ThrottleQuota(1000, TimeSpan.FromHours(1));
Console.WriteLine($"Rate limit: {quotaPerHour.RateLimit}");
Console.WriteLine($"Window: {quotaPerHour.Window.TotalHours} h");

// Allow 10 requests per 15 seconds
var quotaPer15Sec = new ThrottleQuota(10, 15, TimeUnit.Seconds);
Console.WriteLine($"Rate limit: {quotaPer15Sec.RateLimit}");
Console.WriteLine($"Window: {quotaPer15Sec.Window.TotalSeconds} s");

// Use with ThrottleRequest to track usage
var request = new ThrottleRequest(quotaPerMinute);
Console.WriteLine($"Initial total: {request.Total}");
Console.WriteLine($"Expires at: {request.Expires:R}");

request.IncrementTotal();
Console.WriteLine($"After one request: {request.Total}");

request.Refresh(); // resets if window has expired
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="890011864474f5adaefd4c8f"><a href="~/run-actions/890011864474f5adaefd4c8f/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/890011864474f5adaefd4c8f/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
