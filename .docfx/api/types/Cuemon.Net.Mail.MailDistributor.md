---
uid: Cuemon.Net.Mail.MailDistributor
example:
- *content
---

The following example demonstrates how to construct a <see cref="MailDistributor" /> and skip delivery through a filter when you only want to validate a message batch.

```csharp
// Program.cs
using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Cuemon.Net.Mail;

var distributor = new MailDistributor(() => new SmtpClient("smtp.example.com", 25), deliverySize: 10);
using var mail = new MailMessage("sender@example.com", "receiver@example.com")
{
    Subject = "Docs sample",
    Body = "This message is filtered out before delivery."
};

await distributor.SendOneAsync(mail, _ => false);

Console.WriteLine("Delivery skipped by filter.");
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="8c26a1be0c8796d45dd2f0eb"><a href="~/run-actions/8c26a1be0c8796d45dd2f0eb/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/8c26a1be0c8796d45dd2f0eb/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
