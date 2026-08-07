using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using Cuemon.Configuration;

namespace Cuemon.Text;
/// <summary>
/// Provides access to factory methods that are tailored for parsing operations adhering <see cref="IParser"/> and <see cref="IConfigurableParser{TOptions}"/>.
/// </summary>
public static class ParserFactory
{
    internal static readonly IDictionary<string, UriScheme> StringToUriSchemeLookupTable = new Dictionary<string, UriScheme>(StringComparer.OrdinalIgnoreCase)
    {
        { "file", UriScheme.File },
        { "ftp", UriScheme.Ftp },
        { "gopher", UriScheme.Gopher },
        { "http", UriScheme.Http },
        { "https", UriScheme.Https },
        { "mailto", UriScheme.Mailto },
        { "net.pipe", UriScheme.NetPipe },
        { "net.tcp", UriScheme.NetTcp },
        { "news", UriScheme.News },
        { "nntp", UriScheme.Nntp },
        { "sftp", UriScheme.Sftp }
    };

    /// <summary>
    /// Creates an <see cref="IParser"/> implementation from the specified <paramref name="parser"/>.
    /// </summary>
    /// <param name="parser">The function delegate that does the actual parsing of a <see cref="string"/>.</param>
    /// <returns>An <see cref="IParser"/> implementation.</returns>
    public static IParser CreateParser(Func<string, Type, object> parser)
    {
        Validator.ThrowIfNull(parser);
        return new Parser(parser);
    }

    /// <summary>
    /// Creates an <see cref="IParser{TResult}"/> implementation from the specified <paramref name="parser"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the converted result.</typeparam>
    /// <param name="parser">The function delegate that does the actual parsing of a <see cref="string"/>.</param>
    /// <returns>An <see cref="IParser{TResult}"/> implementation.</returns>
    public static IParser<TResult> CreateParser<TResult>(Func<string, TResult> parser)
    {
        Validator.ThrowIfNull(parser);
        return new Parser<TResult>(parser);
    }

    /// <summary>
    /// Creates an <see cref="IConfigurableParser{TOptions}"/> implementation from the specified <paramref name="parser"/>.
    /// </summary>
    /// <typeparam name="TOptions">The type of the delegate setup.</typeparam>
    /// <param name="parser">The function delegate that does the actual parsing of a <see cref="string"/>.</param>
    /// <returns>An <see cref="IConfigurableParser{TOptions}"/> implementation.</returns>
    public static IConfigurableParser<TOptions> CreateConfigurableParser<TOptions>(Func<string, Type, Action<TOptions>, object> parser) where TOptions : class, IParameterObject, new()
    {
        Validator.ThrowIfNull(parser);
        return new ConfigurableParser<TOptions>(parser);
    }

    /// <summary>
    /// Creates an <see cref="IConfigurableParser{TResult,TOptions}"/> implementation from the specified <paramref name="parser"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the converted result.</typeparam>
    /// <typeparam name="TOptions">The type of the delegate setup.</typeparam>
    /// <param name="parser">The function delegate that does the actual parsing of a <see cref="string"/>.</param>
    /// <returns>An <see cref="IConfigurableParser{TResult,TOptions}"/> implementation.</returns>
    public static IConfigurableParser<TResult, TOptions> CreateConfigurableParser<TResult, TOptions>(Func<string, Action<TOptions>, TResult> parser) where TOptions : class, IParameterObject, new()
    {
        Validator.ThrowIfNull(parser);
        return new ConfigurableParser<TResult, TOptions>(parser);
    }

    /// <summary>
    /// Creates a parser that converts a <see cref="string"/>, represented in base-64 digits, to its equivalent <see cref="byte"/> array.
    /// </summary>
    /// <returns>An implementation of <see cref="IConfigurableParser{T}"/> that produces <see cref="byte"/> arrays.</returns>
    /// <exception cref="ArgumentNullException">
    /// <see cref="string"/> cannot be null.
    /// </exception>
    /// <exception cref="FormatException">
    /// <see cref="string"/> consist of illegal base-64 digits.
    /// </exception>
    public static IParser<byte[]> FromBase64()
    {
        return CreateParser(Convert.FromBase64String);
    }

