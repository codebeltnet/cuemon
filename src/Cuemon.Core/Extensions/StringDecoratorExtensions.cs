using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Cuemon.Text;

namespace Cuemon
{
    /// <summary>
    /// Extension methods for the <see cref="string"/> class hidden behind the <see cref="IDecorator{T}"/> interface.
    /// </summary>
    /// <seealso cref="IDecorator{T}"/>
    /// <seealso cref="Decorator{T}"/>
    public static class StringDecoratorExtensions
    {
        /// <summary>
        /// Converts the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> to either lowercase, UPPERCASE, Title Case or unaltered.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{String}"/> to extend.</param>
        /// <param name="method">The method to use in the conversion.</param>
        /// <returns>A <see cref="string"/> that corresponds to the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> with the applied conversion <paramref name="method"/>.</returns>
        /// <remarks>Uses <see cref="CultureInfo.InvariantCulture"/> for the conversion.</remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static string ToCasing(this IDecorator<string> decorator, CasingMethod method = CasingMethod.Default)
        {
            return ToCasing(decorator, method, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> to either lowercase, UPPERCASE, Title Case or unaltered using the specified <paramref name="culture"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{String}"/> to extend.</param>
        /// <param name="method">The method to use in the conversion.</param>
        /// <param name="culture">The culture rules to apply the conversion.</param>
        /// <returns>A <see cref="string"/> that corresponds to the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> with the applied conversion <paramref name="method"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null -or-
        /// <paramref name="culture"/> cannot be null.
        /// </exception>
        public static string ToCasing(this IDecorator<string> decorator, CasingMethod method, CultureInfo culture)
        {
            Validator.ThrowIfNull(decorator);
            Validator.ThrowIfNull(culture);
            var value = decorator.Inner;
            switch (method)
            {
                case CasingMethod.Default:
                    return value;
                case CasingMethod.LowerCase:
                    return culture.TextInfo.ToLower(value);
                case CasingMethod.TitleCase:
                    return culture.TextInfo.ToTitleCase(value);
                case CasingMethod.UpperCase:
                    return culture.TextInfo.ToUpper(value);
            }
            return value;
        }

        /// <summary>
        /// Converts the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> to its equivalent <see cref="byte"/> array representation.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{String}"/> to extend.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
        /// <returns>A <see cref="byte"/> array containing the result of the enclosed <see cref="string"/> of the specified <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="setup"/> was initialized with an invalid <see cref="EncodingOptions.Preamble"/>.
        /// </exception>
        public static byte[] ToByteArray(this IDecorator<string> decorator, Action<EncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(decorator);
            return Convertible.GetBytes(decorator.Inner, setup);
        }

        /// <summary>
        /// Encodes all the characters in the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> to its encoded <see cref="string"/> variant.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{String}"/> to extend.</param>
        /// <param name="setup">The <see cref="FallbackEncodingOptions"/> which may be configured.</param>
        /// <returns>A <see cref="string"/> variant of the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> that is encoded with <see cref="FallbackEncodingOptions.TargetEncoding"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        /// <remarks>The inspiration for this method was retrieved @ SO: https://stackoverflow.com/a/135473/175073.</remarks>
        public static string ToEncodedString(this IDecorator<string> decorator, Action<FallbackEncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(decorator);
            var options = Patterns.Configure(setup);
            var result = Encoding.Convert(options.Encoding, Encoding.GetEncoding(options.TargetEncoding.WebName, options.EncoderFallback, options.DecoderFallback), Convertible.GetBytes(decorator.Inner, o =>
            {
                o.Encoding = options.Encoding;
                o.Preamble = options.Preamble;
            }));
            return options.TargetEncoding.GetString(result);
        }

        /// <summary>
        /// Encodes all the characters in the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> to its ASCII encoded variant.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{String}"/> to extend.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
        /// <returns>A <see cref="string"/> variant of the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> that is ASCII encoded.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static string ToAsciiEncodedString(this IDecorator<string> decorator, Action<EncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(decorator);
            var options = Patterns.Configure(setup);
            return ToEncodedString(decorator, o =>
            {
                o.TargetEncoding = Encoding.ASCII;
                o.Encoding = options.Encoding;
                o.Preamble = options.Preamble;
                o.EncoderFallback = new EncoderReplacementFallback("");
            });
        }

        /// <summary>
        /// Converts the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> to a <see cref="Stream"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{String}"/> to extend.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Stream"/> containing the result of the enclosed <see cref="string"/> of the specified <paramref name="decorator"/>.</returns>
        /// <remarks><see cref="IEncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="setup"/> was initialized with an invalid <see cref="EncodingOptions.Preamble"/>.
        /// </exception>
        public static Stream ToStream(this IDecorator<string> decorator, Action<EncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(decorator);
            return Patterns.SafeInvoke(() => new MemoryStream(), ms =>
            {
                var bytes = Convertible.GetBytes(decorator.Inner, setup);
                ms.Write(bytes, 0, bytes.Length);
                ms.Position = 0;
                return ms;
            });
        }

        /// <summary>
        /// Converts the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> to its equivalent <see cref="Uri"/> representation.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{String}"/> to extend.</param>
        /// <param name="uriKind">Specifies whether the URI string is a relative URI, absolute URI, or is indeterminate.</param>
        /// <returns>A <see cref="Uri"/> that corresponds to the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> and <paramref name="uriKind"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// The enclosed <see cref="string"/> of the specified <paramref name="decorator"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The enclosed <see cref="string"/> of the specified <paramref name="decorator"/> cannot be empty or consist only of white-space characters.
        /// </exception>
        public static Uri ToUri(this IDecorator<string> decorator, UriKind uriKind = UriKind.Absolute)
        {
            Validator.ThrowIfNull(decorator);
            Validator.ThrowIfNullOrWhitespace(decorator.Inner);
            return new Uri(decorator.Inner, uriKind);
        }

        /// <summary>
        /// Determines whether the beginning of the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> matches at least one string in the specified sequence of <paramref name="strings"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{String}"/> to extend.</param>
        /// <param name="strings">A sequence of <see cref="string"/> values to match against.</param>
        /// <returns><c>true</c> if at least one value matches the beginning of the enclosed <see cref="string"/> of the specified <paramref name="decorator"/>; otherwise, <c>false</c>.</returns>
        /// <remarks>This match is performed by using a default value of <see cref="StringComparison.OrdinalIgnoreCase"/>.</remarks>
        public static bool StartsWith(this IDecorator<string> decorator, IEnumerable<string> strings)
        {
            return StartsWith(decorator, StringComparison.OrdinalIgnoreCase, strings);
        }

        /// <summary>
        /// Determines whether the beginning of the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> matches at least one string in the specified sequence of <paramref name="strings"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{String}"/> to extend.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <param name="strings">A sequence of <see cref="string"/> values to match against.</param>
        /// <returns><c>true</c> if at least one value matches the beginning of the enclosed <see cref="string"/> of the specified <paramref name="decorator"/>; otherwise, <c>false</c>.</returns>
        public static bool StartsWith(this IDecorator<string> decorator, StringComparison comparison, IEnumerable<string> strings)
        {
            if (decorator?.Inner == null) { return false; }
            if (strings == null) { return false; }
            foreach (var startWithValue in strings)
            {
                if (decorator.Inner.StartsWith(startWithValue, comparison)) { return true; }
            }
            return false;
        }

        /// <summary>
        /// Determines whether the beginning of the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> matches at least one string in the specified sequence of <paramref name="strings"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{String}"/> to extend.</param>
        /// <param name="strings">A sequence of <see cref="string"/> values to match against.</param>
        /// <returns><c>true</c> if at least one value matches the beginning of the enclosed <see cref="string"/> of the specified <paramref name="decorator"/>; otherwise, <c>false</c>.</returns>
        /// <remarks>This match is performed by using a default value of <see cref="StringComparison.OrdinalIgnoreCase"/>.</remarks>
        public static bool StartsWith(this IDecorator<string> decorator, params string[] strings)
        {
            return StartsWith(decorator, (IEnumerable<string>)strings);
        }

        /// <summary>
        /// Determines whether the beginning of the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> matches at least one string in the specified sequence of <paramref name="strings"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{String}"/> to extend.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <param name="strings">A sequence of <see cref="string"/> values to match against.</param>
        /// <returns><c>true</c> if at least one value matches the beginning of this string; otherwise, <c>false</c>.</returns>
        /// <remarks>This match is performed by using a default value of <see cref="StringComparison.OrdinalIgnoreCase"/>.</remarks>
        public static bool StartsWith(this IDecorator<string> decorator, StringComparison comparison, params string[] strings)
        {
            return StartsWith(decorator, comparison, (IEnumerable<string>)strings);
        }

        /// <summary>
        /// Returns the set difference between <paramref name="second"/> and the enclosed <see cref="string"/> of the specified <paramref name="decorator"/>  or <see cref="string.Empty"/> if no difference.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
        /// <param name="second">The value to compare with the enclosed <see cref="string"/> of the specified <paramref name="decorator"/>.</param>
        /// <returns>>A <see cref="string"/> that contains the set difference between <paramref name="second"/> and the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> or <see cref="string.Empty"/> if no difference.</returns>
        public static string Difference(this IDecorator<string> decorator, string second)
        {
            var first = decorator?.Inner;
            first ??= string.Empty;
            second ??= string.Empty;
            return string.Concat(second.Except(first));
        }

        /// <summary>
        /// Returns a value indicating whether the specified <paramref name="values"/> occurs within the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> object.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <param name="values">The <see cref="char"/> sequence to search within the enclosed <see cref="string"/> of the specified <paramref name="decorator"/>.</param>
        /// <returns>
        /// 	<c>true</c> if the <paramref name="values"/> parameter occurs within the enclosed <see cref="string"/> of the specified <paramref name="decorator"/>; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> is null -or-
        /// <paramref name="values"/> is null.
        /// </exception>
        public static bool ContainsAny(this IDecorator<string> decorator, StringComparison comparison, params char[] values)
        {
            Validator.ThrowIfNull(values);
            foreach (var find in values)
            {
                if (ContainsAny(decorator, find, comparison)) { return true; }
            }
            return false;
        }

        /// <summary>
        /// Returns a value indicating whether the specified <paramref name="find"/> occurs within the enclosed <see cref="string"/> of the specified <paramref name="decorator"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
        /// <param name="find">The <see cref="char"/> to search within enclosed <see cref="string"/> of the specified <paramref name="decorator"/>.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison. Default is <see cref="StringComparison.OrdinalIgnoreCase"/>.</param>
        /// <returns>
        /// 	<c>true</c> if the <paramref name="find"/> parameter occurs within the enclosed <see cref="string"/> of the specified <paramref name="decorator"/>; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> is null -or-
        /// <paramref name="find"/> is null.
        /// </exception>
        public static bool ContainsAny(this IDecorator<string> decorator, char find, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            Validator.ThrowIfNull(decorator, out var value);
            Validator.ThrowIfNull(find);
            return (value.IndexOf(new string(find, 1), 0, value.Length, comparison) >= 0);
        }
    }
}
