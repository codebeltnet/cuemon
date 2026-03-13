namespace Cuemon
{
    /// <summary>
    /// Provides a mechanism for releasing both managed and unmanaged resources with focus on the latter.
    /// Implements the <see cref="Disposable" />
    /// </summary>
    /// <seealso cref="Disposable" />
    public abstract class FinalizeDisposable : Disposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FinalizeDisposable"/> class.
        /// </summary>
        protected FinalizeDisposable()
        {
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="FinalizeDisposable"/> class.
        /// </summary>
        ~FinalizeDisposable()
        {
            Dispose(false);
        }

        /// <summary>
        /// Called when this object is being disposed by either <see cref="Disposable.Dispose()" /> or <see cref="Disposable.Dispose(bool)" /> having <c>disposing</c> set to <c>true</c> and <see cref="Disposable.Disposed" /> is <c>false</c>.
        /// </summary>
        protected override void OnDisposeManagedResources()
        {
        }

        /// <summary>
        /// Called when this object is being disposed by either <see cref="Disposable.Dispose()" /> or <see cref="Disposable.Dispose(bool)" /> and <see cref="Disposable.Disposed" /> is <c>false</c>.
        /// </summary>
        protected abstract override void OnDisposeUnmanagedResources();
    }
}
