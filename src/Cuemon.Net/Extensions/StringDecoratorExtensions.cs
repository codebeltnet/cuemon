using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Cuemon.Net.Collections.Specialized;
using Cuemon.Text;

namespace Cuemon.Net;
/// <summary>
/// Extension methods for the <see cref="string"/> class hidden behind the <see cref="IDecorator{T}"/> interface.
/// </summary>
/// <seealso cref="IDecorator{T}"/>
/// <seealso cref="Decorator{T}"/>
/// <remarks>
/// Kudos to the mono-project team for this class. I only modified some of the original code to fit into this class. For the original code, have a visit here for the source code: https://github.com/mono/mono/blob/master/mcs/class/System.Web/System.Web/HttpUtility.cs or here for the mono-project website: http://www.mono-project.com/.
/// Primary reason for including these methods: https://edi.wang/post/2018/11/25/netcore-webutility-urlencode-httputility-urlencode
/// </remarks>
public static class StringDecoratorExtensions
{
    /// <summary>
    /// Encodes a URL string from the enclosed <see cref="string"/> of the specified <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{String}"/> to extend.</param>
    /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
    /// <returns>An URL encoded string.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static string UrlEncode(this IDecorator<string> decorator, Action<EncodingOptions> setup = null)
    {
        Validator.ThrowIfNull(decorator);
        var value = decorator.Inner;
        if (value == null) { return null; }
        if (value == string.Empty) { return string.Empty; }

        var options = Patterns.Configure(setup);
        var needEncode = false;
        var len = value.Length;
        for (var i = 0; i < len; i++)
        {
            var c = value[i];
            if ((c < '0') || (c < 'A' && c > '9') || (c > 'Z' && c < 'a') || (c > 'z'))
            {
                if (Infrastructure.NotEncoded(c)) { continue; }
                needEncode = true;
                break;
            }
        }

        if (!needEncode) { return value; }

        // avoided GetByteCount call
        var bytes = new byte[options.Encoding.GetMaxByteCount(value.Length)];
        var realLen = options.Encoding.GetBytes(value, 0, value.Length, bytes, 0);

        var encodedBytes = Decorator.Enclose(bytes).UrlEncode(0, realLen);
        return Encoding.ASCII.GetString(encodedBytes, 0, encodedBytes.Length);
    }

    /// <summary>
    /// Converts the enclosed <see cref="string"/> of the specified <paramref name="decorator"/> that has been encoded for transmission in a URL into a decoded string.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{String}"/> to extend.</param>
    /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
    /// <returns>An URL decoded string.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static string UrlDecode(this IDecorator<string> decorator, Action<EncodingOptions> setup = null)
    {
        Validator.ThrowIfNull(decorator);
        var value = decorator.Inner;
        if (null == value) { return null; }
        if (value.IndexOf('%') == -1 && value.IndexOf('+') == -1) { return value; }

        var options = Patterns.Configure(setup);
        long len = value.Length;
        var bytes = new List<byte>();

        for (var i = 0; i < len; i++)
        {
            var ch = value[i];
            if (ch == '%' && i + 2 < len && value[i + 1] != '%')
            {
                int xchar;
                if (value[i + 1] == 'u' && i + 5 < len)
                {
                    // unicode hex sequence
                    xchar = GetChar(value, i + 2, 4);
                    if (xchar != -1)
                    {
                        WriteCharBytes(bytes, (char)xchar, options.Encoding);
                        i += 5;
                    }
                    else
                    {
                        WriteCharBytes(bytes, '%', options.Encoding);
                    }
                }
                else if ((xchar = GetChar(value, i + 1, 2)) != -1)
                {
                    WriteCharBytes(bytes, (char)xchar, options.Encoding);
                    i += 2;
                }
                else
                {
                    WriteCharBytes(bytes, '%', options.Encoding);
                }
                continue;
            }

            WriteCharBytes(bytes, ch == '+' ? ' ' : ch, options.Encoding);
        }

        var buf = bytes.ToArray();
        return options.Encoding.GetString(buf, 0, buf.Length);
    }

    private static void WriteCharBytes(IList buf, char ch, Encoding e)
    {
        if (ch > 255)
        {
            foreach (var b in e.GetBytes(new[] { ch })) { buf.Add(b); }
        }
        else
        {
            buf.Add((byte)ch);
        }
    }

    private static int GetInt(byte b)
    {
        var c = (char)b;
        if (c >= '0' && c <= '9') { return c - '0'; }
        if (c >= 'a' && c <= 'f') { return c - 'a' + 10; }
        if (c >= 'A' && c <= 'F') { return c - 'A' + 10; }
        return -1;
    }

    private static int GetChar(string str, int offset, int length)
    {
        var val = 0;
        var end = length + offset;
        for (var i = offset; i < end; i++)
        {
            var c = str[i];
            if (c > 127) { return -1; }
            var current = GetInt((byte)c);
            if (current == -1) { return -1; }
            val = (val << 4) + current;
        }
        return val;
    }

    internal static QueryStringCollection ToQueryString(this IDecorator<string> decorator, bool urlDecode)
    {
        Validator.ThrowIfNull(decorator);
        var fieldValuePairs = decorator.Inner ?? "";
        var modifiedFieldValuePairs = new QueryStringCollection();
        if (fieldValuePairs.Length == 0) { return modifiedFieldValuePairs; }
        var characterSeparator = NameValueCollectionDecoratorExtensions.GetSeparator(FieldValueSeparator.Ampersand);
        if (fieldValuePairs.StartsWith("?", StringComparison.OrdinalIgnoreCase)) { fieldValuePairs = fieldValuePairs.Remove(0, 1); }
        var namesAndValues = fieldValuePairs.Split(characterSeparator);
        foreach (var nameAndValue in namesAndValues)
        {
            var equalLocation = nameAndValue.IndexOf("=", StringComparison.OrdinalIgnoreCase);
            if (equalLocation < 0) { continue; } // we have no parameter values, just a value pair like lcid=1030& or lcid=1030&test
            var value = equalLocation == nameAndValue.Length ? null : ApplyUrlDecodeWhenRequired(urlDecode, nameAndValue, equalLocation);
            modifiedFieldValuePairs.Add(nameAndValue.Substring(0, nameAndValue.IndexOf("=", StringComparison.OrdinalIgnoreCase)), value);
        }
        return modifiedFieldValuePairs;
    }

    private static string ApplyUrlDecodeWhenRequired(bool urlDecode, string nameAndValue, int equalLocation)
    {
        return urlDecode ? Decorator.Enclose(nameAndValue.Substring(equalLocation + 1)).UrlDecode() : nameAndValue.Substring(equalLocation + 1);
    }
}
