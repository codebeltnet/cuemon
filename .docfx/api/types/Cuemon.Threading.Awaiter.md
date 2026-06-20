---
uid: Cuemon.Threading.Awaiter
example:
- *content
---

The following example retries an asynchronous operation until it succeeds or a timeout is reached. The delegate returns `UnsuccessfulValue` twice before returning `SuccessfulValue`, and the output confirms the operation succeeded after three attempts.

```csharp
using System;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Cuemon.Threading;

public class AwaiterExample
{
    public async Task DemonstrateAsync()
    {
        var attempt = 0;
        var result = await Awaiter.RunUntilSuccessfulOrTimeoutAsync(() =>
        {
            attempt++;
            if (attempt < 3)
            {
                return Task.FromResult<ConditionalValue>(new UnsuccessfulValue());
            }
            return Task.FromResult<ConditionalValue>(new SuccessfulValue());
        }, options =>
        {
            options.Timeout = TimeSpan.FromSeconds(5);
            options.Delay = TimeSpan.FromMilliseconds(100);
        });

        Console.WriteLine($"Succeeded after {attempt} attempts: {result.Succeeded}");
    }
}
```
