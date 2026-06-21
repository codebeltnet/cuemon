---
uid: Cuemon.AspNetCore.Mvc.CacheableFactory
example:
- *content
---

The following example demonstrates how to create cacheable response objects using `CacheableFactory`.

```csharp
using System;
using System.Security.Cryptography;
using System.Text;
using Cuemon.AspNetCore.Mvc;

namespace MyApp.Examples;

public static class CacheableFactoryExample
{
    public static void Demonstrate()
    {
        var content = "hello-world";

        var lastModified = CacheableFactory.CreateHttpLastModified(content, o =>
        {
            o.TimestampProvider = _ => DateTime.UtcNow;
            o.ChangedTimestampProvider = _ => DateTime.UtcNow;
        });

        var entityTag = CacheableFactory.CreateHttpEntityTag(content, o =>
        {
            o.ChecksumProvider = value => SHA256.HashData(Encoding.UTF8.GetBytes(value));
            o.WeakChecksumProvider = _ => false;
        });

        var combined = CacheableFactory.Create(content, o =>
        {
            o.TimestampProvider = _ => DateTime.UtcNow;
            o.ChecksumProvider = value => SHA256.HashData(Encoding.UTF8.GetBytes(value));
            o.ChangedTimestampProvider = _ => DateTime.UtcNow;
            o.WeakChecksumProvider = _ => false;
        });

        Console.WriteLine(lastModified is ICacheableObjectResult);
        Console.WriteLine(entityTag is ICacheableObjectResult);
        Console.WriteLine(combined is ICacheableObjectResult);
    }
}

```
