---
uid: Cuemon.Data.Integrity.EntityDataIntegrityValidation
example:
- *content
---

The following example shows how to create an `EntityInfo` with strong data integrity validation and create a `CacheValidator` that uses it. It demonstrates switching on the validation level and printing the resulting checksum.

```csharp
using System;
using System.Text;
using Cuemon.Data.Integrity;
using Cuemon.Security;

namespace MyApp.Data
{
    public sealed class EntityDataIntegrityValidationExample
    {
        public void Demonstrate()
        {
            var created = new DateTime(2024, 1, 2, 3, 4, 5, DateTimeKind.Utc);
            var modified = created.AddHours(1);
            var entity = new EntityInfo(
                created,
                modified,
                Encoding.UTF8.GetBytes("order-42"),
                EntityDataIntegrityValidation.Strong);

            var validator = new CacheValidator(entity, () => HashFactory.CreateFnv128());

            switch (validator.Validation)
            {
                case EntityDataIntegrityValidation.Unspecified:
                    Console.WriteLine("No checksum strength was supplied.");
                    break;
                case EntityDataIntegrityValidation.Weak:
                    Console.WriteLine("The checksum is semantically valid.");
                    break;
                case EntityDataIntegrityValidation.Strong:
                    Console.WriteLine("The checksum is byte-for-byte strong.");
                    break;
            }

            Console.WriteLine($"Validation: {validator.Validation}");
            Console.WriteLine($"Checksum: {validator.Checksum.ToHexadecimalString()}");
        }
    }
}
```