    /// <summary>
    /// Creates a parser that converts a <see cref="string"/>, represented in binary digits, to its equivalent <see cref="byte"/> array.
    /// </summary>
    /// <returns>An implementation of <see cref="IConfigurableParser{T}"/> that produces <see cref="byte"/> arrays.</returns>
    /// <exception cref="ArgumentNullException">
    /// <see cref="string"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <see cref="string"/> cannot be empty or consist only of white-space characters.
    /// </exception>
    /// <exception cref="FormatException">
    /// <see cref="string"/> must consist only of binary digits.
    /// </exception>
    public static IParser<byte[]> FromBinaryDigits()
    {
        return CreateParser(input =>
        {
            try
            {
                Validator.ThrowIfNullOrWhitespace(input);
                Validator.ThrowIfNotBinaryDigits(input);
                var bytes = new List<byte>();
                for (var i = 0; i < input.Length; i += 8)
                {
                    bytes.Add(Convert.ToByte(input.Substring(i, 8), 2));
                }
                return bytes.ToArray();
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new FormatException(FormattableString.Invariant($"The format of {nameof(input)} must consist only of binary digits."));
            }
        });
    }

    /// <summary>
    /// Creates a parser that converts a <see cref="string"/> to its equivalent <see cref="Guid"/>.
    /// </summary>
    /// <returns>An <see cref="IConfigurableParser{Guid,GuidStringOptions}"/> implementation.</returns>
    /// <exception cref="ArgumentNullException">
    /// <see cref="string"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <see cref="string"/> cannot be empty or consist only of white-space characters.
    /// </exception>
    /// <exception cref="FormatException">
    /// <see cref="string"/> was not recognized to be a GUID.
    /// </exception>
    /// <seealso cref="Guid.Parse(string)"/>
    /// <seealso cref="Guid.ParseExact(string,string)"/>
    public static IConfigurableParser<Guid, GuidStringOptions> FromGuid()
    {
        return CreateConfigurableParser<Guid, GuidStringOptions>((input, setup) =>
        {
            Validator.ThrowIfNullOrWhitespace(input);
            var options = Patterns.Configure(setup);
            if (options.Formats.HasFlag(GuidFormats.Any)) { return Guid.Parse(input); }

            Guid result = Guid.Empty;
            var hasHyphens = input.IndexOf('-') != -1;
            var hasBraces = (input.StartsWith("{", StringComparison.OrdinalIgnoreCase) && input.EndsWith("}", StringComparison.OrdinalIgnoreCase));
            var hasParentheses = (input.StartsWith("(", StringComparison.OrdinalIgnoreCase) && input.EndsWith(")", StringComparison.OrdinalIgnoreCase));

            if (TryParseHexadecimalFormat(input, hasBraces, options, ref result)) { return result; }
            if (TryParseParenthesisFormat(input, hasParentheses, hasHyphens, options, ref result)) { return result; }
            if (TryParseBraceFormat(input, hasBraces, hasHyphens, options, ref result)) { return result; }
            if (TryParseDigitFormat(input, hasHyphens, options, ref result)) { return result; }
            if (TryParseNumberFormat(input, hasHyphens, options, ref result)) { return result; }

            throw new FormatException($"The {nameof(input)} is not in a recognized format.");
        });
    }

    private static bool TryParseNumberFormat(string input, bool hasHyphens, GuidStringOptions options, ref Guid result)
    {
        if (!hasHyphens && options.Formats.HasFlag(GuidFormats.N))
        {
            result = Guid.ParseExact(input, "N");
            return true;
        }
        return false;
    }

    private static bool TryParseDigitFormat(string input, bool hasHyphens, GuidStringOptions options, ref Guid result)
    {
        if (hasHyphens && options.Formats.HasFlag(GuidFormats.D))
        {
            result = Guid.ParseExact(input, "D");
            return true;
        }
        return false;
    }

