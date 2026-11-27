---
uid: Cuemon.Generate
example:
- *content
---

The following example demonstrates how to use `Generate` to produce numeric ranges, random numbers, random strings, hash codes, and structured object portrayals.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using Cuemon;

// Generate a range of values using a generator function
IEnumerable<int> numbers = Generate.RangeOf(5, i => i * 10);
Console.WriteLine(string.Join(", ", numbers)); // 0, 10, 20, 30, 40

// Generate a random integer in a specific range
int randomValue = Generate.RandomNumber(1, 101);
Console.WriteLine($"Random between 1 and 100: {randomValue}");

// Generate a random alphanumeric string
string randomStr = Generate.RandomString(12);
Console.WriteLine($"Random 12-char string: {randomStr}");

// Generate a random string from custom character buckets
string customRandom = Generate.RandomString(8, "ABCDEF", "123456");
Console.WriteLine($"Custom random string: {customRandom}");

// Generate a fixed string of repeated characters
string separator = Generate.FixedString('-', 20);
Console.WriteLine(separator); // --------------------

// Compute hash codes from convertible values
int hash32 = Generate.HashCode32(42, "hello", true);
long hash64 = Generate.HashCode64(42, "hello", true);
Console.WriteLine($"32-bit hash: {hash32}, 64-bit hash: {hash64}");

// Generate a structured portrayal of an object
var person = new { Name = "Alice", Age = 30 };
string portrayal = Generate.ObjectPortrayal(person);
Console.WriteLine(portrayal); // shows property names and values
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="531c943d7d54e5591617bcd3"><a href="~/run-actions/531c943d7d54e5591617bcd3/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/531c943d7d54e5591617bcd3/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
