using System;

namespace Cuemon
{
    /// <summary>
    /// Provides a mechanism for releasing both managed and unmanaged resources with focus on the former.
    /// </summary>
    /// <seealso cref="IDisposable" />
    public abstract class Disposable : IDisposable
    {
#if NET9_0_OR_GREATER
        private readonly System.Threading.Lock _lock = new();
#else
        private readonly object _lock = new();
#endif

        /// <summary>
        /// Gets a value indicating whether this <see cref="Disposable"/> object is disposed.
        /// </summary>
        /// <value><c>true</c> if this <see cref="Disposable"/> object is disposed; otherwise, <c>false</c>.</value>
        public bool Disposed { get; private set; }

        /// <summary>
        /// Called when this object is being disposed by either <see cref="Dispose()"/> or <see cref="Dispose(bool)"/> having <c>disposing</c> set to <c>true</c> and <see cref="Disposed"/> is <c>false</c>.
        /// </summary>
        protected abstract void OnDisposeManagedResources();

        /// <summary>
        /// Called when this object is being disposed by either <see cref="Dispose()"/> or <see cref="Dispose(bool)"/> and <see cref="Disposed"/> is <c>false</c>.
        /// </summary>
        protected virtual void OnDisposeUnmanagedResources()
        {
        }

        /// <summary>
        /// Releases all resources used by the <see cref="Disposable"/> object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="Disposable"/> object and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected void Dispose(bool disposing)
        {
            if (Disposed) { return; }
            lock (_lock)
            {
                if (Disposed) { return; }
                Disposed = true;
                if (disposing)
                {
                    OnDisposeManagedResources();
                }
                OnDisposeUnmanagedResources();
            }
        }
    }
}
