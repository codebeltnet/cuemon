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
        /// Returns a <see cref="T:string[]"/> that contain the substrings of <paramref name="value"/> delimited by a <see cref="DelimitedStringOptions.Delimiter"/> that may be quoted by <see cref="DelimitedStringOptions.Qualifier"/>.
        /// </summary>
        /// <param name="value">The value containing substrings and delimiters.</param>
        /// <param name="setup">The <see cref="DelimitedStringOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:string[]"/> that contain the substrings of <paramref name="value"/> delimited by a <see cref="DelimitedStringOptions.Delimiter"/> and optionally surrounded within <see cref="DelimitedStringOptions.Qualifier"/>.</returns>
        /// <remarks>
        /// This method was inspired by two articles on StackOverflow @ http://stackoverflow.com/questions/2807536/split-string-in-c-sharp, https://stackoverflow.com/questions/3776458/split-a-comma-separated-string-with-both-quoted-and-unquoted-strings and https://stackoverflow.com/questions/6542996/how-to-split-csv-whose-columns-may-contain.
        /// The default implementation conforms with the RFC-4180 standard.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// An error occurred while splitting <paramref name="value"/> into substrings separated by <see cref="DelimitedStringOptions.Delimiter"/> and quoted with <see cref="DelimitedStringOptions.Qualifier"/>.
        /// This is typically related to data corruption, eg. a field has not been properly closed with the <see cref="DelimitedStringOptions.Qualifier"/> specified.
        /// </exception>
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
