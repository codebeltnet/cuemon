using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using Cuemon.Threading;

namespace Cuemon.AspNetCore.Authentication
{
    /// <summary>
    /// Provides a default in-memory implementation of the <see cref="INonceTracker"/> interface.
    /// </summary>
    /// <seealso cref="Disposable" />
    /// <seealso cref="INonceTracker" />
    public class MemoryNonceTracker : Disposable, INonceTracker
    {
        private readonly ConcurrentDictionary<string, NonceTrackerEntry> _entries = new();
        private readonly Timer _expirationTimer;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryNonceTracker"/> class.
        /// </summary>
        public MemoryNonceTracker()
        {
            _expirationTimer = TimerFactory.CreateNonCapturingTimer(state => ((MemoryNonceTracker)state).OnAutomatedSweepCleanup(), this, TimeSpan.FromMinutes(15), TimeSpan.FromHours(1));
        }

        /// <summary>
        /// Attempts to get the <see cref="NonceTrackerEntry" /> associated with the specified <paramref name="nonce" /> from the tracker.
        /// </summary>
        /// <param name="nonce">The unique identifier of the tracker.</param>
        /// <param name="entry">When this method returns, contains the entry associated with the specified <paramref name="nonce" />, or <c>null</c> if the operation failed.</param>
        /// <returns><c>true</c> if the <paramref name="entry" /> was found in the tracker; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="nonce"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="nonce"/> cannot be empty or consist only of white-space characters.
        /// </exception>
        public bool TryGetEntry(string nonce, out NonceTrackerEntry entry)
        {
            Validator.ThrowIfNullOrWhitespace(nonce);
            return _entries.TryGetValue(nonce, out entry);
        }

        /// <summary>
        /// Attempts to insert a <see cref="NonceTrackerEntry" /> into the tracker.
        /// </summary>
        /// <param name="nonce">The unique identifier of the tracker.</param>
        /// <param name="count">The number or bit string that should be used only once.</param>
        /// <returns><c>true</c> if insertion succeeded; otherwise, <c>false</c> when there is already an entry in the tracker with the same key.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="nonce"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="nonce"/> cannot be empty or consist only of white-space characters.
        /// </exception>
        public bool TryAddEntry(string nonce, int count)
        {
            Validator.ThrowIfNullOrWhitespace(nonce);
            return _entries.TryAdd(nonce, new NonceTrackerEntry(count, DateTime.UtcNow));
        }

        /// <summary>
        /// Attempts to remove an entry from the tracker.
        /// </summary>
        /// <param name="nonce">The unique identifier of the tracker.</param>
        /// <returns><c>true</c> if the entry is removed from the tracker; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="nonce"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="nonce"/> cannot be empty or consist only of white-space characters.
        /// </exception>
        public bool TryRemoveEntry(string nonce)
        {
            Validator.ThrowIfNullOrWhitespace(nonce);
            return _entries.TryRemove(nonce, out _);
        }

        private void OnAutomatedSweepCleanup()
        {
            var utcStaleTime = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(5));
            var entries = _entries.Where(pair => pair.Value.Created <= utcStaleTime).ToList();
            if (entries.Count > 0)
            {
                foreach (var entry in entries)
                {
                    _entries.TryRemove(entry.Key, out _);
                }
            }
        }

        /// <summary>
        /// Called when this object is being disposed by either <see cref="Disposable.Dispose" /> or <see cref="Disposable.Dispose(System.Boolean)" /> having <c>disposing</c> set to <c>true</c> and <see cref="Disposable.Disposed" /> is <c>false</c>.
        /// </summary>
        protected override void OnDisposeManagedResources()
        {
            _expirationTimer?.Dispose();
        }
    }
}