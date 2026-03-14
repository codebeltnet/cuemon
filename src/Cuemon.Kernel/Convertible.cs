using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using Cuemon.Text;

namespace Cuemon
{
    /// <summary>
    /// Provides static helper methods for converting <see cref="IConvertible"/> values to and from byte arrays, including support for configurable encoding, byte order, and custom converters.
    /// </summary>
    public static class Convertible
    {
        private static readonly Dictionary<Type, Func<IConvertible, Action<EndianOptions>, byte[]>> EndianSensitiveByteArrayConverters = new()
        {
            { typeof(bool), (i, o) => i is bool x ? GetBytes(x, o) : null },
            { typeof(byte), (i, o) => i is byte x ? GetBytes(x, o) : null },
            { typeof(char), (i, o) => i is char x ? GetBytes(x, o) : null },
            { typeof(double), (i, o) => i is double x ? GetBytes(x, o) : null },
            { typeof(short), (i, o) => i is short x ? GetBytes(x, o) : null },
            { typeof(int), (i, o) => i is int x ? GetBytes(x, o) : null },
            { typeof(long), (i, o) => i is long x ? GetBytes(x, o) : null },
            { typeof(sbyte), (i, o) => i is sbyte x ? GetBytes(x, o) : null },
            { typeof(float), (i, o) => i is float x ? GetBytes(x, o) : null },
            { typeof(ushort), (i, o) => i is ushort x ? GetBytes(x, o) : null },
            { typeof(uint), (i, o) => i is uint x ? GetBytes(x, o) : null },
            { typeof(ulong), (i, o) => i is ulong x ? GetBytes(x, o) : null },
            { typeof(Enum), (i, o) => i is Enum x ? GetBytes(x, o) : null }
        };

        private static readonly Dictionary<Type, Func<IConvertible, byte[]>> ByteArrayConverters = new()
        {
            { typeof(string), input => input is string x ? GetBytes(x) : null },
            { typeof(DateTime), input => input is DateTime x ? GetBytes(x) : null },
            { typeof(decimal), input => input is decimal x ? GetBytes(x) : null },
            { typeof(DBNull), input => input is DBNull x ? GetBytes(x) : null }
        };

        /// <summary>
        /// Represents a null value when converting to a byte array.
        /// </summary>
        public const int NullValue = 0;

        /// <summary>
        /// Represents the number of bits in a byte.
        /// </summary>
        public const int BitsPerByte = 8;

        /// <summary>
        /// Represents the number of bits in a nibble.
        /// </summary>
        public const int BitsPerNibble = BitsPerByte / 2;

        /// <summary>
        /// Registers a custom converter for the specified <see cref="IConvertible"/> implementation.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IConvertible"/> implementation to register.</typeparam>
        /// <param name="converter">The delegate that converts an instance of <typeparamref name="T"/> to a byte array.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="converter"/> is <see langword="null"/>.
        /// </exception>
        public static void RegisterConvertible<T>(Func<T, byte[]> converter) where T : IConvertible
        {
            Validator.ThrowIfNull(converter);
            ByteArrayConverters.Add(typeof(T), convertible => converter((T)convertible));
        }

        /// <summary>
        /// Reverses the bit order of the specified 8-bit unsigned integer.
        /// </summary>
        /// <param name="input">The value whose bits to reverse.</param>
        /// <returns>A <see cref="byte"/> whose bits are reversed.</returns>
        public static byte ReverseBits8(byte input)
        {
            return (byte)ReverseBits(input, sizeof(byte));
        }

        /// <summary>
        /// Reverses the bit order of the specified 16-bit unsigned integer.
        /// </summary>
        /// <param name="input">The value whose bits to reverse.</param>
        /// <returns>A <see cref="ushort"/> whose bits are reversed.</returns>
        public static ushort ReverseBits16(ushort input)
        {
            return (ushort)ReverseBits(input, sizeof(ushort));
        }

        /// <summary>
        /// Reverses the bit order of the specified 32-bit unsigned integer.
        /// </summary>
        /// <param name="input">The value whose bits to reverse.</param>
        /// <returns>A <see cref="uint"/> whose bits are reversed.</returns>
        public static uint ReverseBits32(uint input)
        {
            return (uint)ReverseBits(input, sizeof(uint));
        }