    private static bool TryParseBraceFormat(string input, bool hasBraces, bool hasHyphens, GuidStringOptions options, ref Guid result)
    {
        if (hasBraces && hasHyphens && options.Formats.HasFlag(GuidFormats.B))
        {
            result = Guid.ParseExact(input, "B");
            return true;
        }
        return false;
    }

    private static bool TryParseParenthesisFormat(string input, bool hasParentheses, bool hasHyphens, GuidStringOptions options, ref Guid result)
    {
        if (hasParentheses && hasHyphens && options.Formats.HasFlag(GuidFormats.P))
        {
            result = Guid.ParseExact(input, "P");
            return true;
        }
        return false;
    }

    private static bool TryParseHexadecimalFormat(string input, bool hasBraces, GuidStringOptions options, ref Guid result)
    {
        var xformat = hasBraces && input.Split(',').Length == 11;
        if (xformat && options.Formats.HasFlag(GuidFormats.X))
        {
            result = Guid.ParseExact(input, "X");
            return true;
        }
        return false;
    }

    /// <summary>
    /// Creates a parser that converts a <see cref="string"/>, represented in hexadecimal digits, to its equivalent <see cref="byte"/> array.
    /// </summary>
    /// <returns>An implementation of <see cref="IConfigurableParser{T}"/> that produces <see cref="byte"/> arrays.</returns>
    /// <exception cref="ArgumentNullException">
    /// <see cref="string"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <see cref="string"/> must be hexadecimal.
    /// </exception>
    public static IParser<byte[]> FromHexadecimal()
    {
        return CreateParser(input =>
        {
            Validator.ThrowIfNull(input);
            Validator.ThrowIfNotHex(input);
            var converted = new List<byte>();
            var stringLength = input.Length / 2;
            using (var reader = new StringReader(input))
            {
                for (var i = 0; i < stringLength; i++)
                {
                    var firstChar = (char)reader.Read();
                    var secondChar = (char)reader.Read();
                    converted.Add(Convert.ToByte(new string(new[] { firstChar, secondChar }), 16));
                }
            }
            return converted.ToArray();
        });
    }

    /// <summary>
    /// Creates a parser that converts a <see cref="string"/>, represented as an URI scheme, to its equivalent <see cref="UriScheme"/>.
    /// </summary>
    /// <returns>An <see cref="IConfigurableParser{UriScheme}"/> implementation.</returns>
    /// <exception cref="ArgumentNullException">
    /// <see cref="string"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <see cref="string"/> cannot be empty or consist only of white-space characters.
    /// </exception>
    public static IParser<UriScheme> FromUriScheme()
    {
        return CreateParser(input =>
        {
            Validator.ThrowIfNullOrWhitespace(input);
            if (!StringToUriSchemeLookupTable.TryGetValue(input ?? "", out var result))
            {
                result = UriScheme.Undefined;
            }
            return result;
        });
    }

    /// <summary>
    /// Creates a parser that converts a <see cref="string"/>, represented in URL-safe base-64 digits, to its equivalent <see cref="byte"/> array.
    /// </summary>
    /// <returns>An implementation of <see cref="IConfigurableParser{T}"/> that produces <see cref="byte"/> arrays.</returns>
    /// <exception cref="ArgumentNullException">
    /// <see cref="string"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <see cref="string"/> cannot be empty or consist only of white-space characters.
    /// </exception>
    /// <exception cref="FormatException">
    /// <see cref="string"/> consist of illegal base-64 digits.
    /// </exception>
    public static IParser<byte[]> FromUrlEncodedBase64()
    {
        return CreateParser(input =>
        {
            Validator.ThrowIfNullOrWhitespace(input);
            input = input.Replace('-', '+');
            input = input.Replace('_', '/');
            switch (input.Length % 4)
            {
                case 0:
                    break;
                case 2:
                    input += "==";
                    break;
                case 3:
                    input += "=";
                    break;
                default:
                    throw new FormatException(FormattableString.Invariant($"The format of {nameof(input)} consist of illegal base64 characters."));
            }
            return Convert.FromBase64String(input);
        });
    }

