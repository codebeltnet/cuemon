---
uid: Cuemon.Extensions.AspNetCore.Mvc.CacheableObjectResultExtensions
example:
- *content
---

The following example mirrors the cacheable object patterns covered by the unit tests: a payload can expose Last-Modified metadata, an ETag, or both at once. It calls `WithLastModifiedHeader`, `WithEntityTagHeader`, and `WithCacheableHeaders` in sequence on a `ProductDto`, each with timestamp and checksum provider callbacks. The resulting `ICacheableObjectResult` is read back to display the modified timestamp and entity-tag validation, showing how to attach HTTP caching headers to response payloads.

```csharp
using System;
using System.Text;
using Cuemon.AspNetCore.Mvc;
using Cuemon.Data.Integrity;
using Cuemon.Extensions.AspNetCore.Mvc;

namespace Cuemon.Extensions.AspNetCore.Mvc.DocExamples;

public sealed class CacheableObjectResultExample
{
    public ICacheableObjectResult CreateLastModifiedResult()
    {
        var product = new ProductDto(42, "Coffee Beans");
        return product.WithLastModifiedHeader(options =>
        {
            options.TimestampProvider = _ => new DateTime(2024, 6, 1, 8, 0, 0, DateTimeKind.Utc);
            options.ChangedTimestampProvider = _ => new DateTime(2024, 6, 18, 8, 30, 0, DateTimeKind.Utc);
        });
    }

    public ICacheableObjectResult CreateEntityTagResult()
    {
        var product = new ProductDto(42, "Coffee Beans");
        return product.WithEntityTagHeader(options =>
        {
            options.ChecksumProvider = dto => Encoding.UTF8.GetBytes($"{dto.Id}:{dto.Name}");
            options.WeakChecksumProvider = _ => false;
        });
    }

    public ICacheableObjectResult CreateFullyCacheableResult()
    {
        var product = new ProductDto(42, "Coffee Beans");
        return product.WithCacheableHeaders(options =>
        {
            options.TimestampProvider = _ => new DateTime(2024, 6, 1, 8, 0, 0, DateTimeKind.Utc);
            options.ChangedTimestampProvider = _ => new DateTime(2024, 6, 18, 8, 30, 0, DateTimeKind.Utc);
            options.ChecksumProvider = dto => Encoding.UTF8.GetBytes($"{dto.Id}:{dto.Name}");
            options.WeakChecksumProvider = _ => false;
        });
    }

    public void Describe()
    {
        var result = CreateFullyCacheableResult();
        var timestamp = (IEntityDataTimestamp)result;
        var integrity = (IEntityDataIntegrity)result;
        var product = (ProductDto)result.Value;

        Console.WriteLine($"{product.Name}: {(timestamp.Modified ?? timestamp.Created):O} [{integrity.Validation}]");
    }
}

public sealed class ProductDto
{
    public ProductDto(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; }

    public string Name { get; }
}
```