        /// <summary>
        /// Reverses the bit order of the specified 64-bit unsigned integer.
        /// </summary>
        /// <param name="input">The value whose bits to reverse.</param>
        /// <returns>A <see cref="ulong"/> whose bits are reversed.</returns>
        public static ulong ReverseBits64(ulong input)
        {
            return ReverseBits(input, sizeof(ulong));
        }

        private static ulong ReverseBits(ulong input, byte byteSize)
        {
            var bitSize = byteSize * BitsPerByte;
            ulong output = 0;
            for (var i = 0; i < bitSize; i++)
            {
                if ((input & ((ulong)1 << i)) != 0) { output |= (ulong)1 << ((bitSize - 1) - i); }
            }
            return output;
        }

        /// <summary>
        /// Converts the specified <see cref="IConvertible"/> value to its byte array representation.
        /// </summary>
        /// <param name="input">The value to convert.</param>
        /// <param name="setup">The delegate that configures the conversion options.</param>
        /// <returns>A byte array that represents <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="input"/> is of a type for which no converter has been registered or configured.
        /// </exception>
        /// <remarks>
        /// Returns the byte representation of <see cref="NullValue"/> when <paramref name="input"/> is <see langword="null"/>.
        /// Custom converters may be registered globally with <see cref="RegisterConvertible{T}(Func{T, byte[]})"/> or supplied
        /// locally through <see cref="ConvertibleOptions.Converters"/>.
        /// </remarks>
        public static byte[] GetBytes(IConvertible input, Action<ConvertibleOptions> setup = null)
        {
            if (input == null) { return BitConverter.GetBytes(NullValue); }
            var options = Patterns.Configure(setup);

            if (options.Converters.Count > 0)
            {
                var localConverter = options.Converters[input.GetType()];
                if (localConverter != null) { return localConverter(input); }
            }

            if (input.GetType().IsPrimitive || input is Enum)
            {
                foreach (var item in EndianSensitiveByteArrayConverters)
                {
                    var bytes = item.Value(input, o => o.ByteOrder = options.ByteOrder);
                    if (bytes != null) { return bytes; }
                }
            }

            foreach (var item in ByteArrayConverters)
            {
                var bytes = item.Value(input);
                if (bytes != null) { return bytes; }
            }

            throw new ArgumentOutOfRangeException(nameof(input), input, $"Unknown implementation of IConvertible; please use {nameof(RegisterConvertible)} to make a custom implementation globally known -or- use {nameof(setup)} to add a custom implementation using {nameof(ConvertibleOptions)}.{nameof(ConvertibleOptions.Converters)}.{nameof(ConvertibleOptions.Converters.Add)}.");
        }

        /// <summary>
        /// Converts the specified sequence of <see cref="IConvertible"/> values to a single aggregated byte array.
        /// </summary>
        /// <param name="input">The sequence of values to convert.</param>
        /// <param name="setup">The delegate that configures the conversion options.</param>
        /// <returns>
        /// A byte array containing the concatenated byte representations of the elements in <paramref name="input"/>.
        /// </returns>
        public static byte[] GetBytes(IEnumerable<IConvertible> input, Action<ConvertibleOptions> setup = null)
        {
            var result = new List<byte>();
            foreach (var type in input)
            {
                var bytes = GetBytes(type, setup);
                if (bytes != null) { result.AddRange(bytes); }
            }
            return result.ToArray();
        }

        /// <summary>
        /// Converts the specified <see cref="bool"/> value to its byte array representation.
        /// </summary>
        /// <param name="input">The value to convert.</param>
        /// <param name="setup">The delegate that configures the byte order.</param>
        /// <returns>A byte array that represents <paramref name="input"/>.</returns>
        public static byte[] GetBytes(bool input, Action<EndianOptions> setup = null)
        {
            return GetBytesCore(input, BitConverter.GetBytes, setup);
        }

