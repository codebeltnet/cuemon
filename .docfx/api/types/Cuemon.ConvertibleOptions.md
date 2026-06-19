---
uid: Cuemon.ConvertibleOptions
example:
- *content
---

The following example demonstrates how to use <see cref="ConvertibleOptions"/> to configure byte-order (endianness) and custom converters for the <see cref="Convertible"/> class.

```csharp
using System;
using Cuemon; // for ConvertibleOptions, Convertible, ConvertibleConverterDictionary, Endianness

namespace MyApp.Examples;

public class ConvertibleOptionsExample
{
    public void Demonstrate()
    {
        // Create options with default endianness (system-dependent)
        var options = new ConvertibleOptions();
        Console.WriteLine($"Default byte order: {options.ByteOrder}");
        // Output (on x64): LittleEndian

        // Configure for big-endian byte order
        options.ByteOrder = Endianness.BigEndian;
        Console.WriteLine($"Byte order: {options.ByteOrder}"); // BigEndian

        // Register a custom converter for your own IConvertible type
        options.Converters.Add<MyValue>(value =>
        {
            // Convert MyValue to bytes according to the configured byte order
            byte[] bytes = BitConverter.GetBytes(value.Amount);
            if (options.ByteOrder == Endianness.BigEndian && BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            return bytes;
        });

        // Use Convertible.GetBytes with the configured options
        MyValue myValue = new MyValue { Amount = 12345 };
        byte[] result = Convertible.GetBytes(myValue, o =>
        {
            o.ByteOrder = options.ByteOrder;
        });

        Console.WriteLine($"Converted bytes: {BitConverter.ToString(result)}");
    }
}

public struct MyValue : IConvertible
{
    public int Amount { get; set; }

    // Minimal IConvertible implementation for illustration
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
    public string ToString(IFormatProvider provider) => Amount.ToString();
    public object ToType(Type conversionType, IFormatProvider provider) => Convert.ChangeType(Amount, conversionType);
    public ushort ToUInt16(IFormatProvider provider) => Convert.ToUInt16(Amount);
    public uint ToUInt32(IFormatProvider provider) => Convert.ToUInt32(Amount);
    public ulong ToUInt64(IFormatProvider provider) => Convert.ToUInt64(Amount);
}
```
