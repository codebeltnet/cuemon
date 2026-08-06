using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Cuemon.Text;

namespace Cuemon.Extensions;
/// <summary>
/// Extension methods for the <see cref="string" /> class.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Returns the set difference between <paramref name="second"/> and <paramref name="first"/> or <see cref="string.Empty"/> if no difference.
    /// </summary>
    /// <param name="first">The value where characters that are not also in <paramref name="second"/> will be returned.</param>
    /// <param name="second">The value to compare with <paramref name="first"/>.</param>
    /// <returns>>A <see cref="string"/> that contains the set difference between <paramref name="second"/> and <paramref name="first"/> or <see cref="string.Empty"/> if no difference.</returns>
    public static string Difference(this string first, string second)
    {
        return Decorator.Enclose(first, false).Difference(second);
    }

    /// <summary>
    /// Converts the specified <paramref name="input"/> to its equivalent <see cref="byte"/> array representation.
    /// </summary>
    /// <param name="input">The <see cref="string"/> to be converted into a <see cref="byte"/> array.</param>
    /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
    /// <returns>A <see cref="byte"/> array that is equivalent to <paramref name="input"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="input"/> cannot be null.
    /// </exception>
    /// <exception cref="InvalidEnumArgumentException">
    /// <paramref name="setup"/> was initialized with an invalid <see cref="EncodingOptions.Preamble"/>.
    /// </exception>
    /// <seealso cref="EncodingOptions"/>
    public static byte[] ToByteArray(this string input, Action<EncodingOptions> setup = null)
    {
        return Convertible.GetBytes(input, setup);
    }

    /// <summary>
    /// Converts the specified <paramref name="input"/> of URL-safe base64 characters to its equivalent <see cref="byte"/> array representation.
    /// </summary>
    /// <param name="input">The <see cref="string"/> to extend.</param>
    /// <returns>A <see cref="byte"/> array that is equivalent to <paramref name="input"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="input"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="input"/> cannot be empty or consist only of white-space characters.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="input"/> has illegal base64 characters.
    /// </exception>
    /// <seealso cref="ParserFactory.FromUrlEncodedBase64"/>
    public static byte[] FromUrlEncodedBase64(this string input)
    {
        return ParserFactory.FromUrlEncodedBase64().Parse(input);
    }

    /// <summary>
    /// Converts the specified <paramref name="input"/> of a GUID to its equivalent <see cref="Guid"/> structure.
    /// </summary>
    /// <param name="input">The <see cref="string"/> to extend.</param>
    /// <param name="setup">The <see cref="GuidStringOptions"/> which may be configured.</param>
    /// <returns>A <see cref="Guid"/> that is equivalent to <paramref name="input"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="input"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="input"/> cannot be empty or consist only of white-space characters.
    /// </exception>
    /// <exception cref="FormatException">
    /// The specified <paramref name="input"/> was not recognized to be a GUID.
    /// </exception>
    /// <seealso cref="ParserFactory.FromGuid"/>
    public static Guid ToGuid(this string input, Action<GuidStringOptions> setup = null)
    {
        return ParserFactory.FromGuid().Parse(input, setup);
    }

    /// <summary>
    /// Converts the specified <paramref name="input"/> of binary digits to its equivalent <see cref="byte"/> array representation.
    /// </summary>
    /// <param name="input">The <see cref="string"/> to extend.</param>
    /// <returns>A <see cref="byte"/> array that is equivalent to <paramref name="input"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="input"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="input"/> cannot be empty or consist only of white-space characters.
    /// </exception>
    /// <exception cref="FormatException">
    /// <paramref name="input"/> must consist only of binary digits.
    /// </exception>
    /// <seealso cref="ParserFactory.FromBinaryDigits"/>
    public static byte[] FromBinaryDigits(this string input)
    {
        return ParserFactory.FromBinaryDigits().Parse(input);
    }

    /// <summary>
    /// Converts the specified string, which encodes binary data as base-64 digits, to an equivalent 8-bit unsigned integer array.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <returns>An array of 8-bit unsigned integers that is equivalent to <paramref name="value"/>.</returns>
    public static byte[] FromBase64(this string value)
    {
        Validator.ThrowIfNull(value);
        return Convert.FromBase64String(value);
    }

    /// <summary>
    /// Converts the specified <paramref name="value"/> to either lowercase, UPPERCASE, Title Case or unaltered.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="method">The method to use in the conversion.</param>
    /// <returns>A <see cref="string"/> that corresponds to <paramref name="value"/> with the applied conversion <paramref name="method"/>.</returns>
    /// <remarks>Uses <see cref="CultureInfo.InvariantCulture"/> for the conversion.</remarks>
    public static string ToCasing(this string value, CasingMethod method = CasingMethod.Default)
    {
        return ToCasing(value, method, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Converts the specified <paramref name="value"/> to either lowercase, UPPERCASE, Title Case or unaltered using the specified <paramref name="culture"/>.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="method">The method to use in the conversion.</param>
    /// <param name="culture">The culture rules to apply the conversion.</param>
    /// <returns>A <see cref="string"/> that corresponds to <paramref name="value"/> with the applied conversion <paramref name="method"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> cannot be null -or-
    /// <paramref name="culture"/> cannot be null.
    /// </exception>
    public static string ToCasing(this string value, CasingMethod method, CultureInfo culture)
    {
        Validator.ThrowIfNull(value);
        return Decorator.Enclose(value).ToCasing(method, culture);
    }

    /// <summary>
    /// Converts the specified <paramref name="value"/> to its equivalent <see cref="Uri"/> representation.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="uriKind">Specifies whether the URI string is a relative URI, absolute URI, or is indeterminate.</param>
    /// <returns>A <see cref="Uri"/> that corresponds to <paramref name="value"/> and <paramref name="uriKind"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> cannot be empty or consist only of white-space characters.
    /// </exception>
    public static Uri ToUri(this string value, UriKind uriKind = UriKind.Absolute)
    {
        Validator.ThrowIfNullOrWhitespace(value);
        return Decorator.Enclose(value).ToUri(uriKind);
    }

    /// <summary>
    /// Determines whether the specified <paramref name="value"/> is null or an <see cref="string.Empty"/> string.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <returns><c>true</c> if the <paramref name="value"/> is null or an empty string (""); otherwise, <c>false</c>.</returns>
    public static bool IsNullOrEmpty(this string value)
    {
        return string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// Determines whether a string sequence has at least one value that equals to null or empty.
    /// </summary>
    /// <param name="source">The <see cref="string"/> sequence to extend.</param>
    /// <returns>
    /// 	<c>true</c> if a string sequence has at least one value that equals to null or empty; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> is null.
    /// </exception>
    public static bool IsNullOrEmpty(this IEnumerable<string> source)
    {
        Validator.ThrowIfNull(source);
        foreach (var value in source)
        {
            if (string.IsNullOrEmpty(value)) { return true; }
        }
        return false;
    }

    /// <summary>
    /// Determines whether the specified <paramref name="value"/> is null, empty, or consists only of white-space characters.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <returns><c>true</c> if the value parameter is null or an empty string (""), or if value consists exclusively of white-space characters; otherwise, <c>false</c>.</returns>
    public static bool IsNullOrWhiteSpace(this string value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    /// <summary>
    /// Determines whether the specified <paramref name="value"/> has a valid format of an email address.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <returns><c>true</c> if the specified <paramref name="value"/> has a valid format of an email address; otherwise, <c>false</c>.</returns>
    public static bool IsEmailAddress(this string value)
    {
        return Condition.IsEmailAddress(value);
    }

    /// <summary>
    /// Determines whether the specified <paramref name="value"/> has a valid format of a <see cref="Guid"/>.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="format">A bitmask comprised of one or more <see cref="GuidFormats"/> that specify how the GUID parsing is conducted.</param>
    /// <returns><c>true</c> if the specified <paramref name="value"/> has a format of a <see cref="Guid"/>; otherwise, <c>false</c>.</returns>
    public static bool IsGuid(this string value, GuidFormats format = GuidFormats.B | GuidFormats.D | GuidFormats.P)
    {
        return Condition.IsGuid(value, format);
    }

    /// <summary>
    /// Determines whether the specified <paramref name="value"/> is hexadecimal.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <returns><c>true</c> if the specified <paramref name="value"/> is hexadecimal; otherwise, <c>false</c>.</returns>
    public static bool IsHex(this string value)
    {
        return Condition.IsHex(value);
    }

    /// <summary>
    /// Determines whether the specified value can be evaluated as a number.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="style">A bitwise combination of <see cref="NumberStyles"/> values that indicates the permitted format of <paramref name="value"/>.</param>
    /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information about <paramref name="value"/>.</param>
    /// <returns><c>true</c> if the specified value can be evaluated as a number; otherwise, <c>false</c>.</returns>
    public static bool IsNumeric(this string value, NumberStyles style = NumberStyles.Number, IFormatProvider provider = null)
    {
        return Condition.IsNumeric(value, style, provider);
    }

    /// <summary>
    /// Determines whether the specified <paramref name="value"/> matches a Base64 structure.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <returns><c>true</c> if the specified <paramref name="value"/> matches a Base64 structure; otherwise, <c>false</c>.</returns>
    /// <remarks>This method will skip common Base64 structures typically used as checksums. This includes 32, 128, 160, 256, 384 and 512 bit checksums.</remarks>
    public static bool IsBase64(this string value)
    {
        return Condition.IsBase64(value);
    }

    /// <summary>
    /// Returns a <see cref="string"/> array that contain the substrings of <paramref name="value"/> delimited by a <see cref="DelimitedStringOptions.Delimiter"/> that may be quoted by <see cref="DelimitedStringOptions.Qualifier"/>.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="setup">The <see cref="DelimitedStringOptions"/> which may be configured.</param>
    /// <returns>A <see cref="string"/> array that contain the substrings of <paramref name="value"/> delimited by a <see cref="DelimitedStringOptions.Delimiter"/> and optionally surrounded within <see cref="DelimitedStringOptions.Qualifier"/>.</returns>
    /// <remarks>
    /// This method was inspired by two articles on StackOverflow @ http://stackoverflow.com/questions/2807536/split-string-in-c-sharp and https://stackoverflow.com/questions/3776458/split-a-comma-separated-string-with-both-quoted-and-unquoted-strings.
    /// The default implementation conforms with the RFC-4180 standard.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// An error occurred while splitting <paramref name="value"/> into substrings separated by <see cref="DelimitedStringOptions.Delimiter"/> and quoted with <see cref="DelimitedStringOptions.Qualifier"/>.
    /// This is typically related to data corruption, eg. a field has not been properly closed with the <see cref="DelimitedStringOptions.Qualifier"/> specified.
    /// </exception>
    public static string[] SplitDelimited(this string value, Action<DelimitedStringOptions> setup = null)
    {
        return DelimitedString.Split(value, setup);
    }

    /// <summary>
    /// Counts the occurrences of <paramref name="character"/> in the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="character">The <see cref="char"/> value to count in <paramref name="value"/>.</param>
    /// <returns>The number of times the <paramref name="character"/> was found in the <paramref name="value"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is null.
    /// </exception>
    public static int Count(this string value, char character)
    {
        Validator.ThrowIfNull(value);
        var count = 0;
        for (var i = 0; i < value.Length; i++)
        {
            if (value[i] == character) { count++; }
        }
        return count;
    }

    /// <summary>
    /// Returns a new string in which all the specified <paramref name="filter"/> values has been deleted from the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="filter">The filter containing the characters and/or words to delete.</param>
    /// <returns>A new string that is equivalent to <paramref name="value"/> except for the removed characters and/or words.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is null or <paramref name="filter"/> is null.
    /// </exception>
    /// <remarks>This method performs an ordinal (case-sensitive and culture-insensitive) comparison. The search begins at the first character position of this string and continues through the last character position.</remarks>
    public static string RemoveAll(this string value, params string[] filter)
    {
        return StringReplacePair.RemoveAll(value, filter);
    }
    /// <summary>
    /// Returns a new string in which all the specified <paramref name="filter"/> values has been deleted from the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
    /// <param name="filter">The filter containing the characters and/or words to delete.</param>
    /// <returns>A new string that is equivalent to <paramref name="value"/> except for the removed characters and/or words.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is null or <paramref name="filter"/> is null.
    /// </exception>
    public static string RemoveAll(this string value, StringComparison comparison, params string[] filter)
    {
        return StringReplacePair.RemoveAll(value, comparison, filter);
    }

    /// <summary>
    /// Returns a new string array in which all the specified <paramref name="filter"/> values has been deleted from the specified <paramref name="source"/> array.
    /// </summary>
    /// <param name="source">The <see cref="string"/> sequence to extend.</param>
    /// <param name="filter">The filter containing the characters and/or words to delete.</param>
    /// <returns>A new string array that is equivalent to <paramref name="source"/> except for the removed characters and/or words.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> is null or <paramref name="filter"/> is null.
    /// </exception>
    /// <remarks>This method performs an ordinal (case-sensitive and culture-insensitive) comparison. The search begins at the first character position of this string and continues through the last character position.</remarks>
    public static string[] RemoveAll(this string[] source, params string[] filter)
    {
        return StringReplacePair.RemoveAll(source, filter);
    }

    /// <summary>
    /// Returns a new string array in which all the specified <paramref name="filter"/> values has been deleted from the specified <paramref name="source"/> array.
    /// </summary>
    /// <param name="source">The <see cref="string"/> sequence to extend.</param>
    /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
    /// <param name="filter">The filter containing the characters and/or words to delete.</param>
    /// <returns>A new string array that is equivalent to <paramref name="source"/> except for the removed characters and/or words.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> is null or <paramref name="filter"/> is null.
    /// </exception>
    public static string[] RemoveAll(this string[] source, StringComparison comparison, params string[] filter)
    {
        return StringReplacePair.RemoveAll(source, comparison, filter);
    }

    /// <summary>
    /// Returns a new string in which all the specified <paramref name="filter"/> values has been deleted from the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="filter">The filter containing the characters and/or words to delete.</param>
    /// <returns>A new string that is equivalent to <paramref name="value"/> except for the removed characters.</returns>
    public static string RemoveAll(this string value, params char[] filter)
    {
        return StringReplacePair.RemoveAll(value, filter);
    }

    /// <summary>
    /// Replaces all occurrences of <paramref name="oldValue"/> in <paramref name="value"/>, with <paramref name="newValue"/>.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="oldValue">The <see cref="string"/> value to be replaced.</param>
    /// <param name="newValue">The <see cref="string"/> value to replace all occurrences of <paramref name="oldValue"/>.</param>
    /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison. Default is <see cref="StringComparison.OrdinalIgnoreCase"/>.</param>
    /// <returns>A <see cref="string"/> equivalent to <paramref name="value"/> but with all instances of <paramref name="oldValue"/> replaced with <paramref name="newValue"/>.</returns>
    public static string ReplaceAll(this string value, string oldValue, string newValue, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
    {
        return StringReplacePair.ReplaceAll(value, oldValue, newValue, comparison);
    }

    /// <summary>
    /// Escapes the given <see cref="string"/> the same way as the well known JavaScript escape() function.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <returns>The input <paramref name="value"/> with an escaped equivalent.</returns>
    public static string JsEscape(this string value)
    {
        Validator.ThrowIfNull(value);
        var builder = new StringBuilder(value.Length);
        foreach (var character in value)
        {
            if (DoEscapeOrUnescape(character))
            {
                builder.AppendFormat(CultureInfo.InvariantCulture, character < byte.MaxValue ? "%{0:X2}" : "%u{0:X4}", (uint)character);
            }
            else
            {
                builder.Append(character);
            }
        }
        return builder.ToString();
    }

    /// <summary>
    /// Unescapes the given <see cref="string"/> the same way as the well known Javascript unescape() function.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <returns>The input <paramref name="value"/> with an unescaped equivalent.</returns>
    public static string JsUnescape(this string value)
    {
        Validator.ThrowIfNull(value);
        var builder = new StringBuilder(value);
        var unicode = new Regex("%u([0-9]|[a-f])([0-9]|[a-f])([0-9]|[a-f])([0-9]|[a-f])", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(2));
        var matches = unicode.Matches(value);
        foreach (Match unicodeMatch in matches)
        {
            builder.Replace(unicodeMatch.Value, Convert.ToChar(int.Parse(unicodeMatch.Value.Remove(0, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture)).ToString());
        }

        for (var i = byte.MinValue; i < byte.MaxValue; i++)
        {
            if (DoEscapeOrUnescape(i))
            {
                builder.Replace(string.Format(CultureInfo.InvariantCulture, "%{0:X2}", i), Convert.ToChar(i).ToString());
            }
        }
        return builder.ToString();
    }

    private static bool DoEscapeOrUnescape(int charValue)
    {
        return ((charValue < 42 || charValue > 126) || (charValue > 57 && charValue < 64) || (charValue == 92));
    }

    /// <summary>
    /// Returns a value indicating whether any of the specified <paramref name="values"/> occurs within the <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="values">The <see cref="string"/> sequence to search within <paramref name="value"/>.</param>
    /// <returns>
    /// 	<c>true</c> if any of the <paramref name="values"/> occurs within the <paramref name="value"/>; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>This method performs an ordinal (case-insensitive and culture-insensitive) comparison. The search begins at the first character position of this string and continues through the last character position.</remarks>
    public static bool ContainsAny(this string value, params string[] values)
    {
        return ContainsAny(value, StringComparison.OrdinalIgnoreCase, values);
    }

    /// <summary>
    /// Returns a value indicating whether any of the specified <paramref name="values"/> occurs within the <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
    /// <param name="values">The <see cref="string"/> sequence to search within <paramref name="value"/>.</param>
    /// <returns>
    /// 	<c>true</c> if any of the <paramref name="values"/> occurs within the <paramref name="value"/>; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is null -or-
    /// <paramref name="values"/> is null.
    /// </exception>
    public static bool ContainsAny(this string value, StringComparison comparison, params string[] values)
    {
        Validator.ThrowIfNull(values);
        foreach (var find in values)
        {
            if (ContainsAny(value, find, comparison)) { return true; }
        }
        return false;
    }

    /// <summary>
    /// Returns a value indicating whether the specified <paramref name="find"/> occurs within the <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="find">The <see cref="string"/> to find within <paramref name="value"/>.</param>
    /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison. Default is <see cref="StringComparison.OrdinalIgnoreCase"/>.</param>
    /// <returns>
    /// 	<c>true</c> if the <paramref name="find"/> parameter occurs within the <paramref name="value"/>; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is null -or-
    /// <paramref name="find"/> is null.
    /// </exception>
    public static bool ContainsAny(this string value, string find, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
    {
        Validator.ThrowIfNull(value);
        Validator.ThrowIfNull(find);
        return (value.IndexOf(find, comparison) >= 0);
    }


    /// <summary>
    /// Returns a value indicating whether the specified <paramref name="values"/> occurs within the <paramref name="value"/> object.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
    /// <param name="values">The <see cref="char"/> sequence to search within <paramref name="value"/>.</param>
    /// <returns>
    /// 	<c>true</c> if the <paramref name="values"/> parameter occurs within the <paramref name="value"/>; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is null -or-
    /// <paramref name="values"/> is null.
    /// </exception>
    public static bool ContainsAny(this string value, StringComparison comparison, params char[] values)
    {
        return Decorator.EncloseToExpose(value).ContainsAny(comparison, values);
    }

    /// <summary>
    /// Returns a value indicating whether the specified <paramref name="find"/> occurs within the <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="find">The <see cref="char"/> to search within <paramref name="value"/>.</param>
    /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison. Default is <see cref="StringComparison.OrdinalIgnoreCase"/>.</param>
    /// <returns>
    /// 	<c>true</c> if the <paramref name="find"/> parameter occurs within the <paramref name="value"/>; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is null -or-
    /// <paramref name="find"/> is null.
    /// </exception>
    public static bool ContainsAny(this string value, char find, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
    {
        return Decorator.EncloseToExpose(value, false).ContainsAny(find, comparison);
    }

    /// <summary>
    /// Returns a value indicating whether the specified <paramref name="values"/> occurs within the <paramref name="value"/> object.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="values">The <see cref="char"/> sequence to search within <paramref name="value"/>.</param>
    /// <returns>
    /// 	<c>true</c> if the <paramref name="values"/> parameter occurs within the <paramref name="value"/>; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>This method performs an ordinal (case-sensitive and culture-insensitive) comparison. The search begins at the first character position of this string and continues through the last character position.</remarks>
    public static bool ContainsAny(this string value, params char[] values)
    {
        return ContainsAny(value, StringComparison.Ordinal, values);
    }

    /// <summary>
    /// Returns a value indicating whether all of the specified <paramref name="values"/> occurs within the <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="values">The <see cref="string"/> sequence to search within <paramref name="value"/>.</param>
    /// <returns>
    /// 	<c>true</c> if all of the <paramref name="values"/> occurs within the <paramref name="value"/>; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>This method performs an ordinal (case-insensitive and culture-insensitive) comparison. The search begins at the first character position of this string and continues through the last character position.</remarks>
    public static bool ContainsAll(this string value, params string[] values)
    {
        return ContainsAll(value, StringComparison.OrdinalIgnoreCase, values);
    }

    /// <summary>
    /// Returns a value indicating whether all of the specified <paramref name="values"/> occurs within the <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
    /// <param name="values">The <see cref="string"/> sequence to search within <paramref name="value"/>.</param>
    /// <returns>
    /// 	<c>true</c> if all of the <paramref name="values"/> occurs within the <paramref name="value"/>; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is null -or-
    /// <paramref name="values"/> is null.
    /// </exception>
    public static bool ContainsAll(this string value, StringComparison comparison, params string[] values)
    {
        Validator.ThrowIfNull(value);
        Validator.ThrowIfNull(values);
        var result = true;
        foreach (var s in values)
        {
            result &= ContainsAny(value, comparison, s);
        }
        return result;
    }

    /// <summary>
    /// Returns a value indicating the specified <paramref name="value"/> equals one of the specified <paramref name="values"/>.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="values">The <see cref="string"/> sequence to search within <paramref name="value"/>.</param>
    /// <returns><c>true</c> if one the <paramref name="values"/> is the same as the <paramref name="value"/>; otherwise <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is null - or - <paramref name="values"/> is null.
    /// </exception>
    /// <remarks>This method performs an ordinal (case-sensitive and culture-insensitive) comparison. The search begins at the first character position of this string and continues through the last character position.</remarks>
    public static bool EqualsAny(this string value, params string[] values)
    {
        return EqualsAny(value, StringComparison.Ordinal, values);
    }

    /// <summary>
    /// Returns a value indicating the specified <paramref name="value"/> equals one of the specified <paramref name="values"/>.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="values">The <see cref="string"/> sequence to search within <paramref name="value"/>.</param>
    /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
    /// <returns><c>true</c> if one the <paramref name="values"/> is the same as the <paramref name="value"/>; otherwise <c>false</c>.</returns>
    public static bool EqualsAny(this string value, StringComparison comparison, params string[] values)
    {
        if (value == null) { return false; }
        if (values == null) { return false; }
        foreach (var v in values)
        {
            if (value.Equals(v, comparison)) { return true; }
        }
        return false;
    }

    /// <summary>
    /// Determines whether the beginning of an instance of <see cref="string"/> matches at least one string in the specified sequence of strings.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="startWithValues">A sequence of <see cref="string"/> values to match against.</param>
    /// <returns><c>true</c> if at least one value matches the beginning of this string; otherwise, <c>false</c>.</returns>
    /// <remarks>This match is performed by using a default value of <see cref="StringComparison.OrdinalIgnoreCase"/>.</remarks>
    public static bool StartsWith(this string value, IEnumerable<string> startWithValues)
    {
        return StartsWith(value, StringComparison.OrdinalIgnoreCase, startWithValues);
    }

    /// <summary>
    /// Determines whether the beginning of an instance of <see cref="string"/> matches at least one string in the specified sequence of strings.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
    /// <param name="strings">A sequence of <see cref="string"/> values to match against.</param>
    /// <returns><c>true</c> if at least one value matches the beginning of this string; otherwise, <c>false</c>.</returns>
    public static bool StartsWith(this string value, StringComparison comparison, IEnumerable<string> strings)
    {
        return Decorator.Enclose(value).StartsWith(comparison, strings);
    }

    /// <summary>
    /// Determines whether the beginning of an instance of <see cref="string"/> matches at least one string in the specified sequence of strings.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="strings">A sequence of <see cref="string"/> values to match against.</param>
    /// <returns><c>true</c> if at least one value matches the beginning of this string; otherwise, <c>false</c>.</returns>
    /// <remarks>This match is performed by using a default value of <see cref="StringComparison.OrdinalIgnoreCase"/>.</remarks>
    public static bool StartsWith(this string value, params string[] strings)
    {
        return StartsWith(value, (IEnumerable<string>)strings);
    }

    /// <summary>
    /// Determines whether the beginning of an instance of <see cref="string"/> matches at least one string in the specified sequence of strings.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
    /// <param name="strings">A sequence of <see cref="string"/> values to match against.</param>
    /// <returns><c>true</c> if at least one value matches the beginning of this string; otherwise, <c>false</c>.</returns>
    /// <remarks>This match is performed by using a default value of <see cref="StringComparison.OrdinalIgnoreCase"/>.</remarks>
    public static bool StartsWith(this string value, StringComparison comparison, params string[] strings)
    {
        return StartsWith(value, comparison, (IEnumerable<string>)strings);
    }

    /// <summary>
    /// Removes all occurrences of white-space characters from the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <returns>The string that remains after all occurrences of white-space characters are removed from the specified <paramref name="value"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is null.
    /// </exception>
    public static string TrimAll(this string value)
    {
        return TrimAll(value, null);
    }

    /// <summary>
    /// Removes all occurrences of a set of characters specified in <paramref name="trimChars"/> from the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="trimChars">An array of Unicode characters to remove. Default value is <see cref="Alphanumeric.WhiteSpace"/>.</param>
    /// <returns>The string that remains after all occurrences of the characters in the <paramref name="trimChars"/> parameter are removed from the specified <paramref name="value"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is null.
    /// </exception>
    public static string TrimAll(this string value, params char[] trimChars)
    {
        Validator.ThrowIfNull(value);
        if (trimChars == null || trimChars.Length == 0) { trimChars = Alphanumeric.WhiteSpace.ToCharArray(); }
        var result = new List<char>();
        foreach (var c in value)
        {
            var skip = false;
            foreach (var t in trimChars)
            {
                if (c.Equals(t))
                {
                    skip = true;
                    break;
                }
            }
            if (!skip) { result.Add(c); }
        }
        return new string(result.ToArray());
    }

    /// <summary>
    /// Determines whether the elements of the specified <paramref name="source"/> is equivalent to the specified <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the expected values contained within the sequence of <paramref name="source"/>.</typeparam>
    /// <param name="source">The <see cref="string"/> sequence to extend.</param>
    /// <param name="culture">The culture-specific formatting information to apply on the elements within <paramref name="source"/>.</param>
    /// <param name="context">The type-specific formatting information to apply on the elements within <paramref name="source"/>.</param>
    /// <param name="parser">The function delegate that evaluates if the elements  of <paramref name="source"/> is equivalent to the specified <typeparamref name="T"/>.</param>
    /// <returns><c>true</c> if elements of the <paramref name="source"/> parameter was successfully converted; otherwise <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> cannot be null.
    /// </exception>
    public static bool IsSequenceOf<T>(this IEnumerable<string> source, CultureInfo culture = null, ITypeDescriptorContext context = null, Func<string, CultureInfo, bool> parser = null)
    {
        Validator.ThrowIfNull(source);
        return IsSequenceOfCore<T>(source, culture, context, parser);
    }

    private static bool IsSequenceOfCore<T>(IEnumerable<string> source, CultureInfo culture, ITypeDescriptorContext context, Func<string, CultureInfo, bool> parser)
    {
        culture ??= CultureInfo.InvariantCulture;
        var converterHasValue = (parser != null);
        var valid = true;
        foreach (var substring in source)
        {
            valid &= converterHasValue ? parser(substring, culture) : CanConvertString<T>(substring, culture, context);
        }
        return valid;
    }

    private static bool CanConvertString<T>(string s, CultureInfo culture, ITypeDescriptorContext context)
    {
        return ParserFactory.FromObject().TryParse<T>(s, out _, o =>
        {
            o.FormatProvider = culture;
            o.DescriptorContext = context;
        });
    }

    /// <summary>
    /// Converts the specified hexadecimal <paramref name="value"/> to its equivalent <see cref="string"/> representation.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="setup">The <see cref="EncodingOptions"/> which need to be configured.</param>
    /// <returns>A <see cref="string"/> representation of the hexadecimal characters in <paramref name="value"/>.</returns>
    /// <remarks><see cref="EncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> must be hexadecimal.
    /// </exception>
    /// <seealso cref="ParserFactory.FromHexadecimal"/>
    public static string FromHexadecimal(this string value, Action<EncodingOptions> setup = null)
    {
        return Convertible.ToString(ParserFactory.FromHexadecimal().Parse(value), setup);
    }

    /// <summary>
    /// Converts the specified <paramref name="value"/> to its equivalent hexadecimal representation.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="setup">The <see cref="EncodingOptions"/> which need to be configured.</param>
    /// <returns>A hexadecimal <see cref="string"/> representation of the characters in <paramref name="value"/>.</returns>
    /// <remarks><see cref="EncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
    public static string ToHexadecimal(this string value, Action<EncodingOptions> setup = null)
    {
        return StringFactory.CreateHexadecimal(value, setup);
    }

    /// <summary>
    /// Converts the string representation of the name or numeric <paramref name="value"/> of one or more enumerated constants to an equivalent enumerated <typeparamref name="TEnum"/>.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enumeration to convert.</typeparam>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="ignoreCase"><c>true</c> to ignore case; <c>false</c> to regard case.</param>
    /// <returns>An enum of type <typeparamref name="TEnum" /> whose value is represented by <paramref name="value" />.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <typeparamref name="TEnum"/> does not represents an enumeration.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> does not represents an enumeration.
    /// </exception>
    /// <seealso cref="ParserFactory.FromEnum"/>
    public static TEnum ToEnum<TEnum>(this string value, bool ignoreCase = true) where TEnum : struct, IConvertible
    {
        if (value != null) { Validator.ThrowIfNotEnumType<TEnum>(nameof(TEnum)); }
        return ParserFactory.FromEnum().Parse<TEnum>(value, o => o.IgnoreCase = ignoreCase);
    }

    /// <summary>
    /// Converts the specified <paramref name="value"/> to its equivalent <see cref="TimeSpan"/> representation.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="timeUnit">One of the enumeration values that specifies the outcome of the conversion.</param>
    /// <returns>A <see cref="TimeSpan"/> that corresponds to <paramref name="value"/> from <paramref name="timeUnit"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is null.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The <paramref name="value"/> paired with <paramref name="timeUnit"/> is outside its valid range.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="timeUnit"/> was outside its valid range.
    /// </exception>
    public static TimeSpan ToTimeSpan(this string value, TimeUnit timeUnit)
    {
        return Decorator.Enclose(double.Parse(value, CultureInfo.InvariantCulture)).ToTimeSpan(timeUnit);
    }

    /// <summary>
    /// Retrieves a substring from the specified <paramref name="value"/>. The substring starts at position 0 and continues until the first occurrence of <paramref name="match"/>.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to extend.</param>
    /// <param name="match">The match that will define the stopping point.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
    /// <returns>A substring that contains only the value just before <paramref name="match"/>.</returns>
    public static string SubstringBefore(this string value, string match, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
    {
        Validator.ThrowIfNull(value);
        Validator.ThrowIfNull(match);
        var indexOf = value.IndexOf(match, comparisonType);
        return indexOf == -1 ? "" : value.Substring(0, indexOf);
    }

    /// <summary>
    /// Returns a sequence that is chunked into string-slices having a length of 1024 that is equivalent to <paramref name="value"/>.
    /// </summary>
    /// <param name="value">A <see cref="string" /> to chunk into a sequence of smaller string-slices for partitioned storage or similar.</param>
    /// <returns>A sequence that is chunked into string-slices having a length of 1024 that is equivalent to <paramref name="value"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is null.
    /// </exception>
    public static IEnumerable<string> Chunk(this string value)
    {
        return Chunk(value, 1024);
    }

    /// <summary>
    /// Returns a sequence that is chunked into string-slices of the specified <paramref name="length"/> that is equivalent to <paramref name="value"/>. Default is 1024.
    /// </summary>
    /// <param name="value">A <see cref="string" /> to chunk into a sequence of smaller string-slices for partitioned storage or similar.</param>
    /// <param name="length">The desired length of each string-slice in the sequence.</param>
    /// <returns>A sequence that is chunked into string-slices of the specified <paramref name="length"/> that is equivalent to <paramref name="value"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="length"/> is less or equal to 0.
    /// </exception>
    public static IEnumerable<string> Chunk(this string value, int length)
    {
        Validator.ThrowIfNull(value);
        Validator.ThrowIfLowerThanOrEqual(length, 0, nameof(length));
        if (value.Length <= length)
        {
            yield return value;
        }
        else
        {
            var index = 0;
            while (index < value.Length)
            {
                var smallestLength = Math.Min(length, value.Length - index);
                yield return value.Substring(index, smallestLength);
                index += smallestLength;
            }
        }
    }

    /// <summary>
    /// Suffixes the <paramref name="source"/> with the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="source">The string to extend.</param>
    /// <param name="value">The value to suffix <paramref name="source"/>.</param>
    /// <returns>A string with a suffix <paramref name="value"/>.</returns>
    public static string SuffixWith(this string source, string value)
    {
        return new Stem(source).AttachSuffix(value);
    }

    /// <summary>
    /// Suffixes the <paramref name="source"/> with a forwarding slash.
    /// </summary>
    /// <param name="source">The string to extend.</param>
    /// <returns>A string with a suffix forwarding slash.</returns>
    public static string SuffixWithForwardingSlash(this string source)
    {
        return source.SuffixWith("/");
    }

    /// <summary>
    /// Prefixes the <paramref name="source"/> with the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="source">The string to extend.</param>
    /// <param name="value">The value to prefix <paramref name="source"/>.</param>
    /// <returns>A string with a prefix <paramref name="value"/>.</returns>
    public static string PrefixWith(this string source, string value)
    {
        return new Stem(source).AttachPrefix(value);
    }
}