        /// <summary>
        /// Converts the specified <see cref="byte"/> value to its byte array representation.
        /// </summary>
        /// <param name="input">The value to convert.</param>
        /// <param name="setup">The delegate that configures the byte order.</param>
        /// <returns>A byte array that represents <paramref name="input"/>.</returns>
        public static byte[] GetBytes(byte input, Action<EndianOptions> setup = null)
        {
            return GetBytesCore(input, x => new[] { x }, setup);
        }

        /// <summary>
        /// Converts the specified <see cref="char"/> value to its byte array representation.
        /// </summary>
        /// <param name="input">The value to convert.</param>
        /// <param name="setup">The delegate that configures the byte order.</param>
        /// <returns>A byte array that represents <paramref name="input"/>.</returns>
        public static byte[] GetBytes(char input, Action<EndianOptions> setup = null)
        {
            return GetBytesCore(input, BitConverter.GetBytes, setup);
        }

        /// <summary>
        /// Converts the specified <see cref="DateTime"/> value to its byte array representation.
        /// </summary>
        /// <param name="input">The value to convert.</param>
        /// <returns>A byte array that represents <paramref name="input"/>.</returns>
        /// <remarks>
        /// The value is formatted using the universal sortable date and time pattern and encoded with ASCII.
        /// </remarks>
        public static byte[] GetBytes(DateTime input)
        {
            return GetBytes(input.ToString("u", CultureInfo.InvariantCulture), o => o.Encoding = Encoding.ASCII);
        }

        /// <summary>
        /// Converts the specified <see cref="DBNull"/> value to its byte array representation.
        /// </summary>
        /// <param name="input">The value to convert.</param>
        /// <returns>A byte array representing <see cref="NullValue"/>.</returns>
        public static byte[] GetBytes(DBNull input)
        {
            return GetBytesCore(input, _ => BitConverter.GetBytes(NullValue), null);
        }

        /// <summary>
        /// Converts the specified <see cref="decimal"/> value to its byte array representation.
        /// </summary>
        /// <param name="input">The value to convert.</param>
        /// <returns>A byte array that represents <paramref name="input"/>.</returns>
        /// <remarks>
        /// The value is formatted using the invariant culture and encoded with ASCII.
        /// </remarks>
        public static byte[] GetBytes(decimal input)
        {
            return GetBytes(input.ToString(CultureInfo.InvariantCulture), o => o.Encoding = Encoding.ASCII);
        }

        /// <summary>
        /// Converts the specified <see cref="double"/> value to its byte array representation.
        /// </summary>
        /// <param name="input">The value to convert.</param>
        /// <param name="setup">The delegate that configures the byte order.</param>
        /// <returns>A byte array that represents <paramref name="input"/>.</returns>
        public static byte[] GetBytes(double input, Action<EndianOptions> setup = null)
        {
            return GetBytesCore(input, BitConverter.GetBytes, setup);
        }

        /// <summary>
        /// Converts the specified <see cref="short"/> value to its byte array representation.
        /// </summary>
        /// <param name="input">The value to convert.</param>
        /// <param name="setup">The delegate that configures the byte order.</param>
        /// <returns>A byte array that represents <paramref name="input"/>.</returns>
        public static byte[] GetBytes(short input, Action<EndianOptions> setup = null)
        {
            return GetBytesCore(input, BitConverter.GetBytes, setup);
        }

        /// <summary>
        /// Converts the specified <see cref="int"/> value to its byte array representation.
        /// </summary>
        /// <param name="input">The value to convert.</param>
        /// <param name="setup">The delegate that configures the byte order.</param>
        /// <returns>A byte array that represents <paramref name="input"/>.</returns>
        public static byte[] GetBytes(int input, Action<EndianOptions> setup = null)
        {
            return GetBytesCore(input, BitConverter.GetBytes, setup);
        }

        /// <summary>
        /// Converts the specified <see cref="long"/> value to its byte array representation.
        /// </summary>
        /// <param name="input">The value to convert.</param>
        /// <param name="setup">The delegate that configures the byte order.</param>
        /// <returns>A byte array that represents <paramref name="input"/>.</returns>
        public static byte[] GetBytes(long input, Action<EndianOptions> setup = null)
        {
            return GetBytesCore(input, BitConverter.GetBytes, setup);
        }

