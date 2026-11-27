---
uid: Cuemon.Reflection.TypeNameOptions
example:
- *content
---

The following example demonstrates how to use `TypeNameOptions` to control the friendly name output of a `Type`.

```csharp
// Program.cs
using System;
using Cuemon;
using Cuemon.Reflection;

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
Console.WriteLine(friendlyName);

friendlyName = Decorator.Enclose(type).ToFriendlyName(o =>
{
    o.FullName = false;
});
// friendlyName == "Console"
Console.WriteLine(friendlyName);
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="72d7ace2963ab100fc7c4da1"><a href="~/run-actions/72d7ace2963ab100fc7c4da1/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/72d7ace2963ab100fc7c4da1/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
