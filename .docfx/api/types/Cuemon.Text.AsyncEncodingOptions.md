---
uid: Cuemon.Text.AsyncEncodingOptions
example:
- *content
---

The following example demonstrates how to configure `AsyncEncodingOptions` with a cancellation token for use in asynchronous encoding operations.

```csharp
using System.Threading.Tasks;
using System.Text;
using System.Threading;
using Cuemon.Text;

namespace MyApp.Examples;

public class AsyncEncodingOptionsExample
{
    public void Demonstrate()
    {
        using var cts = new CancellationTokenSource();
        var options = new AsyncEncodingOptions
        {
            Encoding = Encoding.UTF8,
            Preamble = PreambleSequence.Remove,
            CancellationToken = cts.Token
        };

        // The options can be passed to async encoding methods
        // that accept AsyncEncodingOptions. If cancellation is
        // requested, the operation will be cancelled gracefully.

}
}

```