    /// <summary>
    /// Creates a parser that converts a <see cref="string"/>, represented as a simple input type, to its equivalent <see cref="bool"/>, <see cref="byte"/>, <see cref="int"/>, <see cref="long"/>, <see cref="double"/>, <see cref="float"/>, <see cref="DateTime"/> or <see cref="Guid"/>.
    /// </summary>
    /// <returns>An <see cref="IConfigurableParser{Object,FormattingOptions}"/> implementation.</returns>
    public static IConfigurableParser<object, FormattingOptions> FromValueType()
    {
        return CreateConfigurableParser<object, FormattingOptions>((input, setup) =>
        {
            if (input == null) { return null; }
            var options = Patterns.Configure(setup);
            if (bool.TryParse(input, out var boolinput)) { return boolinput; }
            if (byte.TryParse(input, NumberStyles.None, options.FormatProvider, out var byteinput)) { return byteinput; }
            if (int.TryParse(input, NumberStyles.None, options.FormatProvider, out var intinput)) { return intinput; }
            if (long.TryParse(input, NumberStyles.None, options.FormatProvider, out var longinput)) { return longinput; }
            if (double.TryParse(input, NumberStyles.Any & ~NumberStyles.AllowHexSpecifier & ~NumberStyles.HexNumber, options.FormatProvider, out var doubleinput)) { return doubleinput; }
            if (input.Length > 6 && DateTime.TryParse(input, options.FormatProvider, DateTimeStyles.AdjustToUniversal, out var dateTimeinput)) { return dateTimeinput; }
            if (input.Length > 31 && input.Length < 69 && Guid.TryParse(input, out var guidinput)) { return guidinput; }
            return input;
        });
    }

    /// <summary>
    /// Creates a parser that converts a <see cref="string"/>, represented as an URL, to its equivalent <see cref="Uri"/>.
    /// </summary>
    /// <returns>An <see cref="IConfigurableParser{Uri,UriStringOptions}"/> implementation.</returns>
    /// <exception cref="ArgumentNullException">
    /// <see cref="string"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <see cref="string"/> cannot be empty or consist only of white-space characters.
    /// </exception>
    public static IConfigurableParser<Uri, UriStringOptions> FromUri()
    {
        return CreateConfigurableParser<Uri, UriStringOptions>((input, setup) =>
        {
            Validator.ThrowIfNullOrWhitespace(input);
            Validator.ThrowIfInvalidConfigurator(setup, out var options);
            var isValid = options.Kind == UriKind.Relative;
            if (!isValid)
            {
                foreach (var scheme in options.Schemes)
                {
                    switch (scheme)
                    {
                        case UriScheme.Undefined:
                            break;
                        case UriScheme.File:
                        case UriScheme.Ftp:
                        case UriScheme.Sftp:
                        case UriScheme.Gopher:
                        case UriScheme.Http:
                        case UriScheme.Https:
                        case UriScheme.Mailto:
                        case UriScheme.NetPipe:
                        case UriScheme.NetTcp:
                        case UriScheme.News:
                        case UriScheme.Nntp:
                            var validUriScheme = StringFactory.CreateUriScheme(scheme);
                            isValid = input.StartsWith(validUriScheme, StringComparison.OrdinalIgnoreCase);
                            break;
                        default:
                            throw new InvalidEnumArgumentException(nameof(setup), (int)scheme, typeof(UriScheme));
                    }
                    if (isValid) { break; }
                }
            }
            if (!isValid || !Uri.TryCreate(input, options.Kind, out var result)) { throw new ArgumentException("The specified input is not a valid URI.", nameof(input)); }
            return result;
        });
    }

