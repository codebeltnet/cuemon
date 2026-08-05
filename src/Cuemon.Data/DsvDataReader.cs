using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Cuemon.Text;

namespace Cuemon.Data
{
    /// <summary>
    /// Provides a way of reading a forward-only stream of rows from a DSV (Delimiter Separated Values) based data source. This class cannot be inherited.
    /// </summary>
    public sealed class DsvDataReader : DataReader<string[]>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DsvDataReader"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="StreamReader"/> object that contains the DSV data.</param>
        /// <param name="header">The header defining the columns of the DSV data. Default is reading the first line of the <paramref name="reader"/>.</param>
        /// <param name="parser">The function delegate that returns a primitive object whose value is equivalent to the provided <see cref="string"/> value. Default is <see cref="ParserFactory.FromValueType"/>.</param>
        /// <param name="setup">The <see cref="FormattingOptions"/> which may be configured.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="header"/> cannot be empty or consist only of white-space characters -or-
        /// <paramref name="header"/> does not contain the <see cref="DelimitedStringOptions.Delimiter"/> specified in <paramref name="setup"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> is null.
        /// </exception>
        public DsvDataReader(StreamReader reader, string header = null, Func<string, object> parser = null, Action<DelimitedStringOptions> setup = null)
        {
            Validator.ThrowIfNull(reader);
            Validator.ThrowIfInvalidConfigurator(setup, out var options);

            if (header == null)
            {
                header = reader.ReadLine();
                Validator.ThrowIfNullOrWhitespace(header);
            }

            if (!header!.Contains(options.Delimiter)) { throw new ArgumentException("Header does not contain the specified delimiter."); }

            Reader = reader;
            var headerFields = DelimitedString.Split(header, setup);
            Header = headerFields;
            Delimiter = options.Delimiter;
            Qualifier = options.Qualifier;
            Parser = parser ?? (s => ParserFactory.FromValueType().Parse(s, o => o.FormatProvider = options.FormatProvider));
            SetFields(headerFields);
        }

        private Func<string, object> Parser { get; set; }

        private StreamReader Reader { get; }

        /// <summary>
        /// Gets the delimiter used to separate fields of this instance.
        /// </summary>
        /// <value>The delimiter used to separate fields of this instance.</value>
        public string Delimiter { get; }

        /// <summary>
        /// Gets the header that defines the field names of this instance.
        /// </summary>
        /// <value>The header that defines the field names of this instance.</value>
        public string[] Header { get; }

        /// <summary>
        /// Gets the qualifier that surrounds a field.
        /// </summary>
        /// <value>The qualifier that surrounds a field.</value>
        public string Qualifier { get; }

        /// <summary>
        /// Gets the currently processed row count of this instance.
        /// </summary>
        /// <value>The currently processed row count of this instance.</value>
        /// <remarks>This property is incremented when the invoked <see cref="Read"/> method returns <c>true</c>.</remarks>
        public override int RowCount { get; protected set; }

        /// <summary>
        /// Gets the value that indicates that no more rows exists.
        /// </summary>
        /// <value>The value that indicates that no more rows exists.</value>
        protected override string[] NullRead => null;

        /// <summary>
        /// Advances this instance to the next record.
        /// </summary>
        /// <returns>A <see cref="string"/> array for as long as there are rows; <see cref="NullRead"/> when no more rows exists.</returns>
        protected override string[] ReadNext(string[] columns)
        {
            if (columns != NullRead)
            {
                if (columns.Length != Header.Length) { throw new InvalidOperationException(FormattableString.Invariant($"Line {RowCount + 1} does not match the expected numbers of columns. Actual columns: {columns.Length}. Expected: {Header.Length}.")); }
                SetFields(columns);
            }
            return columns;
        }

        private void SetFields(string[] columns)
        {
            var fields = new OrderedDictionary(StringComparer.OrdinalIgnoreCase);
            for (var i = 0; i < columns.Length; i++)
            {
                if (fields.Contains(Header[i]))
                {
                    fields[Header[i]] = Parser(columns[i]);
                }
                else
                {
                    fields.Add(Header[i], Parser(columns[i]));
                }
            }
            SetFields(fields);
        }

        /// <summary>
        /// Advances this instance to the next line of the DSV data source.
        /// </summary>
        /// <returns><c>true</c> if there are more lines; otherwise, <c>false</c>.</returns>
        /// <exception cref="ObjectDisposedException">
        /// This instance has been disposed.
        /// </exception>
        public override bool Read()
        {
            return ReadAllLinesAsync(() => Task.FromResult(Reader.ReadLine())).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Asynchronously advances this instance to the next line of the DSV data source.
        /// </summary>
        /// <returns><c>true</c> if there are more lines; otherwise, <c>false</c>.</returns>
        /// <exception cref="ObjectDisposedException">
        /// This instance has been disposed.
        /// </exception>
        public Task<bool> ReadAsync()
        {
            return ReadAllLinesAsync(Reader.ReadLineAsync);
        }

        private async Task<bool> ReadAllLinesAsync(Func<Task<string>> readLineAsyncCallback)
        {
            Validator.ThrowIfDisposed(Disposed, GetType());
            var line = await readLineAsyncCallback();
            if (line != null)
            {
                var tb = new TokenBuilder(Delimiter, Qualifier, Header.Length).Append(line);
                while (!tb.IsValid && await readLineAsyncCallback() is { } nextLine)
                {
                    tb.Append(nextLine);
                }

                RowCount++;

                return ReadNext(DelimitedString.Split(tb.ToString(), o =>
                {
                    o.Delimiter = Delimiter.ToString(CultureInfo.InvariantCulture);
                    o.Qualifier = Qualifier.ToString(CultureInfo.InvariantCulture);
                })) != NullRead;
            }
            return false;
        }

        /// <summary>
        /// Called when this object is being disposed by either <see cref="Disposable.Dispose()" /> or <see cref="Disposable.Dispose(bool)" /> having <c>disposing</c> set to <c>true</c> and <see cref="Disposable.Disposed" /> is <c>false</c>.
        /// </summary>
        protected override void OnDisposeManagedResources()
        {
            Reader?.Dispose();
        }
    }
}
