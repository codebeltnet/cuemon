---
uid: Cuemon.Threading.Awaiter
example:
- *content
---

The following example retries an asynchronous operation until it succeeds or the retry window closes. The delegate returns `UnsuccessfulValue` twice before returning `SuccessfulValue`, and the configured cancellation token is observed between attempts.

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Cuemon.Threading;

public class AwaiterExample
{
    public async Task DemonstrateAsync()
    {
        var attempt = 0;
        var cancellationSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));

        var result = await Awaiter.RunUntilSuccessfulOrTimeoutAsync(async () =>
        {
            attempt++;
            if (attempt < 3)
            {
                await Task.Delay(50);
                return new UnsuccessfulValue();
            }
            return new SuccessfulValue();
        }, options =>
        {
            options.Timeout = TimeSpan.FromSeconds(5);
            options.Delay = TimeSpan.FromMilliseconds(100);
            options.CancellationToken = cancellationSource.Token;
        });

        Console.WriteLine($"Succeeded after {attempt} attempts: {result.Succeeded}");
    }
}
```