    /// <summary>
    /// Creates a parser that converts a <see cref="string"/>, represented as a protocol relative URL, to its equivalent <see cref="Uri"/>.
    /// </summary>
    /// <returns>An <see cref="IConfigurableParser{Uri,UriStringOptions}"/> implementation.</returns>
    /// <exception cref="ArgumentNullException">
    /// <see cref="string"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <see cref="string"/> cannot be empty or consist only of white-space characters.
    /// </exception>
    public static IConfigurableParser<Uri, ProtocolRelativeUriStringOptions> FromProtocolRelativeUri()
    {
        return CreateConfigurableParser<Uri, ProtocolRelativeUriStringOptions>((input, setup) =>
        {
            Validator.ThrowIfNullOrWhitespace(input);
            var options = Patterns.Configure(setup, validator: o =>
            {
                Validator.ThrowIfFalse(input.StartsWith(o.RelativeReference, StringComparison.OrdinalIgnoreCase), nameof(input), FormattableString.Invariant($"The specified input did not start with the expected input of: {o.RelativeReference}."));
            });
            var relativeReferenceLength = options.RelativeReference.Length;
            return new Uri(input.Remove(0, relativeReferenceLength).Insert(0, FormattableString.Invariant($"{StringFactory.CreateUriScheme(options.Protocol)}://")));
        });
    }

    /// <summary>
    /// Creates a parser that converts a <see cref="string"/> to an <see cref="object"/> of a particular type.
    /// </summary>
    /// <returns>An <see cref="IConfigurableParser{ObjectFormattingOptions}"/> implementation.</returns>
    /// <exception cref="NotSupportedException">
    /// <see cref="string"/> cannot be converted to the specified <see cref="Type"/>.
    /// </exception>
    /// <remarks>If the underlying <see cref="IFormatProvider"/> of <see cref="FormattingOptions.FormatProvider"/> is a <see cref="CultureInfo"/>, then this will be used in the conversion.</remarks>
    public static IConfigurableParser<ObjectFormattingOptions> FromObject()
    {
        return CreateConfigurableParser<ObjectFormattingOptions>((input, targetType, setup) =>
        {
            if (input == null) { return default; }
            var options = Patterns.Configure(setup);
            var converter = TypeDescriptor.GetConverter(targetType);
            if (options.FormatProvider is CultureInfo ci)
            {
                return converter.ConvertFromString(options.DescriptorContext, ci, input);
            }
            return converter.ConvertFromString(options.DescriptorContext, input);
        });
    }

    /// <summary>
    /// Creates a parser that converts a <see cref="string"/> to its equivalent <see cref="Enum"/>.
    /// </summary>
    /// <returns>An <see cref="IConfigurableParser{EnumStringOptions}"/> implementation.</returns>
    /// <exception cref="ArgumentNullException">
    /// <see cref="string"/> cannot be null -or-
    /// <see cref="Type"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <see cref="string"/> cannot be empty or consist only of white-space characters -or-
    /// <see cref="Type"/> does not represents an enumeration.
    /// </exception>
    /// <exception cref="OverflowException">
    /// <see cref="string"/> is outside the range of the underlying type of <see cref="Type"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <see cref="string"/> is not type SByte, Int16, Int32, Int64, Byte, UInt16, UInt32, or UInt64, or String.
    /// </exception>
    public static IConfigurableParser<EnumStringOptions> FromEnum()
    {
        return CreateConfigurableParser<EnumStringOptions>((input, targetType, setup) =>
        {
            Validator.ThrowIfNullOrWhitespace(input);
            Validator.ThrowIfNull(targetType);
            Validator.ThrowIfNotEnumType(targetType);
            var options = Patterns.Configure(setup);
            var enumType = targetType;
            var hasFlags = enumType.GetTypeInfo().IsDefined(typeof(FlagsAttribute), false);
            var result = Enum.Parse(targetType, input, options.IgnoreCase);
            if (hasFlags && input.IndexOf(',') != -1) { return result; }
            if (Enum.IsDefined(targetType, result)) { return result; }
            throw new ArgumentException("Value does not represents an enumeration.");
        });
    }
}
