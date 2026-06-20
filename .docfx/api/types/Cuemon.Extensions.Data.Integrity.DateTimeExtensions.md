---
uid: Cuemon.Extensions.Data.Integrity.DateTimeExtensions
example:
- *content
---

The following example demonstrates generating a <see cref="CacheValidator"/> from timestamp values using the <xref:Cuemon.Extensions.Data.Integrity.DateTimeExtensions.GetCacheValidator(System.DateTime,System.Nullable{System.DateTime},System.Func{Cuemon.Security.Hash},Cuemon.Data.Integrity.EntityDataIntegrityMethod)> and <xref:Cuemon.Extensions.Data.Integrity.DateTimeExtensions.GetCacheValidator(System.DateTime,System.DateTime,System.Byte[],Cuemon.Data.Integrity.EntityDataIntegrityValidation,System.Func{Cuemon.Security.Hash},Cuemon.Data.Integrity.EntityDataIntegrityMethod)> extension methods.

```csharp
using System;
using Cuemon.Data.Integrity;
using Cuemon.Extensions.Data.Integrity;
using Cuemon.Security;

namespace MyApp.Examples;

public class DateTimeExtensionsExample
{
    public static void Main()
    {
        // Define timestamps for when data was created and last modified
        DateTime created = new DateTime(2025, 1, 15, 10, 0, 0, DateTimeKind.Utc);
        DateTime modified = new DateTime(2025, 6, 10, 14, 30, 0, DateTimeKind.Utc);

        // Basic timestamp-only validator (weak integrity based on timestamps)
        CacheValidator timestampValidator = created.GetCacheValidator(modified);
        Console.WriteLine($"Created (UTC): {timestampValidator.Created:O}");
        Console.WriteLine($"Modified (UTC): {timestampValidator.Modified:O}");
        Console.WriteLine($"Checksum (hex): {timestampValidator.Checksum.ToHexadecimalString()}");

        // Create a validator using the Timestamp method
        // (checksum is derived purely from timestamps)
        CacheValidator timeBasedValidator = created.GetCacheValidator(modified,
            hashFactory: () => HashFactory.CreateFnv128(),
            method: EntityDataIntegrityMethod.Timestamp);
        Console.WriteLine($"Time-based method checksum: {timeBasedValidator.Checksum.ToHexadecimalString()}");

        // Create a validator with both timestamps and a content checksum
        byte[] contentChecksum = HashFactory.CreateFnv128().ComputeHash("content-data").GetBytes();
        CacheValidator strongValidator = created.GetCacheValidator(modified, contentChecksum,
            validation: EntityDataIntegrityValidation.Strong);
        Console.WriteLine($"Strong validator checksum: {strongValidator.Checksum.ToHexadecimalString()}");
        Console.WriteLine($"Validation level: {strongValidator.Validation}");

        // Create a validator with only the created timestamp (no modified date)
        CacheValidator createdOnly = created.GetCacheValidator();
        Console.WriteLine($"Created-only checksum: {createdOnly.Checksum.ToHexadecimalString()}");

}
}

```
