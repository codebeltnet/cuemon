---
uid: Cuemon.AspNetCore.Authentication.Digest.DigestFields
example:
- *content
---

The following example demonstrates how to use the field constants of `DigestFields` when building a Digest access authorization header.

```csharp
// Program.cs
using System;
using Cuemon.AspNetCore.Authentication.Digest;

var builder = new DigestAuthorizationHeaderBuilder(DigestCryptoAlgorithm.Sha256);
builder.AddRealm("my-realm");
builder.AddUserName("alice");
builder.AddUri("/api/resource");
builder.AddNc(1);
builder.AddCnonce(Guid.NewGuid().ToString("N"));

Console.WriteLine(DigestFields.Realm);
Console.WriteLine(DigestFields.Nonce);
Console.WriteLine(DigestFields.QualityOfProtection);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="e99519768547ebea5218730b"><a href="~/run-actions/e99519768547ebea5218730b/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/e99519768547ebea5218730b/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
