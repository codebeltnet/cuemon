using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cuemon.Collections.Generic;

namespace Cuemon
{
    /// <summary>
    /// Defines a oldValue/newValue pair that can be set or retrieved for string replace operations.
    /// </summary>
    public readonly struct StringReplacePair
    {
        /// <summary>
        /// Replaces all occurrences of <paramref name="oldValue"/> in <paramref name="value"/>, with <paramref name="newValue"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to perform the replacement on.</param>
        /// <param name="oldValue">The <see cref="string"/> value to be replaced.</param>
        /// <param name="newValue">The <see cref="string"/> value to replace all occurrences of <paramref name="oldValue"/>.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison. Default is <see cref="StringComparison.OrdinalIgnoreCase"/>.</param>
        /// <returns>A <see cref="string"/> equivalent to <paramref name="value"/> but with all instances of <paramref name="oldValue"/> replaced with <paramref name="newValue"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null -or-
        /// <paramref name="oldValue"/> is null.
        /// </exception>
        public static string ReplaceAll(string value, string oldValue, string newValue, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            Validator.ThrowIfNull(value);
            Validator.ThrowIfNull(oldValue);
            return ReplaceAll(value, Arguments.Yield(new StringReplacePair(oldValue, newValue)), comparison);
        }

        /// <summary>
        /// Replaces all occurrences of the <see cref="OldValue"/> with <see cref="NewValue"/> of the <paramref name="replacePairs"/> sequence in <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to perform the replacement on.</param>
        /// <param name="replacePairs">A sequence of <see cref="StringReplacePair"/> values.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison. Default is <see cref="StringComparison.OrdinalIgnoreCase"/>.</param>
        /// <returns>A <see cref="string"/> equivalent to <paramref name="value"/> but with all instances of <see cref="OldValue"/> replaced with <see cref="NewValue"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null -or-
        /// <paramref name="replacePairs"/> is null.
        /// </exception>
        public static string ReplaceAll(string value, IEnumerable<StringReplacePair> replacePairs, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            Validator.ThrowIfNull(value);
            Validator.ThrowIfNull(replacePairs);
            var replaceEngine = new StringReplaceEngine(value, replacePairs, comparison);
            return replaceEngine.ToString();
        }

        /// <summary>
        /// Returns a new string in which all the specified <paramref name="fragments"/> has been removed from the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to perform the sweep on.</param>
        /// <param name="fragments">The fragments containing the characters and/or words to delete.</param>
        /// <returns>A new string that is equivalent to <paramref name="value"/> except for the removed characters and/or words.</returns>
        /// <remarks>This method performs an ordinal (case-sensitive and culture-insensitive) comparison. The search begins at the first character position of this string and continues through the last character position.</remarks>
        public static string RemoveAll(string value, params string[] fragments)
        {
            return RemoveAll(value, StringComparison.Ordinal, fragments);
        }

        /// <summary>
        /// Returns a new string in which all the specified <paramref name="fragments"/> has been deleted from the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to perform the sweep on.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <param name="fragments">The fragments containing the characters and/or words to delete.</param>
        /// <returns>A new string that is equivalent to <paramref name="value"/> except for the removed characters and/or words.</returns>
        public static string RemoveAll(string value, StringComparison comparison, params string[] fragments)
        {
            if (string.IsNullOrEmpty(value)) { return value; }
            if (fragments == null || fragments.Length == 0) { return value; }
            foreach (var f in fragments)
            {
                value = ReplaceAll(value, f, "", comparison);
            }
            return value;
        }

        /// <summary>
        /// Returns a new string in which all the specified <paramref name="fragments"/> has been deleted from the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to perform the sweep on.</param>
        /// <param name="fragments">The fragments containing the characters and/or words to delete.</param>
        /// <returns>A new string that is equivalent to <paramref name="value"/> except for the removed characters.</returns>
        public static string RemoveAll(string value, params char[] fragments)
        {
            if (string.IsNullOrEmpty(value)) { return value; }
            var result = new StringBuilder(value.Length);
            foreach (var c in value)
            {
                if (fragments.Contains(c)) { continue; }
                result.Append(c);
            }
            return result.ToString();
        }

        /// <summary>
        /// Returns a new string array in which all the specified <paramref name="fragments"/> has been deleted from the specified <paramref name="source"/> array.
        /// </summary>
        /// <param name="source">The <see cref="string"/> array value to perform the sweep on.</param>
        /// <param name="fragments">The fragments containing the characters and/or words to delete.</param>
        /// <returns>A new string array that is equivalent to <paramref name="source"/> except for the removed characters and/or words.</returns>
        /// <remarks>This method performs an ordinal (case-sensitive and culture-insensitive) comparison. The search begins at the first character position of this string and continues through the last character position.</remarks>
        public static string[] RemoveAll(string[] source, params string[] fragments)
        {
            return RemoveAll(source, StringComparison.Ordinal, fragments);
        }

        /// <summary>
        /// Returns a new string array in which all the specified <paramref name="fragments"/> has been deleted from the specified <paramref name="source"/> array.
        /// </summary>
        /// <param name="source">The <see cref="string"/> array value to perform the sweep on.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <param name="fragments">The fragments containing the characters and/or words to delete.</param>
        /// <returns>A new string array that is equivalent to <paramref name="source"/> except for the removed characters and/or words.</returns>
        public static string[] RemoveAll(string[] source, StringComparison comparison, params string[] fragments)
        {
            if (source == null || source.Length == 0) { return source; }
            if (fragments == null || fragments.Length == 0) { return source; }
            var result = new List<string>();
            foreach (var s in source)
            {
                result.Add(RemoveAll(s, comparison, fragments));
            }
            return result.ToArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringReplacePair"/> struct.
        /// </summary>
        /// <param name="oldValue">The <see cref="string"/> value to be replaced.</param>
        /// <param name="newValue">The <see cref="string"/> value to replace all occurrences of <paramref name="oldValue"/>.</param>
        public StringReplacePair(string oldValue, string newValue) : this()
        {
            NewValue = newValue;
            OldValue = oldValue;
        }

        /// <summary>
        /// Gets the <see cref="string"/> value to be replaced.
        /// </summary>
        /// <value>The <see cref="string"/> value to be replaced.</value>
        public string OldValue { get; }

        /// <summary>
        /// Gets the <see cref="string"/> value to replace all occurrences of <see cref="OldValue"/>.
        /// </summary>
        /// <value>The <see cref="string"/> value to replace all occurrences of <see cref="OldValue"/>.</value>
        public string NewValue { get; }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return OldValue.GetHashCode() ^ NewValue.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is not StringReplacePair pair) { return false; }
            return Equals(pair);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the other parameter; otherwise, <c>false</c>. </returns>
        public bool Equals(StringReplacePair other)
        {
            if ((OldValue != other.OldValue)) { return false; }
            return (NewValue == other.NewValue);
        }

        /// <summary>
        /// Indicates whether two <see cref="StringReplacePair"/> instances are equal.
        /// </summary>
        /// <param name="replacePair1">The first date interval to compare.</param>
        /// <param name="replacePair2">The second date interval to compare.</param>
        /// <returns><c>true</c> if the values of <paramref name="replacePair1"/> and <paramref name="replacePair2"/> are equal; otherwise, false. </returns>
        public static bool operator ==(StringReplacePair replacePair1, StringReplacePair replacePair2)
        {
            return replacePair1.Equals(replacePair2);
        }

        /// <summary>
        /// Indicates whether two <see cref="DateSpan"/> instances are not equal.
        /// </summary>
        /// <param name="replacePair1">The first date interval to compare.</param>
        /// <param name="replacePair2">The second date interval to compare.</param>
        /// <returns><c>true</c> if the values of <paramref name="replacePair1"/> and <paramref name="replacePair2"/> are not equal; otherwise, false.</returns>
        public static bool operator !=(StringReplacePair replacePair1, StringReplacePair replacePair2)
        {
            return !replacePair1.Equals(replacePair2);
        }

        /// <summary>
        /// Returns a string representation of the <see cref="StringReplacePair"/>, using the string representations of the oldValue and newValue.
        /// </summary>
        /// <returns>A string representation of the <see cref="StringReplacePair"/>, which includes the string representations of the oldValue and newValue.</returns>
        /// <remarks>The string representation consists of the string representations of the oldValue and newValue, separated by a comma and a space, and enclosed in square brackets. For example, the ToString method for a <see cref="StringReplacePair"/> structure with the string OldValue "Test1" and the string NewValue "Test2" returns the string "[Test1, Test2]".</remarks>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append('[');
            if (OldValue != null)
            {
                builder.Append(OldValue);
            }
            builder.Append(", ");
            if (NewValue != null)
            {
                builder.Append(NewValue);
            }
            builder.Append(']');
            return builder.ToString();
        }
    }
}