using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Cuemon.Runtime;
using Cuemon.Security;

namespace Cuemon.Data
{
    /// <summary>
    /// Provides a watcher implementation designed to monitor and signal changes applied to a relational database by raising the <see cref="Watcher.Changed"/> event.
    /// </summary>
    /// <seealso cref="Watcher" />
    public class DatabaseWatcher : Watcher
    {
#if NET10_0_OR_GREATER
        private readonly System.Threading.Lock _lock = new();
#else
        private readonly object _lock = new();
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseWatcher"/> class.
        /// </summary>
        /// <param name="connection">The <see cref="IDbConnection"/> used to connect to a database.</param>
        /// <param name="readerFactory">The function delegate that will resolve an implementation of an <see cref="IDataReader"/>.</param>
        /// <param name="setup">The <see cref="WatcherOptions" /> which may be configured.</param>
        public DatabaseWatcher(IDbConnection connection, Func<IDbConnection, IDataReader> readerFactory, Action<WatcherOptions> setup = null) : base(setup)
        {
            Validator.ThrowIfNull(connection);
            Validator.ThrowIfNull(readerFactory);
            Connection = connection;
            ReaderFactory = readerFactory;
        }

        /// <summary>
        /// Gets the <see cref="IDbConnection"/> of this instance.
        /// </summary>
        /// <value>The <see cref="IDbConnection"/> of this instance.</value>
        public IDbConnection Connection { get; }

        /// <summary>
        /// Gets the function delegate that will resolve an implementation of an <see cref="IDataReader"/>.
        /// </summary>
        /// <value>The function delegate that will resolve an implementation of an <see cref="IDataReader"/>.</value>
        public Func<IDbConnection, IDataReader> ReaderFactory { get; }

        /// <summary>
        /// Gets the checksum that is associated with the query specified in <see cref="ReaderFactory"/>.
        /// </summary>
        /// <value>The checksum that is associated with the query specified in <see cref="ReaderFactory"/>.</value>
        public string Checksum { get; private set; }

        /// <summary>
        /// Handles the signaling of this <see cref="DatabaseWatcher" />.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        protected override Task HandleSignalingAsync()
        {
            lock (_lock)
            {
                try
                {
                    if (Connection.State != ConnectionState.Open) { Connection.Open(); }

                    var checksums = new List<long>();
                    using (var reader = ReaderFactory.Invoke(Connection))
                    {
                        while (reader.Read())
                        {
                            var readerValues = new object[reader.FieldCount];
                            reader.GetValues(readerValues);
                            checksums.Add(Generate.HashCode64(readerValues.Where(o => o != DBNull.Value).Select(o => o.ToString())));
                        }
                    }

                    var currentChecksum = HashFactory.CreateCrc64().ComputeHash(checksums.Cast<IConvertible>()).ToHexadecimalString();

                    Checksum ??= currentChecksum;
                    if (!Checksum.Equals(currentChecksum, StringComparison.OrdinalIgnoreCase))
                    {
                        SetUtcLastModified(DateTime.UtcNow);
                        OnChangedRaised();
                    }
                    Checksum = currentChecksum;
                }
                finally
                {
                    Connection.Close();
                }
            }
            return Task.CompletedTask;
        }
    }
}