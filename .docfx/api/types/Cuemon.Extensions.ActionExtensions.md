---
uid: Cuemon.Extensions.ActionExtensions
example:
- *content
---

The following example demonstrates applying the options pattern and factory initialization using the <xref:Cuemon.Extensions.ActionExtensions.Configure``1(System.Action{``0})> and <xref:Cuemon.Extensions.ActionExtensions.CreateInstance``1(System.Action{``0})> extension methods.

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
