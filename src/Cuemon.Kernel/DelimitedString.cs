using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Cuemon
{
    /// <summary>
    /// Provides a set of static methods to convert a sequence into a delimited string and break a delimited string into substrings.
    /// </summary>
    public static class DelimitedString
    {
        private static readonly ConcurrentDictionary<(string Delimiter, string Qualifier), Regex> CompiledSplitExpressions = new();

        /// <summary>
        /// Creates a delimited string representation from the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The <see cref="IEnumerable{T}"/> to convert.</param>
        /// <param name="setup">The <see cref="DelimitedStringOptions{T}"/> which may be configured.</param>
        /// <returns>A <see cref="string"/> of delimited values that is a result of <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> cannot be null.
        /// </exception>
        public static string Create<T>(IEnumerable<T> source, Action<DelimitedStringOptions<T>> setup = null)
        {
            Validator.ThrowIfNull(source);
            var options = Patterns.Configure(setup);
            var delimitedValues = new StringBuilder();

            using (var enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext()) { return string.Empty; }

                delimitedValues.Append(options.StringConverter(enumerator.Current));
                while (enumerator.MoveNext())
                {
                    delimitedValues.Append(options.Delimiter);
                    delimitedValues.Append(options.StringConverter(enumerator.Current));
                }
            }

            return delimitedValues.ToString();
        }

        /// <summary>
        /// Splits the specified <paramref name="value"/> into substrings by using the configured <see cref="DelimitedStringOptions.Delimiter"/> and <see cref="DelimitedStringOptions.Qualifier"/>.
        /// </summary>
        /// <param name="value">The delimited string to split.</param>
        /// <param name="setup">The <see cref="DelimitedStringOptions"/> which may be configured.</param>
        /// <returns>
        /// An array of <see cref="string"/> values that contains the substrings extracted from <paramref name="value"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> is <see langword="null"/>, empty, or consists only of white-space characters.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when <paramref name="value"/> cannot be split using the configured <see cref="DelimitedStringOptions.Delimiter"/> and <see cref="DelimitedStringOptions.Qualifier"/>.
        /// This typically indicates malformed input, such as an unclosed qualified field.
        /// </exception>
        /// <remarks>
        /// The default implementation conforms to RFC 4180.
        /// <para>
        /// This implementation was inspired by the following Stack Overflow discussions:
        /// </para>
        /// <list type="bullet">
        /// <item>
        /// <description>https://stackoverflow.com/questions/2807536/split-string-in-c-sharp</description>
        /// </item>
        /// <item>
        /// <description>https://stackoverflow.com/questions/3776458/split-a-comma-separated-string-with-both-quoted-and-unquoted-strings</description>
        /// </item>
        /// <item>
        /// <description>https://stackoverflow.com/questions/6542996/how-to-split-csv-whose-columns-may-contain</description>
        /// </item>
        /// </list>
        /// </remarks>
        public static string[] Split(string value, Action<DelimitedStringOptions> setup = null)
        {
            Validator.ThrowIfNullOrWhitespace(value);
            var options = Patterns.Configure(setup);
            var delimiter = options.Delimiter;
            var qualifier = options.Qualifier;

            if (delimiter.Length == 1 && qualifier.Length == 1) { return SplitSingleCharCsv(value, delimiter[0], qualifier[0]); }

            var key = (delimiter, qualifier);
            var compiledSplit = CompiledSplitExpressions.GetOrAdd(
                key,
                k => new Regex(string.Format(options.FormatProvider, "{0}(?=(?:[^{1}]*{1}[^{1}]*{1})*(?![^{1}]*{1}))", Regex.Escape(k.Delimiter), Regex.Escape(k.Qualifier)), RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromSeconds(2)));

            try
            {
                return compiledSplit.Split(value);
            }
            catch (RegexMatchTimeoutException)
            {
                throw new InvalidOperationException(FormattableString.Invariant($"An error occurred while splitting '{value}' into substrings separated by '{delimiter}' and quoted with '{qualifier}'. This is typically related to data corruption, eg. a field has not been properly closed with the {nameof(options.Qualifier)} specified."));
            }
        }

        private static string[] SplitSingleCharCsv(string value, char delimiter, char qualifier)
        {
            var result = new List<string>();
            var field = new StringBuilder(value.Length); // upper bound heuristic
            bool inQuotes = false;

            for (int i = 0; i < value.Length; i++)
            {
                var c = value[i];

                if (c == delimiter && !inQuotes)
                {
                    result.Add(field.ToString());
                    field.Length = 0; // reuse the builder
                    continue;
                }

                field.Append(c);

                if (c == qualifier)
                {
                    inQuotes = !inQuotes;
                }
            }

            if (inQuotes)
            {
                throw new InvalidOperationException($"An error occurred while splitting '{value}' into substrings separated by '{delimiter}' and quoted with '{qualifier}'. This is typically related to data corruption, eg. a field has not been properly closed with the {nameof(DelimitedStringOptions.Qualifier)} specified.");
            }

            result.Add(field.ToString());

            return result.ToArray();
        }
    }
}
