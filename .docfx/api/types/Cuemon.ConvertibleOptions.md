---
uid: Cuemon.ConvertibleOptions
example:
- *content
---

The following example demonstrates how to use <see cref="ConvertibleOptions"/> to configure byte-order (endianness) and custom converters for the <see cref="Convertible"/> class.

```csharp
// Program.cs
using System;
using Cuemon;
using MyApp.Examples;

new ConvertibleOptionsExample().Demonstrate();

namespace MyApp.Examples
{
    public class ConvertibleOptionsExample
    {
        public void Demonstrate()
        {
            var options = new ConvertibleOptions();
            Console.WriteLine($"Default byte order: {options.ByteOrder}");

            options.ByteOrder = Endianness.BigEndian;
            Console.WriteLine($"Byte order: {options.ByteOrder}");

            MyValue myValue = new MyValue { Amount = 12345 };
            byte[] result = Convertible.GetBytes(myValue, o =>
            {
                o.ByteOrder = Endianness.BigEndian;
                o.Converters.Add<MyValue>(value =>
                {
                    byte[] bytes = BitConverter.GetBytes(value.Amount);
                    if (o.ByteOrder == Endianness.BigEndian && BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(bytes);
                    }
                    return bytes;
                });
            });

            Console.WriteLine($"Converted bytes: {BitConverter.ToString(result)}");
        }
    }
    public struct MyValue : IConvertible
    {
        public int Amount { get; set; }

        public TypeCode GetTypeCode() => TypeCode.Object;
        public bool ToBoolean(IFormatProvider provider) => Convert.ToBoolean(Amount);
        public byte ToByte(IFormatProvider provider) => Convert.ToByte(Amount);
        public char ToChar(IFormatProvider provider) => Convert.ToChar(Amount);
        public DateTime ToDateTime(IFormatProvider provider) => Convert.ToDateTime(Amount);
        public decimal ToDecimal(IFormatProvider provider) => Amount;
        public double ToDouble(IFormatProvider provider) => Amount;
        public short ToInt16(IFormatProvider provider) => Convert.ToInt16(Amount);
        public int ToInt32(IFormatProvider provider) => Amount;
        public long ToInt64(IFormatProvider provider) => Amount;
        public sbyte ToSByte(IFormatProvider provider) => Convert.ToSByte(Amount);
        public float ToSingle(IFormatProvider provider) => Amount;
        public string ToString(IFormatProvider provider) => Amount.ToString(provider);
        public object ToType(Type conversionType, IFormatProvider provider) => Convert.ChangeType(Amount, conversionType, provider);
        public ushort ToUInt16(IFormatProvider provider) => Convert.ToUInt16(Amount, provider);
        public uint ToUInt32(IFormatProvider provider) => Convert.ToUInt32(Amount, provider);
        public ulong ToUInt64(IFormatProvider provider) => Convert.ToUInt64(Amount, provider);
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="4a4e45791b3d4022e1c1967e"><a href="~/run-actions/4a4e45791b3d4022e1c1967e/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/4a4e45791b3d4022e1c1967e/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
