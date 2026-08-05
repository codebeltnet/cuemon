using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace Cuemon.Data
{
    /// <summary>
    /// Represents a collection of <see cref="DataTransferRow"/> objects for a table in a database. This class cannot be inherited.
    /// </summary>
    public sealed class DataTransferRowCollection : IEnumerable<DataTransferRow>
    {
        private IEnumerable<string> _columnNames;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTransferRowCollection"/> class.
        /// </summary>
        /// <param name="reader">The reader to convert.</param>
        internal DataTransferRowCollection(IDataReader reader)
        {
            Validator.ThrowIfNull(reader);
            Validator.ThrowIfTrue(reader.IsClosed, nameof(reader), "Reader was closed.");

            Columns = new DataTransferColumnCollection(reader);

            var rowNumber = 1;
            while (reader.Read())
            {
                DataTransferRows.Add(new DataTransferRow(this, rowNumber));
                var fieldCount = reader.FieldCount;
                var values = new object[fieldCount];
                reader.GetValues(values);
                for (var i = 0; i < fieldCount; i++)
                {
                    var columnType = reader[i].GetType();
                    Data.Add(values[i] == null ? Decorator.Enclose(columnType).GetDefaultValue() : ChangeDbNullToNullWhenApplicable(values[i]));
                }
                rowNumber++;
            }
        }

        private static object ChangeDbNullToNullWhenApplicable(object value)
        {
            return DBNull.Value.Equals(value) ? null : value;
        }

        internal DataTransferColumnCollection Columns { get; }

        internal List<object> Data { get; } = new();

        /// <summary>
        /// Gets the column names that is present in this <see cref="DataTransferRow"/>.
        /// </summary>
        /// <value>The column names of a table-row in a database.</value>
        public IEnumerable<string> ColumnNames => _columnNames ??= Columns.Select(column => column.Name);

        private Collection<DataTransferRow> DataTransferRows { get; } = new();

        /// <summary>
        /// Gets the <see cref="DataTransferRow"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the row to return.</param>
        /// <returns>The specified <see cref="DataTransferRow"/>.</returns>
        public DataTransferRow this[int index] => DataTransferRows[index];

        /// <summary>
        /// Determines whether the <see cref="ICollection{T}" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="ICollection{T}" />.</param>
        /// <returns>true if <paramref name="item" /> is found in the <see cref="ICollection{T}" />; otherwise, false.</returns>
        public bool Contains(DataTransferRow item)
        {
            return DataTransferRows.Contains(item);
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="ICollection{T}" />.
        /// </summary>
        /// <value>The count.</value>
        /// <returns>The number of elements contained in the <see cref="ICollection{T}" />.</returns>
        public int Count => DataTransferRows.Count;


        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="IEnumerator{T}" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<DataTransferRow> GetEnumerator()
        {
            return DataTransferRows.GetEnumerator();
        }


        /// <summary>
        /// Determines the index of a specific item in the <see cref="IList{T}" />.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="IList{T}" />.</param>
        /// <returns>The index of <paramref name="item" /> if found in the list; otherwise, -1.</returns>
        public int IndexOf(DataTransferRow item)
        {
            return DataTransferRows.IndexOf(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
