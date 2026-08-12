using System;
using System.Collections.Generic;
using System.Globalization;

namespace Cuemon.Data.SqlClient;
/// <summary>
/// A Microsoft SQL implementation of the <see cref="QueryBuilder"/> class.
/// </summary>
public class SqlQueryBuilder : QueryBuilder
{
    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="SqlQueryBuilder"/> class.
    /// </summary>
    public SqlQueryBuilder()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlQueryBuilder"/> class.
    /// </summary>
    /// <param name="tableName">The name of the table or view.</param>
    /// <param name="keyColumns">The key columns to be used in this <see cref="SqlQueryBuilder"/> instance.</param>
    public SqlQueryBuilder(string tableName, IDictionary<string, string> keyColumns)
        : base(tableName, keyColumns)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlQueryBuilder"/> class.
    /// </summary>
    /// <param name="tableName">The name of the table or view.</param>
    /// <param name="keyColumns">The key columns to be used in this <see cref="SqlQueryBuilder"/> instance.</param>
    /// <param name="columns">The none-key columns to be used in this <see cref="SqlQueryBuilder"/> instance.</param>
    public SqlQueryBuilder(string tableName, IDictionary<string, string> keyColumns, IDictionary<string, string> columns)
        : base(tableName, keyColumns, columns)
    {
    }
    #endregion

    #region Methods
    /// <summary>
    /// Create and returns the query from the specified <see cref="QueryType"/>.
    /// </summary>
    /// <param name="queryType">Type of the query to create.</param>
    /// <param name="tableName">The name of the table or view. Overrides the class wide tableName.</param>
    /// <returns>The result of the builder as a T-SQL query.</returns>
    public override string GetQuery(QueryType queryType, string tableName)
    {
        tableName ??= TableName;
        switch (queryType)
        {
            case QueryType.Exists:
                BuildSelectCheckExistsQuery(tableName);
                break;
            case QueryType.Delete:
                BuildDeleteQuery(tableName);
                break;
            case QueryType.Insert:
                BuildInsertQuery(tableName);
                break;
            case QueryType.Select:
                BuildSelectQuery(tableName);
                break;
            case QueryType.Update:
                BuildUpdateQuery(tableName);
                break;
            default:
                throw new ArgumentOutOfRangeException(string.Format(CultureInfo.InvariantCulture, "The given queryType value ('{0}') is not supported!", queryType));
        }
        return ToString();
    }

    private void BuildDeleteQuery(string tableName)
    {
        Append(EnableTableAndColumnEncapsulation ? "DELETE FROM [{0}]" : "DELETE FROM {0}", string.IsNullOrEmpty(tableName) ? TableName : tableName);
        AppendWhereClause();
    }

    private void BuildInsertQuery(string tableName)
    {
        byte i = 0;
        var columns = new string[Columns.Keys.Count];
        var parameters = new string[Columns.Values.Count];

        Columns.Keys.CopyTo(columns, 0);

        foreach (var parameter in Columns.Values)
        {
            parameters[i] = parameter;
            i++;
        }
        if (columns.Length != 0)
        {
            Append(EnableTableAndColumnEncapsulation ? "INSERT INTO [{0}] ({1}) VALUES ({2})" : "INSERT INTO {0} ({1}) VALUES ({2})",
                string.IsNullOrEmpty(tableName) ? TableName : tableName,
                EnableTableAndColumnEncapsulation ? EncodeFragment(QueryFormat.DelimitedSquareBracket, columns) : EncodeFragment(QueryFormat.Delimited, columns),
                EncodeFragment(QueryFormat.Delimited, parameters));
        }
        else
        {
            Append(EnableTableAndColumnEncapsulation ? "INSERT INTO [{0}] DEFAULT VALUES" : "INSERT INTO {0} DEFAULT VALUES", string.IsNullOrEmpty(tableName) ? TableName : tableName);
        }
    }

    private void BuildUpdateQuery(string tableName)
    {
        byte i = 1;
        Append(EnableTableAndColumnEncapsulation ? "UPDATE [{0}] SET" : "UPDATE {0} SET", string.IsNullOrEmpty(tableName) ? TableName : tableName);
        foreach (var column in Columns)
        {
            Append(EnableTableAndColumnEncapsulation ? "[{0}]={1}" : "{0}={1}", column.Key, column.Value);
            if (i < Columns.Count) { Append(","); }
            i++;
        }
        AppendWhereClause();
    }

    private void BuildSelectQuery(string tableName)
    {
        var enableSquareBracketEncapsulationOnTable = EnableTableAndColumnEncapsulation;
        var columns = new string[KeyColumns.Count + Columns.Count];
        KeyColumns.Keys.CopyTo(columns, 0);
        Columns.Keys.CopyTo(columns, KeyColumns.Count);

        Append("SELECT ");
        if (EnableReadLimit)
        {
            Append("TOP {0} ", ReadLimit);
        }
        Append(EnableTableAndColumnEncapsulation ? EncodeFragment(QueryFormat.DelimitedSquareBracket, columns, true) : EncodeFragment(QueryFormat.Delimited, columns, true));
        if (enableSquareBracketEncapsulationOnTable)
        {
            enableSquareBracketEncapsulationOnTable = !(tableName.Contains("[") && tableName.Contains("]")); // check if we have an overridden tableName with square brackets already integrated and reverse the boolean result.
        }
        Append(enableSquareBracketEncapsulationOnTable ? " FROM [{0}]" : " FROM {0}", string.IsNullOrEmpty(tableName) ? TableName : tableName);
        if (EnableDirtyReads) { Append(" WITH(NOLOCK)"); }
        AppendWhereClause();
    }

    private void BuildSelectCheckExistsQuery(string tableName)
    {
        var columns = new string[KeyColumns.Count];
        KeyColumns.Keys.CopyTo(columns, 0);

        Append("SELECT 1 ");
        Append(EnableTableAndColumnEncapsulation ? "FROM [{0}]" : "FROM {0}", string.IsNullOrEmpty(tableName) ? TableName : tableName);
        if (EnableDirtyReads) { Append(" WITH(NOLOCK)"); }
        AppendWhereClause();
    }

    private void AppendWhereClause()
    {
        byte i = 1;
        if (KeyColumns.Count > 0) { Append(" WHERE"); }
        foreach (var keyColumn in KeyColumns)
        {
            Append(EnableTableAndColumnEncapsulation ? " [{0}]{2}{1}" : " {0}{2}{1}", keyColumn.Key, keyColumn.Value ?? "", keyColumn.Value == null ? " IS NULL" : "=");
            if (i < KeyColumns.Count) { Append(" AND"); }
            i++;
        }
    }
    #endregion
}
