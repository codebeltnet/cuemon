---
uid: Cuemon.Threading.TimerFactory
example:
- *content
---

```csharp
using System;
using System.Threading;
using Cuemon.Threading;

namespace Cuemon.Threading;

public class TimerFactoryExample
{
    public void Demonstrate()
    {
        using var timer = TimerFactory.CreateNonCapturingTimer(
            state => Console.WriteLine("Timer ticked"),
            null,
            TimeSpan.FromSeconds(1),
            TimeSpan.FromSeconds(2));

        Console.WriteLine("Timer started (1s delay, 2s interval)");
        Thread.Sleep(5000);
    }
}
```
