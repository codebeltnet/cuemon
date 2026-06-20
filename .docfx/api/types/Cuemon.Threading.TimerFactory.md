---
uid: Cuemon.Threading.TimerFactory
example:
- *content
---

The following example creates a non-capturing timer that fires every two seconds after an initial one-second delay. The callback prints "Timer ticked" each time it executes; the timer is disposed after five seconds.

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
