---
uid: Cuemon.Reflection.TypeNameOptions
example:
- *content
---

The following example demonstrates how to use `TypeNameOptions` to control the friendly name output of a `Type`.

```csharp
using System;
using Cuemon;
using Cuemon.Reflection;

namespace Examples;

public class TypeNameFormattingExample
{
    public void Demonstrate()
    {
        // Direct instantiation of TypeNameOptions
        var options = new TypeNameOptions
        {
            FullName = true,
            ExcludeGenericArguments = false
        };

        Type type = typeof(Console);

        string friendlyName = Decorator.Enclose(type).ToFriendlyName(o =>
        {
            o.FullName = true;
            o.ExcludeGenericArguments = false;
        });
        // friendlyName == "System.Console"

        friendlyName = Decorator.Enclose(type).ToFriendlyName(o =>
        {
            o.FullName = false;
        });
        // friendlyName == "Console"

}
}

```
