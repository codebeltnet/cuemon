using System;
using System.IO;
using System.Threading.Tasks;
using Cuemon.Security;

namespace Cuemon.Runtime
{
    /// <summary>
    /// Provides a watcher implementation designed to monitor and signal changes applied to a file by raising the <see cref="Watcher.Changed"/> event.
    /// </summary>
    /// <seealso cref="Watcher" />
    public class FileWatcher : Watcher
    {
#if NET9_0_OR_GREATER
        private readonly System.Threading.Lock _lock = new();
#else
        private readonly object _lock = new();
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="FileWatcher"/> class.
        /// </summary>
        /// <param name="path">The file to monitor, in standard or Universal Naming Convention (UNC) notation.</param>
        /// <param name="readFile">if set to <c>true</c> the file specified in <paramref name="path"/> will be opened and a checksum will be computed using <see cref="CyclicRedundancyCheck64"/> algorithm.</param>
        /// <param name="setup">The <see cref="WatcherOptions" /> which may be configured.</param>
        public FileWatcher(string path, bool readFile = false, Action<WatcherOptions> setup = null) : base(setup)
        {
            Validator.ThrowIfNullOrWhitespace(path);
            Path = path;
            ReadFile = readFile;
            UtcCreated = UtcLastModified;
            Checksum = null;
        }

        /// <summary>
        /// Gets the checksum that is associated with the file specified in <see cref="Path"/>.
        /// </summary>
        /// <value>The checksum that is associated with the file specified in <see cref="Path"/>.</value>
        /// <remarks>If <see cref="ReadFile"/> is <c>false</c> this property will remain <c>null</c>.</remarks>
        public string Checksum { get; private set; }

        /// <summary>
        /// Gets the time, in Coordinated Universal Time (UTC), when this instance was created.
        /// </summary>
        /// <value>The time, in Coordinated Universal Time (UTC), when this instance was created.</value>
        public DateTime UtcCreated { get; }

        /// <summary>
        /// Gets the path of the file to watch.
        /// </summary>
        /// <value>The path to monitor.</value>
        public string Path { get; }

        /// <summary>
        /// Gets a value indicating whether the file specified in <see cref="Path"/> will be opened, read and assign the computed value to <see cref="Checksum"/>.
        /// </summary>
        /// <value><c>true</c> if the file specified in <see cref="Path"/> will be opened, read and assign the computed value to <see cref="Checksum"/>; otherwise, <c>false</c>.</value>
        public bool ReadFile { get; }

        /// <summary>
        /// Handles the signaling of this <see cref="FileWatcher" />.
        /// </summary>
        protected override Task HandleSignalingAsync()
        {
            lock (_lock)
            {
                var utcLastModified = File.GetLastWriteTimeUtc(Path);
                if (ReadFile)
                {
                    using (var stream = new FileStream(Path, FileMode.Open, FileAccess.Read))
                    {
                        stream.Position = 0;
                        var currentChecksum = HashFactory.CreateCrc64().ComputeHash(stream).ToHexadecimalString();

                        Checksum ??= currentChecksum;
                        if (!Checksum.Equals(currentChecksum, StringComparison.OrdinalIgnoreCase))
                        {
                            SetUtcLastModified(utcLastModified);
                            OnChangedRaised();
                        }
                        Checksum = currentChecksum;
                    }
                }
                else if (utcLastModified > UtcLastModified)
                {
                    SetUtcLastModified(utcLastModified);
                    OnChangedRaised();
                }
            }
            return Task.CompletedTask;
        }
    }
}
