using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Cuemon.Collections.Generic;
using Cuemon.Resilience;
using Microsoft.Data.SqlClient;

namespace Cuemon.Data.SqlClient
{
    /// <summary>
    /// The SqlDataManager is the primary class of the <see cref="SqlClient"/> namespace that can be used to execute commands targeted Microsoft SQL Server.
    /// </summary>
    public class SqlDataManager : DataManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDataManager"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="DataManagerOptions"/> which need to be configured.</param>
        public SqlDataManager(Action<DataManagerOptions> setup) : base(setup)
        {
        }

        /// <summary>
        /// Gets or sets the callback delegate that will provide options for transient fault handling.
        /// </summary>
        /// <value>An <see cref="Action{T}" /> with the options for transient fault handling.</value>
        /// <remarks>
        /// This implementation is compatible with transient related faults on Microsoft SQL Azure.<br/>
        /// Microsoft SQL Server is supported as well.
        /// </remarks>
        public Action<TransientOperationOptions> TransientFaultHandlingOptionsCallback { get; set; } = options =>
        {
            options.EnableRecovery = true;
            options.DetectionStrategy = exception =>
            {
                if (exception == null) { return false; }

                var sqlException = ParseException(exception);
                if (sqlException != null)
                {
                    switch (sqlException.Number)
                    {
                        case -2:
                        case 20:
                        case 64:
                        case 233:
                        case 10053:
                        case 10054:
                        case 10060:
                        case 10928:
                        case 10929:
                        case 40001:
                        case 40143:
                        case 40166:
                        case 40174:
                        case 40197:
                        case 40501:
                        case 40544:
                        case 40549:
                        case 40550:
                        case 40551:
                        case 40552:
                        case 40553:
                        case 40613:
                        case 40615:
                            return true;
                    }
                }

                var fault = exception.Message.StartsWith("Timeout expired.", StringComparison.OrdinalIgnoreCase);
                fault |= exception.Message.IndexOf("The wait operation timed out", StringComparison.OrdinalIgnoreCase) >= 0;
                fault |= exception.Message.IndexOf("The semaphore timeout period has expired", StringComparison.OrdinalIgnoreCase) >= 0;

                return fault;
            };
        };

        /// <summary>
        /// Executes the command statement and returns an identity value as int.
        /// </summary>
        /// <param name="statement">The command statement to execute.</param>
        /// <returns><see cref="int"/></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="statement"/> cannot be null.
        /// </exception>
        public int ExecuteIdentityInt32(DataStatement statement)
        {
            Validator.ThrowIfNull(statement);
            if (statement.Type != CommandType.Text) { throw new ArgumentException("This method only supports CommandType.Text specifications.", nameof(statement)); }
            return ExecuteScalarAs<int>(new DataStatement(FormattableString.Invariant($"{statement.Text} SELECT CONVERT(INT, SCOPE_IDENTITY())"), o =>
            {
                o.Parameters = Arguments.ToArrayOf(statement.Parameters);
                o.Timeout = statement.Timeout;
                o.Type = statement.Type;
            }));
        }

        /// <summary>
        /// Executes the command statement and returns an identity value as long.
        /// </summary>
        /// <param name="statement">The command statement to execute.</param>
        /// <returns><see cref="long"/></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="statement"/> cannot be null.
        /// </exception>
        public long ExecuteIdentityInt64(DataStatement statement)
        {
            Validator.ThrowIfNull(statement);
            if (statement.Type != CommandType.Text) { throw new ArgumentException("This method only supports CommandType.Text specifications.", nameof(statement)); }
            return ExecuteScalarAs<long>(new DataStatement(FormattableString.Invariant($"{statement.Text} SELECT CONVERT(BIGINT, SCOPE_IDENTITY())"), o =>
            {
                o.Parameters = Arguments.ToArrayOf(statement.Parameters);
                o.Timeout = statement.Timeout;
                o.Type = statement.Type;
            }));
        }

