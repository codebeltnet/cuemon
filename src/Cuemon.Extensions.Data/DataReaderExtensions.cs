using System;
using System.Data;
using Cuemon.Data;

namespace Cuemon.Extensions.Data;
/// <summary>
/// Extension methods for the <see cref="IDataReader"/> interface.
/// </summary>
public static class DataReaderExtensions
{
    /// <summary>
    /// Converts the specified <paramref name="reader"/> implementation to a table-like data transfer object.
    /// </summary>
    /// <param name="reader">The reader to be converted.</param>
    /// <returns>A <see cref="DataTransferRowCollection"/> that is the result of the specified <paramref name="reader"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="reader"/> is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="reader"/> is closed.
    /// </exception>
    public static DataTransferRowCollection ToRows(this IDataReader reader)
    {
        return DataTransfer.GetRows(reader);
    }

    /// <summary>
    /// Converts the specified and read-initialized <paramref name="reader"/> implementation to a column-like data transfer object.
    /// </summary>
    /// <param name="reader">The read-initialized reader to be converted.</param>
    /// <returns>A <see cref="DataTransferColumnCollection"/> that is the result of the specified and read-initialized <paramref name="reader"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="reader"/> is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="reader"/> is closed.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Invalid attempt to read from <paramref name="reader"/> when no data is present.
    /// </exception>
    public static DataTransferColumnCollection ToColumns(this IDataReader reader)
    {
        return DataTransfer.GetColumns(reader);
    }
}
