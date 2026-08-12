using System;
using System.Threading.Tasks;

namespace Cuemon.Extensions;
/// <summary>
/// Provides a mechanism for asynchronously releasing both managed and unmanaged resources with focus on the former.
/// </summary>
/// <seealso cref="Disposable" />
/// <seealso cref="IAsyncDisposable" />
public abstract class AsyncDisposable : Disposable, IAsyncDisposable
{
    /// <summary>
    /// Called when this object is being disposed by either <see cref="Disposable.Dispose()"/> or <see cref="Disposable.Dispose(bool)"/> having <c>disposing</c> set to <c>true</c> and <see cref="Disposable.Disposed"/> is <c>false</c>.
    /// </summary>
    /// <remarks>You should almost never override this - unless you want to call it from <see cref="OnDisposeManagedResourcesAsync"/>.</remarks>
    protected override void OnDisposeManagedResources()
    {
    }

    /// <summary>
    /// Called when this object is being disposed by <see cref="DisposeAsync()"/>.
    /// </summary>
    protected abstract ValueTask OnDisposeManagedResourcesAsync();

    /// <summary>
    /// Asynchronously releases the resources used by the <see cref="AsyncDisposable"/>.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> that represents the asynchronous dispose operation.</returns>
    /// <remarks>https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-disposeasync#the-disposeasync-method</remarks>
    public async ValueTask DisposeAsync()
    {
        await OnDisposeManagedResourcesAsync().ConfigureAwait(false);
        Dispose(false);
        GC.SuppressFinalize(this);
    }
}
