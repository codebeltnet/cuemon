using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Cuemon.Data;
/// <summary>
/// An abstract class for building T-SQL statements from table and columns definitions.
/// </summary>
public abstract class QueryBuilder
{
    private int _readLimit = 1000;
    private StringBuilder _query;

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="QueryBuilder"/> class.
    /// </summary>
    protected QueryBuilder()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryBuilder"/> class.
    /// </summary>
    /// <param name="tableName">The name of the table or view.</param>
    /// <param name="keyColumns">The key columns to be used in this <see cref="QueryBuilder"/> instance.</param>
    protected QueryBuilder(string tableName, IDictionary<string, string> keyColumns)
    {
        TableName = tableName;
        KeyColumns = keyColumns;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryBuilder"/> class.
    /// </summary>
    /// <param name="tableName">The name of the table or view.</param>
    /// <param name="keyColumns">The key columns to be used in this <see cref="QueryBuilder"/> instance.</param>
    /// <param name="columns">The none-key columns to be used in this <see cref="QueryBuilder"/> instance.</param>
    protected QueryBuilder(string tableName, IDictionary<string, string> keyColumns, IDictionary<string, string> columns)
    {
        TableName = tableName;
        KeyColumns = keyColumns;
        Columns = columns;
    }
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets a value limiting the maximum amount of records that can be retrieved from a repository. Default is 1000.
    /// </summary>
    /// <value>
    /// The maximum amount of records that can be retrieved from a repository.
    /// </value>
    public int ReadLimit
    {
        get => _readLimit;
        set
        {
            Validator.ThrowIfLowerThanOrEqual(value, 0, nameof(value), "Value must be a positive number.");
            _readLimit = value;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether a query is restricted in how many records (<see cref="ReadLimit"/>) can be retrieved from a repository. Default is false.
    /// </summary>
    /// <value>
    ///   <c>true</c> if a query is restricted in how many records (<see cref="ReadLimit"/>) can be retrieved from a repository; otherwise, <c>false</c>.
    /// </value>
    public bool EnableReadLimit { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether an encapsulation should be committed automatically on table and column names.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if an encapsulation should be committed automatically on table and column names; otherwise, <c>false</c>.
    /// </value>
    public bool EnableTableAndColumnEncapsulation { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the data source should try to prevent locking from readonly queries.
    /// </summary>
    /// <value><c>true</c> if the data source should try to prevent locking from readonly queries; otherwise, <c>false</c>.</value>
    public bool EnableDirtyReads { get; set; }

    /// <summary>
    /// Gets or sets the name of the table or view.
    /// </summary>
    /// <value>The name of the table or view.</value>
    public string TableName { get; set; }

    /// <summary>
    /// Gets the none-key columns to be used in the <see cref="QueryBuilder"/> instance.
    /// </summary>
    /// <value>The none-key columns to be used in the <see cref="QueryBuilder"/> instance.</value>
    public IDictionary<string, string> Columns { get; }

    /// <summary>
    /// Gets the key columns to be used in the <see cref="QueryBuilder"/> instance.
    /// </summary>
    /// <value>The key columns to be used in the <see cref="QueryBuilder"/> instance.</value>
    public IDictionary<string, string> KeyColumns { get; }

    private StringBuilder Query => _query ??= new StringBuilder(100);
    #endregion

    #region Methods
    /// <summary>
    /// Encodes the specified sequence of <paramref name="values"/> into the desired <paramref name="format"/> of fragments.
    /// </summary>
    /// <param name="format">One of the enumeration values that specifies the fragment to produce.</param>
    /// <param name="values">The <see cref="IEnumerable{String}"/> to convert into the desired <paramref name="format"/> of fragments.</param>
    /// <param name="distinct">if set to <c>true</c>, <paramref name="values"/> will be filtered for doublets.</param>
    /// <returns>A query fragment in the desired format.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="values"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="values"/> contains no elements.
    /// </exception>
    public static string EncodeFragment(QueryFormat format, IEnumerable<string> values, bool distinct = false)
    {
        Validator.ThrowIfSequenceNullOrEmpty(values, nameof(values));
        if (distinct) { values = new List<string>(values.Distinct()); }
        switch (format)
        {
            case QueryFormat.Delimited:
                return DelimitedString.Create(values);
            case QueryFormat.DelimitedString:
                return DelimitedString.Create(values, o =>
                {
                    o.Delimiter = ",";
                    o.StringConverter = s => FormattableString.Invariant($"'{s}'");
                });
            case QueryFormat.DelimitedSquareBracket:
                return DelimitedString.Create(values, o =>
                {
                    o.Delimiter = ",";
                    o.StringConverter = s => FormattableString.Invariant($"[{s}]");
                });
            default:
                throw new InvalidEnumArgumentException(nameof(format), (int)format, typeof(QueryFormat));
        }
    }

    /// <summary>
    /// Create and returns the query from the specified <see cref="QueryType"/>.
    /// </summary>
    /// <param name="queryType">Type of the query to create.</param>
    /// <returns>The result of the builder as a T-SQL query.</returns>
    public string GetQuery(QueryType queryType)
    {
        return GetQuery(queryType, null);
    }

    /// <summary>
    /// Create and returns the builded query from the specified <see cref="QueryType"/>.
    /// </summary>
    /// <param name="queryType">Type of the query to create.</param>
    /// <param name="tableName">The name of the table or view. Overrides the class wide tableName.</param>
    /// <returns></returns>
    public abstract string GetQuery(QueryType queryType, string tableName);

    /// <summary>
    /// Appends the specified query fragment to the end of this instance.
    /// </summary>
    /// <param name="queryFragment">The query fragment to append.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    protected QueryBuilder Append(string queryFragment)
    {
        Query.Append(queryFragment);
        return this;
    }

    /// <summary>
    /// Appends a formatted query fragment, which contains zero or more format specifications, to the end of this instance.
    /// Each format specification is replaced by the string representation of a corresponding object argument.
    /// </summary>
    /// <param name="queryFragment">The query fragment to append.</param>
    /// <param name="args">An array of objects to format.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    protected QueryBuilder Append(string queryFragment, params object[] args)
    {
        Query.AppendFormat(CultureInfo.InvariantCulture, queryFragment, args);
        return this;
    }

    /// <summary>
    /// Returns a <see cref="System.String"/> that represents the current <see cref="System.Object"/>.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String"/> that represents the current <see cref="System.Object"/>.
    /// </returns>
    public override string ToString()
    {
        return Query.ToString();
    }
    #endregion
}
