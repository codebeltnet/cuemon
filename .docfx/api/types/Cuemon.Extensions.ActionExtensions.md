---
uid: Cuemon.Extensions.ActionExtensions
example:
- *content
---

The following example demonstrates applying the options pattern and factory initialization using the [Configure](https://docs.cuemon.net/api/extensions/dotnet/Cuemon.Extensions.ActionExtensions.html#Cuemon_Extensions_ActionExtensions_Configure__1_System_Action___0__) and [CreateInstance](https://docs.cuemon.net/api/extensions/dotnet/Cuemon.Extensions.ActionExtensions.html#Cuemon_Extensions_ActionExtensions_CreateInstance__1_System_Action___0__) extension methods.

```csharp
using System;
using Cuemon.Configuration;
using Cuemon.Extensions;

namespace MyApp.Examples;

public static class ActionExtensionsExample
{
    private sealed class MyOptions : IParameterObject
    {
        public string Delimiter { get; set; } = ",";

        public string Qualifier { get; set; } = "\"";
    }

    private sealed class MyService
    {
        public string ConnectionString { get; set; } = string.Empty;

        public int Timeout { get; set; } = 30;
    }

    public static void Demonstrate()
    {
        var options = new Action<MyOptions>(setup =>
        {
            setup.Delimiter = ";";
            setup.Qualifier = "'";
        }).Configure();

        var service = new Action<MyService>(factory =>
        {
            factory.ConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=Docs";
            factory.Timeout = 60;
        }).CreateInstance();

        Console.WriteLine($"{options.Delimiter} {options.Qualifier}");
        Console.WriteLine($"{service.ConnectionString} ({service.Timeout}s)");
    }
}

```
