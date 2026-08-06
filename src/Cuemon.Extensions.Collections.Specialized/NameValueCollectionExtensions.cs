using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Cuemon.Extensions.Collections.Specialized;
/// <summary>
/// Extension methods for the <see cref="NameValueCollection"/> class.
/// </summary>
public static class NameValueCollectionExtensions
{
    /// <summary>
    /// Determines whether the specified <paramref name="nvc"/> contains an entry with the specified <paramref name="key"/>.
    /// </summary>
    /// <param name="nvc">The <see cref="NameValueCollection"/> to extend.</param>
    /// <param name="key">The key to locate in <paramref name="nvc"/>.</param>
    /// <returns><c>true</c> if the specified <paramref name="nvc"/> contains an entry with the <paramref name="key"/>; otherwise, <c>false</c>.</returns>
    /// <remarks>This method performs an <see cref="StringComparison.OrdinalIgnoreCase"/> search for <paramref name="key"/>.</remarks>
    public static bool ContainsKey(this NameValueCollection nvc, string key)
    {
        if (nvc == null) { return false; }
        if (key == null) { return false; }
        if (nvc.Get(key) == null)
        {
            return nvc.AllKeys.Contains(key, StringComparer.OrdinalIgnoreCase);
        }
        return true;
    }

    /// <summary>
    /// Creates a <see cref="IDictionary{TKey,TValue}"/> from the specified <paramref name="nvc"/>.
    /// </summary>
    /// <param name="nvc">The <see cref="NameValueCollection"/> to extend.</param>
    /// <param name="setup">The <see cref="DelimitedStringOptions"/> which may be configured.</param>
    /// <returns>A <see cref="IDictionary{TKey,TValue}"/> that is equivalent to the specified <paramref name="nvc"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="nvc"/> cannot be null.
    /// </exception>
    public static IDictionary<string, string[]> ToDictionary(this NameValueCollection nvc, Action<DelimitedStringOptions> setup = null)
    {
        Validator.ThrowIfNull(nvc);
        var result = new Dictionary<string, string[]>();
        foreach (string item in nvc)
        {
            result.Add(item, DelimitedString.Split(nvc[item], setup));
        }
        return result;
    }
}
