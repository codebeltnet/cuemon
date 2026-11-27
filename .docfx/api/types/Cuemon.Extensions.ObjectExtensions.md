---
uid: Cuemon.Extensions.ObjectExtensions
example:
- *content
---

`ObjectExtensions` provides extension methods for type conversion with fallback (`As<T>`), wrapper creation (`UseWrapper`), collection manipulation (`Adjust`, `Alter`), and delimited-string formatting. This example converts `"42"` to an `int`, handles `"not-a-number"` with a fallback of `99`, wraps a string with `UseWrapper` to attach metadata, adjusts a `List<int>` by appending elements, and alters it in-place. Key steps include using the `As` method for safe casts, `UseWrapper` for attaching diagnostic data, and `Adjust`/`Alter` for immutable/mutable collection operations. Console output confirms each conversion result, the wrapper data value `"example"`, hash codes, and the delimited string `"1;2;3;4"`.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using Cuemon.Extensions;
using MyApp.Examples;

ObjectExtensionsExample.Demonstrate();

namespace MyApp.Examples
{
    public static class ObjectExtensionsExample
    {
        public static void Demonstrate()
        {
            object raw = "42";
            int number = raw.As(0);
            int fallback = "not-a-number".As(99);
            object nullValue = null;
            int safe = nullValue.As(0);
            string upper = "hello".As(value => value.ToUpperInvariant());
            object converted = raw.As(typeof(int));

            var wrapped = "docs".UseWrapper(extender =>
            {
                extender["source"] = "example";
            });

            var memberWrapped = 42.UseWrapper(typeof(int).GetMethod(nameof(ToString), Type.EmptyTypes), data =>
            {
                data["category"] = "number";
            });

            var numbers = new List<int> { 1, 2, 3 };
            var adjusted = numbers.Adjust(list =>
            {
                var copy = new List<int>(list);
                copy.Add(4);
                return copy;
            });
            var altered = numbers.Alter(list => list.Add(4));
            string delimited = adjusted.ToDelimitedString(options => options.Delimiter = ";");

            Console.WriteLine(number);
            Console.WriteLine(fallback);
            Console.WriteLine(safe);
            Console.WriteLine(upper);
            Console.WriteLine(converted);
            Console.WriteLine(wrapped.Data["source"]);
            Console.WriteLine(memberWrapped.MemberReference?.Name);
            Console.WriteLine(adjusted.GetHashCode32());
            Console.WriteLine(altered.GetHashCode64());
            Console.WriteLine(delimited);
            Console.WriteLine(default(int?).IsNullable<int?>());
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="b6a8447a34c2618c33e351ad"><a href="~/run-actions/b6a8447a34c2618c33e351ad/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/b6a8447a34c2618c33e351ad/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
