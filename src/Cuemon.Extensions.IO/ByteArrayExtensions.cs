using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Cuemon.Extensions.IO;
/// <summary>
/// Extension methods for the <see cref="byte"/> array.
/// </summary>
public static class ByteArrayExtensions
{
    /// <summary>
    /// Converts the specified <paramref name="bytes"/> to its equivalent <see cref="Stream"/> representation.
    /// </summary>
    /// <param name="bytes">The <see cref="byte"/> array to extend.</param>
    /// <returns>A <see cref="Stream"/> that is equivalent to <paramref name="bytes"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="bytes"/> cannot be null.
    /// </exception>
    public static Stream ToStream(this byte[] bytes)
    {
        Validator.ThrowIfNull(bytes);
        return Decorator.Enclose(bytes).ToStream();
    }

    /// <summary>
    /// Converts the specified <paramref name="bytes"/> to its equivalent <see cref="Stream"/> representation.
    /// </summary>
    /// <param name="bytes">The <see cref="byte"/> array to extend.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Stream"/> that is equivalent to <paramref name="bytes"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="bytes"/> cannot be null.
    /// </exception>
    public static Task<Stream> ToStreamAsync(this byte[] bytes, CancellationToken cancellationToken = default)
    {
        Validator.ThrowIfNull(bytes);
        return AsyncPatterns.SafeInvokeAsync<Stream>(() => new MemoryStream(bytes.Length), async (ms, cti) =>
        {
#if NETSTANDARD
#if NETSTANDARD2_0
            await ms.WriteAsync(bytes, 0, bytes.Length, cti).ConfigureAwait(false);
#else
            await ms.WriteAsync(bytes, cti).ConfigureAwait(false);
#endif
#else
            await ms.WriteAsync(bytes.AsMemory(0, bytes.Length), cti).ConfigureAwait(false);
#endif
            ms.Position = 0;
            return ms;
        }, ct: cancellationToken);
    }
}
