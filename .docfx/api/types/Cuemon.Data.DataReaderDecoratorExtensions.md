---
uid: Cuemon.Data.DataReaderDecoratorExtensions
example:
- *content
---

`DataReaderDecoratorExtensions` provides extension methods on `Decorator.Enclose` for converting `IDataReader` content into encoded strings or streams, both synchronously and asynchronously. This example creates a single-column DSV data source backed by `DsvDataReader` with three rows of data, then wraps it and calls `ToEncodedString`, `ToEncodedStringAsync`, and `ToStream`. Key steps include repositioning the underlying stream between conversions and using `DsvDataReader` as the input source. Console output shows the string representation of the data and the stream length.

```csharp
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Data;

namespace MyApp.Examples
{
    public class DataReaderDecoratorExtensionsExample
    {
        public static async Task DemonstrateAsync()
        {
            // Create a single-column DSV data source (must have exactly one field)
            var csv = "Value\r\n1001\r\n1002\r\n1003\r\n";
            var bytes = Encoding.UTF8.GetBytes(csv);

            using (var stream = new MemoryStream(bytes))
            using (var reader = new DsvDataReader(new StreamReader(stream)))
            {
                // Convert the single-field IDataReader to an encoded string (sync)
                string result = Decorator.Enclose((IDataReader)reader).ToEncodedString();
                Console.WriteLine(result);

                // Convert the single-field IDataReader to an encoded string (async)
                stream.Position = 0;
                using (var asyncReader = new DsvDataReader(new StreamReader(new MemoryStream(bytes))))
                {
                    string asyncResult = await Decorator.Enclose((IDataReader)asyncReader).ToEncodedStringAsync();
                    Console.WriteLine(asyncResult);

                // Convert the data reader content to a stream
                stream.Position = 0;
                using (var readerAgain = new DsvDataReader(new StreamReader(new MemoryStream(bytes))))
                {
                    Stream dataStream = Decorator.Enclose((IDataReader)readerAgain).ToStream();
                    Console.WriteLine($"Stream length: {dataStream.Length}");

}}}}}
}

```
