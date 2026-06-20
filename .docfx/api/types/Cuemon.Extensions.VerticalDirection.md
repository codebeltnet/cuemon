---
uid: Cuemon.Extensions.VerticalDirection
example:
- *content
---

The following example demonstrates how to use the <xref:Cuemon.Extensions.VerticalDirection> enum to indicate vertical positioning.

```csharp
using System;
using Cuemon.Extensions;

namespace MyApp.Examples;

public static class VerticalDirectionExample
{
    public static void Demonstrate()
    {
        var upward = VerticalDirection.Up;
        var downward = (VerticalDirection)Enum.Parse(typeof(VerticalDirection), "Down");

        Console.WriteLine($"{upward} = {(int)upward}");
        Console.WriteLine($"{downward} = {(int)downward}");
        Console.WriteLine(default(VerticalDirection));
    }
}

```