        /// <summary>
        /// Converts the specified <see cref="sbyte"/> value to its byte array representation.
        /// </summary>
        /// <param name="input">The value to convert.</param>
        /// <param name="setup">The delegate that configures the byte order.</param>
        /// <returns>A byte array that represents <paramref name="input"/>.</returns>
        public static byte[] GetBytes(sbyte input, Action<EndianOptions> setup = null)
        {
#if NET10_0_OR_GREATER
            return GetBytesCore(input, x => BitConverter.GetBytes((short)x), setup);
#else
            return GetBytesCore(input, x => BitConverter.GetBytes(x), setup);
#endif
        }

        /// <summary>
        /// Converts the specified <see cref="float"/> value to its byte array representation.
        /// </summary>
        /// <param name="input">The value to convert.</param>
        /// <param name="setup">The delegate that configures the byte order.</param>
        /// <returns>A byte array that represents <paramref name="input"/>.</returns>
        public static byte[] GetBytes(float input, Action<EndianOptions> setup = null)
        {
            return GetBytesCore(input, BitConverter.GetBytes, setup);
        }

        /// <summary>
        /// Converts the specified <see cref="ushort"/> value to its byte array representation.
        /// </summary>
        /// <param name="input">The value to convert.</param>
        /// <param name="setup">The delegate that configures the byte order.</param>
        /// <returns>A byte array that represents <paramref name="input"/>.</returns>
        public static byte[] GetBytes(ushort input, Action<EndianOptions> setup = null)
        {
            return GetBytesCore(input, BitConverter.GetBytes, setup);
        }

        /// <summary>
        /// Converts the specified <see cref="uint"/> value to its byte array representation.
        /// </summary>
        /// <param name="input">The value to convert.</param>
        /// <param name="setup">The delegate that configures the byte order.</param>
        /// <returns>A byte array that represents <paramref name="input"/>.</returns>
        public static byte[] GetBytes(uint input, Action<EndianOptions> setup = null)
        {
            return GetBytesCore(input, BitConverter.GetBytes, setup);
        }

        /// <summary>
        /// Converts the specified <see cref="ulong"/> value to its byte array representation.
        /// </summary>
        /// <param name="input">The value to convert.</param>
        /// <param name="setup">The delegate that configures the byte order.</param>
        /// <returns>A byte array that represents <paramref name="input"/>.</returns>
        public static byte[] GetBytes(ulong input, Action<EndianOptions> setup = null)
        {
            return GetBytesCore(input, BitConverter.GetBytes, setup);
        }

        /// <summary>
        /// Converts the specified <see cref="string"/> value to its byte array representation.
        /// </summary>
        /// <param name="input">The value to convert.</param>
        /// <param name="setup">The delegate that configures the encoding behavior.</param>
        /// <returns>A byte array that represents <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="setup"/> configures an invalid value for <see cref="EncodingOptions.Preamble"/>.
        /// </exception>
        /// <remarks>
        /// <see cref="EncodingOptions"/> is initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and
        /// <see cref="EncodingOptions.DefaultEncoding"/>.
        /// </remarks>
        public static byte[] GetBytes(string input, Action<EncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(input);
            var options = Patterns.Configure(setup);
            byte[] valueInBytes;
            switch (options.Preamble)
            {
                case PreambleSequence.Keep:
                    valueInBytes = options.Encoding.GetPreamble().Concat(options.Encoding.GetBytes(input)).ToArray();
                    break;
                case PreambleSequence.Remove:
                    valueInBytes = options.Encoding.GetBytes(input);
                    break;
                default:
                    throw new InvalidEnumArgumentException(nameof(setup), (int)options.Preamble, typeof(PreambleSequence));
            }
            return valueInBytes;
        }

