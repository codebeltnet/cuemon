---
uid: Cuemon.Net.Mail.MailDistributor
example:
- *content
---

The following example demonstrates how to construct a <see cref="MailDistributor" /> and skip delivery through a filter when you only want to validate a message batch.

```csharp
using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Cuemon.Net.Mail;

namespace MyApp.Examples;

public static class MailDistributorExample
{
    public static async Task DemonstrateAsync()
    {
        var distributor = new MailDistributor(() => new SmtpClient("smtp.example.com", 25), deliverySize: 10);
        using var mail = new MailMessage("sender@example.com", "receiver@example.com")
        {
            Subject = "Docs sample",
            Body = "This message is filtered out before delivery."
        };

        await distributor.SendOneAsync(mail, _ => false);

        Console.WriteLine("Delivery skipped by filter.");
    }
}
```
