using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Configuration;

namespace Cuemon.Data;
/// <summary>
/// The DataManager is an abstract class in the <see cref="Data"/> namespace that can be used to implement execute commands of different database providers.
/// </summary>
public abstract class DataManager : Configurable<DataManagerOptions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DataManager"/> class.
    /// </summary>
    /// <param name="setup">The <see cref="DataManagerOptions"/> which need to be configured.</param>
    protected DataManager(Action<DataManagerOptions> setup) : base(Validator.CheckParameter(() =>
    {
        Validator.ThrowIfInvalidConfigurator(setup, out var options);
        return options;
    }))
    {
    }

    /// <summary>
    /// Creates a new object that is a copy of the current instance.
    /// </summary>
    /// <returns>
    /// A new object that is a copy of this instance.
    /// </returns>
    public abstract DataManager Clone();

    /// <summary>
    /// Executes the command statement and returns the number of rows affected.
    /// </summary>
    /// <param name="statement">The command statement to execute.</param>
    /// <returns>
    /// A <b><see cref="int"/></b> value.
    /// </returns>
    public int Execute(DataStatement statement)
    {
        return ExecuteCommand(statement, command =>
        {
            try
            {
                return command.ExecuteNonQuery();
            }
            finally
            {
                if (!Options.LeaveConnectionOpen && command.Connection != null && command.Connection.State != ConnectionState.Closed)
                {
                    command.Connection.Close();
                }
            }
        });
    }

    /// <summary>
    /// Asynchronously executes the command statement and returns the number of rows affected.
    /// </summary>
    /// <param name="statement">The command statement to execute.</param>
    /// <param name="ct">A token to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of rows affected.</returns>
    public Task<int> ExecuteAsync(DataStatement statement, CancellationToken ct = default)
    {
        return ExecuteCommandAsync(statement, async command =>
        {
            try
            {
                return await command.ExecuteNonQueryAsync(ct).ConfigureAwait(false);
            }
            finally
            {
                if (!Options.LeaveConnectionOpen && command.Connection != null && command.Connection.State != ConnectionState.Closed)
                {

#if NETSTANDARD2_0_OR_GREATER
                    command.Connection.Close();
#else
                    await command.Connection.CloseAsync().ConfigureAwait(false);
#endif

                }
            }
        });
    }

    /// <summary>
    /// Executes the command statement and returns an object supporting the IDataReader interface.
    /// </summary>
    /// <param name="statement">The command statement to execute.</param>
    /// <returns>
    /// An object supporting the <b><see cref="IDataReader"/></b> interface.
    /// </returns>
    public IDataReader ExecuteReader(DataStatement statement)
    {
        return ExecuteCommand(statement, dbCommand => dbCommand.ExecuteReader(Options.PreferredReaderBehavior));
    }

    /// <summary>
    /// Asynchronously executes the command statement and returns a <see cref="DbDataReader"/>.
    /// </summary>
    /// <param name="statement">The command statement to execute.</param>
    /// <param name="ct">A token to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an instance of <see cref="DbDataReader"/>.</returns>
    public Task<DbDataReader> ExecuteReaderAsync(DataStatement statement, CancellationToken ct = default)
    {
        return ExecuteCommandAsync(statement, command => command.ExecuteReaderAsync(Options.PreferredReaderBehavior, ct));
    }

    /// <summary>
    /// Executes the command statement and returns a <see cref="string"/>.
    /// </summary>
    /// <param name="statement">The command statement to execute.</param>
    /// <returns>
    /// A <see cref="string"/> object.
    /// </returns>
    public virtual string ExecuteString(DataStatement statement)
    {
        using (var reader = ExecuteReader(statement))
        {
            return Decorator.Enclose(reader).ToEncodedString();
        }
    }

    /// <summary>
    /// Asynchronously executes the command statement and returns a <see cref="string"/>.
    /// </summary>
    /// <param name="statement">The command statement to execute.</param>
    /// <param name="ct">A token to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="string"/>.</returns>
    public virtual async Task<string> ExecuteStringAsync(DataStatement statement, CancellationToken ct = default)
    {
        using (var reader = await ExecuteReaderAsync(statement, ct).ConfigureAwait(false))
        {
            return await Decorator.Enclose(reader).ToEncodedStringAsync().ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Executes the command statement and returns <c>true</c> if one or more records exists; otherwise <c>false</c>.
    /// </summary>
    /// <param name="statement">The command statement to execute.</param>
    /// <returns>
    /// A <b><see cref="bool"/></b> value.
    /// </returns>
    public bool ExecuteExists(DataStatement statement)
    {
        using (var reader = ExecuteReader(statement))
        {
            return reader.Read();
        }
    }

    /// <summary>
    /// Asynchronously executes the command statement and returns <c>true</c> if one or more records exists; otherwise <c>false</c>.
    /// </summary>
    /// <param name="statement">The command statement to execute.</param>
    /// <param name="ct">A token to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="bool"/>.</returns>
    public async Task<bool> ExecuteExistsAsync(DataStatement statement, CancellationToken ct = default)
    {
        using (var reader = await ExecuteReaderAsync(statement, ct).ConfigureAwait(false))
        {
            return await reader.ReadAsync(ct).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Executes the command statement, and returns the value from the first column of the first row in the result set.
    /// Additional columns or rows are ignored.
    /// </summary>
    /// <param name="statement">The command statement to execute.</param>
    /// <returns>The first column of the first row in the result from <paramref name="statement"/>.</returns>
    public object ExecuteScalar(DataStatement statement)
    {
        return ExecuteCommand(statement, command =>
        {
            try
            {
                return command.ExecuteScalar();
            }
            finally
            {
                if (!Options.LeaveConnectionOpen && command.Connection != null && command.Connection.State != ConnectionState.Closed)
                {
                    command.Connection.Close();
                }
            }
        });
    }

    /// <summary>
    /// Asynchronously executes the command statement, and returns the value from the first column of the first row in the result set.
    /// Additional columns or rows are ignored.
    /// </summary>
    /// <param name="statement">The command statement to execute.</param>
    /// <param name="ct">A token to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the first column of the first row in the result from <paramref name="statement"/>.</returns>
    public Task<object> ExecuteScalarAsync(DataStatement statement, CancellationToken ct = default)
    {
        return ExecuteCommandAsync(statement, async command =>
        {
            try
            {
                return await command.ExecuteScalarAsync(ct).ConfigureAwait(false);
            }
            finally
            {
                if (!Options.LeaveConnectionOpen && command.Connection != null && command.Connection.State != ConnectionState.Closed)
                {

#if NETSTANDARD2_0_OR_GREATER
                    command.Connection.Close();
#else
                    await command.Connection.CloseAsync().ConfigureAwait(false);
#endif

                }
            }
        });
    }

    /// <summary>
    /// Executes the command statement, and attempts to convert the first column of the first row in the result set to the specified <paramref name="returnType"/>.
    /// Additional columns or rows are ignored.
    /// </summary>
    /// <param name="statement">The command statement to execute.</param>
    /// <param name="returnType">The type to return the first column value as.</param>
    /// <param name="setup">The <see cref="ObjectFormattingOptions"/> which needs to be configured.</param>
    /// <returns>The first column of the first row in the result from <paramref name="statement"/> as the specified <paramref name="returnType"/>.</returns>
    public virtual object ExecuteScalarAsType(DataStatement statement, Type returnType, Action<ObjectFormattingOptions> setup = null)
    {
        return Decorator.Enclose(ExecuteScalar(statement)).ChangeType(returnType, setup);
    }

    /// <summary>
    /// Asynchronously executes the command statement, and attempts to convert the first column of the first row in the result set to the specified <paramref name="returnType"/>.
    /// Additional columns or rows are ignored.
    /// </summary>
    /// <param name="statement">The command statement to execute.</param>
    /// <param name="returnType">The type to return the first column value as.</param>
    /// <param name="setup">The <see cref="ObjectFormattingOptions"/> which may be configured.</param>
    /// <param name="ct">A token to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the first column of the first column of the first row in the result from <paramref name="statement"/> as the specified <paramref name="returnType"/>.</returns>
    public virtual async Task<object> ExecuteScalarAsTypeAsync(DataStatement statement, Type returnType, Action<ObjectFormattingOptions> setup = null, CancellationToken ct = default)
    {
        return Decorator.Enclose(await ExecuteScalarAsync(statement, ct).ConfigureAwait(false)).ChangeType(returnType, setup);
    }

    /// <summary>
    /// Executes the command statement, and attempts to convert the first column of the first row in the result set to the specified <typeparamref name="TResult"/>.
    /// Additional columns or rows are ignored.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value.</typeparam>
    /// <param name="statement">The command statement to execute.</param>
    /// <param name="setup">The <see cref="ObjectFormattingOptions"/> which may be configured.</param>
    /// <returns>The first column of the first row in the result from <paramref name="statement" /> as <typeparamref name="TResult"/>.</returns>
    /// <remarks>This method uses <see cref="CultureInfo.InvariantCulture"/> when casting the first column of the first row in the result from <paramref name="statement"/>.</remarks>
    /// <exception cref="AggregateException">
    /// The first column of the first row in the result set could not be converted.
    /// </exception>
    /// <remarks>What differs from the <see cref="Convert.ChangeType(object,Type)"/> is, that this converter supports generics and enums. Fallback uses <see cref="TypeDescriptor"/> and checks if the underlying <see cref="IFormatProvider"/> of <see cref="FormattingOptions.FormatProvider"/> is a <see cref="CultureInfo"/>, then this will be used in the conversion together with <see cref="ObjectFormattingOptions.DescriptorContext"/>.</remarks>
    /// <seealso cref="Convert.ChangeType(object,Type)"/>
    /// <seealso cref="TypeDescriptor.GetConverter(Type)"/>
    public virtual TResult ExecuteScalarAs<TResult>(DataStatement statement, Action<ObjectFormattingOptions> setup = null)
    {
        return (TResult)ExecuteScalarAsType(statement, typeof(TResult), setup);
    }

    /// <summary>
    /// Asynchronously executes the command statement, and attempts to convert the first column of the first row in the result set to the specified <typeparamref name="TResult"/>.
    /// Additional columns or rows are ignored.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value.</typeparam>
    /// <param name="statement">The command statement to execute.</param>
    /// <param name="setup">The <see cref="ObjectFormattingOptions"/> which may be configured.</param>
    /// <param name="ct">A token to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the first column of the first column of the first row in the result from <paramref name="statement" /> as <typeparamref name="TResult"/>.</returns>
    /// <remarks>This method uses <see cref="CultureInfo.InvariantCulture"/> when casting the first column of the first row in the result from <paramref name="statement"/>.</remarks>
    /// <exception cref="AggregateException">
    /// The first column of the first row in the result set could not be converted.
    /// </exception>
    /// <remarks>What differs from the <see cref="Convert.ChangeType(object,Type)"/> is, that this converter supports generics and enums. Fallback uses <see cref="TypeDescriptor"/> and checks if the underlying <see cref="IFormatProvider"/> of <see cref="FormattingOptions.FormatProvider"/> is a <see cref="CultureInfo"/>, then this will be used in the conversion together with <see cref="ObjectFormattingOptions.DescriptorContext"/>.</remarks>
    /// <seealso cref="Convert.ChangeType(object,Type)"/>
    /// <seealso cref="TypeDescriptor.GetConverter(Type)"/>
    public virtual async Task<TResult> ExecuteScalarAsAsync<TResult>(DataStatement statement, Action<ObjectFormattingOptions> setup = null, CancellationToken ct = default)
    {
        return (TResult)await ExecuteScalarAsTypeAsync(statement, typeof(TResult), setup, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Core method for executing methods on the <see cref="IDbCommand"/> interface resolved from the abstract <see cref="GetDbCommand"/> method.
    /// </summary>
    /// <typeparam name="T">The type to return.</typeparam>
    /// <param name="statement">The command statement to execute.</param>
    /// <param name="executeSelector">The function delegate that will invoke a method on the resolved <see cref="IDbCommand"/> from the abstract <see cref="GetDbCommand"/> method.</param>
    /// <returns>A value of <typeparamref name="T"/> that is equal to the invoked method of the <see cref="IDbCommand"/> implementation.</returns>
    protected virtual T ExecuteCommand<T>(DataStatement statement, Func<IDbCommand, T> executeSelector)
    {
        Validator.ThrowIfNull(statement);
        Validator.ThrowIfNull(executeSelector);
        T result;
        IDbCommand command = null;
        try
        {
            command = GetDbCommand(statement);
            OpenConnection(command);
            result = executeSelector(command);
        }
        catch (Exception)
        {
            command?.Parameters.Clear();
            throw;
        }
        finally
        {
            if (!Options.LeaveCommandOpen)
            {
                command?.Dispose();
            }
        }
        return result;
    }

    /// <summary>
    /// Asynchronous core method for executing methods on the <see cref="DbCommand"/> resolved from the abstract <see cref="GetDbCommand"/> method.
    /// </summary>
    /// <typeparam name="T">The type to return.</typeparam>
    /// <param name="statement">The command statement to execute.</param>
    /// <param name="executeSelector">The function delegate that will invoke a method on the resolved <see cref="DbCommand"/> from the abstract <see cref="GetDbCommand"/> method.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a value of <typeparamref name="T"/> that is equal to the invoked method of <see cref="DbCommand"/>.</returns>
    protected virtual async Task<T> ExecuteCommandAsync<T>(DataStatement statement, Func<DbCommand, Task<T>> executeSelector)
    {
        Validator.ThrowIfNull(statement);
        Validator.ThrowIfNull(executeSelector);
        T result;
        DbCommand command = null;
        try
        {
            command = GetDbCommand(statement) as DbCommand;
            OpenConnection(command);
            result = await executeSelector(command).ConfigureAwait(false);
        }
        catch (Exception)
        {
            command?.Parameters.Clear();
            throw;
        }
        finally
        {
            if (!Options.LeaveCommandOpen)
            {
                command?.Dispose();
            }
        }
        return result;
    }

    private static void OpenConnection(IDbCommand command)
    {
        Validator.ThrowIfNull(command);
        Validator.ThrowIfNull(command.Connection, $"The connection of the {nameof(command)} was not set.", nameof(command));
        if (command.Connection!.State != ConnectionState.Open) { command.Connection.Open(); }
    }

    /// <summary>
    /// Gets the command object to be used by all execute related methods.
    /// </summary>
    /// <param name="statement">The command statement to execute.</param>
    /// <returns>An instance of a <see cref="IDbCommand"/> implementation.</returns>
    protected abstract IDbCommand GetDbCommand(DataStatement statement);
}