        /// <summary>
        /// Converts the specified <see cref="Enum"/> value to its underlying byte array representation.
        /// </summary>
        /// <param name="input">The value to convert.</param>
        /// <param name="setup">The delegate that configures the byte order.</param>
        /// <returns>A byte array that represents the underlying numeric value of <paramref name="input"/>.</returns>
        public static byte[] GetBytes(Enum input, Action<EndianOptions> setup = null)
        {
            var tc = input.GetTypeCode();
            var c = input as IConvertible;
            switch (tc)
            {
                case TypeCode.Byte:
                    {
                        return GetBytes(c.ToByte(CultureInfo.InvariantCulture), setup);
                    }
                case TypeCode.Int16:
                    {
                        return GetBytes(c.ToInt16(CultureInfo.InvariantCulture), setup);
                    }
                case TypeCode.Int64:
                    {
                        return GetBytes(c.ToInt64(CultureInfo.InvariantCulture), setup);
                    }
                case TypeCode.UInt16:
                    {
                        return GetBytes(c.ToUInt16(CultureInfo.InvariantCulture), setup);
                    }
                case TypeCode.UInt32:
                    {
                        return GetBytes(c.ToUInt32(CultureInfo.InvariantCulture), setup);
                    }
                case TypeCode.UInt64:
                    {
                        return GetBytes(c.ToUInt64(CultureInfo.InvariantCulture), setup);
                    }
                case TypeCode.SByte:
                    {
                        return GetBytes(c.ToSByte(CultureInfo.InvariantCulture), setup);
                    }
                default:
                    return GetBytes(c.ToInt32(CultureInfo.InvariantCulture), setup);
            }
        }

        /// <summary>
        /// Converts the specified byte array to its string representation.
        /// </summary>
        /// <param name="input">The byte array to convert.</param>
        /// <param name="setup">The delegate that configures the encoding behavior.</param>
        /// <returns>A string that represents <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="setup"/> configures an invalid value for <see cref="EncodingOptions.Preamble"/>.
        /// </exception>
        /// <remarks>
        /// <see cref="EncodingOptions"/> is initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and
        /// <see cref="EncodingOptions.DefaultEncoding"/>.
        /// If the configured encoding is the default encoding, the encoding is detected from the byte order mark when possible.
        /// </remarks>
        public static string ToString(byte[] input, Action<EncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(input);
            var options = Patterns.Configure(setup);
            if (options.Encoding.Equals(EncodingOptions.DefaultEncoding)) { options.Encoding = ByteOrderMark.DetectEncodingOrDefault(input, options.Encoding); }
            switch (options.Preamble)
            {
                case PreambleSequence.Keep:
                    break;
                case PreambleSequence.Remove:
                    input = ByteOrderMark.Remove(input, options.Encoding);
                    break;
                default:
                    throw new InvalidEnumArgumentException(nameof(setup), (int)options.Preamble, typeof(PreambleSequence));
            }
            return options.Encoding.GetString(input, 0, input.Length);
        }

        /// <summary>
        /// Reverses the byte order of the specified byte array when required by the configured endianness.
        /// </summary>
        /// <param name="input">The byte array whose byte order to reverse.</param>
        /// <param name="setup">The delegate that configures the desired byte order.</param>
        /// <returns>
        /// <paramref name="input"/>, either unchanged or reversed to match the configured byte order.
        /// </returns>
        public static byte[] ReverseEndianness(byte[] input, Action<EndianOptions> setup = null)
        {
            var options = Patterns.Configure(setup);
            switch (options.ByteOrder)
            {
                case Endianness.BigEndian:
                    if (BitConverter.IsLittleEndian) { Array.Reverse(input); }
                    break;
                default:
                    if (!BitConverter.IsLittleEndian) { Array.Reverse(input); }
                    break;
            }
            return input;
        }

        private static byte[] GetBytesCore<T>(T input, Func<T, byte[]> converter, Action<EndianOptions> setup) where T : IConvertible
        {
            if (TryCast<T>(input, out var result)) { return setup == null ? converter(result) : ReverseEndianness(converter(result), setup); }
            return BitConverter.GetBytes(NullValue);
        }

        private static bool TryCast<T>(IConvertible convertible, out T concrete) where T : IConvertible
        {
            if (convertible is T ct)
            {
                concrete = ct;
                return true;
            }
            concrete = default;
            return false;
        }
    }
}