        /// <summary>
        /// Executes the command statement and returns an identity value as decimal.
        /// </summary>
        /// <param name="statement">The command statement to execute.</param>
        /// <returns><see cref="decimal"/></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="statement"/> cannot be null.
        /// </exception>
        public decimal ExecuteIdentityDecimal(DataStatement statement)
        {
            Validator.ThrowIfNull(statement);
            Validator.ThrowIfInvalidState(statement.Type != CommandType.Text);
            if (statement.Type != CommandType.Text) { throw new ArgumentException("This method only supports CommandType.Text specifications.", nameof(statement)); }
            return ExecuteScalarAs<decimal>(new DataStatement(FormattableString.Invariant($"{statement.Text} SELECT CONVERT(NUMERIC, SCOPE_IDENTITY())"), o =>
            {
                o.Parameters = Arguments.ToArrayOf(statement.Parameters);
                o.Timeout = statement.Timeout;
                o.Type = statement.Type;
            }));
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public override DataManager Clone()
        {
            return new SqlDataManager(Patterns.ConfigureRevert(Options));
        }

        /// <summary>
        /// Core method for executing methods on the <see cref="System.Data.Common.DbCommand" /> object resolved from the virtual execute command workflow on <see cref="DataManager" />.
        /// </summary>
        /// <typeparam name="T">The type to return.</typeparam>
        /// <param name="statement">The command statement to execute.</param>
        /// <param name="executeSelector">The function delegate that will invoke a method on the resolved <see cref="System.Data.Common.DbCommand" /> object.</param>
        /// <returns>A value of <typeparamref name="T" /> that is equal to the invoked method of the <see cref="System.Data.Common.DbCommand" /> object.</returns>
        /// <remarks>
        /// If <see cref="TransientFaultHandlingOptionsCallback"/> is null, no SQL operation is wrapped inside a transient fault handling operation.
        /// Otherwise, if <see cref="TransientFaultHandlingOptionsCallback"/> has the <see cref="TransientOperationOptions.EnableRecovery"/> set to <c>true</c>, this method will, with it's default implementation, try to gracefully recover from transient faults when the following condition is met:<br/>
        /// <see cref="TransientOperationOptions.RetryAttempts"/> is less than the current attempt starting from 1 with a maximum of <see cref="byte.MaxValue"/> retries<br/>
        /// <see cref="TransientOperationOptions.DetectionStrategy"/> must evaluate to <c>true</c><br/>
        /// In case of a transient failure the default implementation will use <see cref="TransientOperationOptions.RetryStrategy"/>.<br/>
        /// In any other case the originating exception is thrown.
        /// </remarks>
        protected override T ExecuteCommand<T>(DataStatement statement, Func<IDbCommand, T> executeSelector)
        {
            return TransientFaultHandlingOptionsCallback == null
                ? base.ExecuteCommand(statement, executeSelector)
                : TransientOperation.WithFunc(() => base.ExecuteCommand(statement, executeSelector), TransientFaultHandlingOptionsCallback);
        }

        /// <summary>
        /// Gets the command object used by all execute related methods.
        /// </summary>
        /// <param name="statement">The command statement to execute.</param>
        /// <returns>A a new <see cref="SqlCommand"/> instance.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="statement"/> cannot be null.
        /// </exception>
        protected override IDbCommand GetDbCommand(DataStatement statement)
        {
            Validator.ThrowIfNull(statement);
            return Patterns.SafeInvoke(() => new SqlCommand(statement.Text, new SqlConnection(Options.ConnectionString)), sc =>
           {
               AddSqlParameters(sc, statement.Parameters);
               sc.CommandType = statement.Type;
               sc.CommandTimeout = (int)statement.Timeout.TotalSeconds;
               return sc;
           }, ex => throw ExceptionInsights.Embed(new InvalidOperationException("There is an error when creating a new SqlCommand.", ex), MethodBase.GetCurrentMethod(), Arguments.ToArray(statement)));
        }

        private static void AddSqlParameters(SqlCommand command, IEnumerable<IDataParameter> parameters)
        {
            foreach (var parameter in parameters)
            {
                if (parameter is SqlParameter sqlParameter && (sqlParameter.SqlDbType == SqlDbType.SmallDateTime || sqlParameter.SqlDbType == SqlDbType.DateTime))
                {
                    HandleSqlDateTime(sqlParameter); // handle dates so they are compatible with SQL 200X and forward
                }

                parameter.Value ??= DBNull.Value;
                command.Parameters.Add(parameter);
            }
        }

        private static void HandleSqlDateTime(SqlParameter parameter)
        {
            if (parameter.Value != null && DateTime.TryParse(parameter.Value.ToString(), out var dateTime))
            {
                if (dateTime == DateTime.MinValue)
                {
                    parameter.Value = parameter.SqlDbType == SqlDbType.DateTime ? DateTime.Parse("1753-01-01", CultureInfo.InvariantCulture) : DateTime.Parse("1900-01-01", CultureInfo.InvariantCulture);
                }

                if (dateTime == DateTime.MaxValue && parameter.SqlDbType == SqlDbType.SmallDateTime)
                {
                    parameter.Value = DateTime.Parse("2079-06-01", CultureInfo.InvariantCulture);
                }
            }
        }

        private static SqlException ParseException(Exception exception)
        {
            var exceptions = Arguments.Yield(exception).Concat(Decorator.EncloseToExpose(exception).Flatten());
            return exceptions.FirstOrDefault(ex => ex is SqlException) as SqlException;
        }
    }
}
