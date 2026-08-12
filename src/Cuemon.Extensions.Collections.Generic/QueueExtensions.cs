#if NETSTANDARD2_0_OR_GREATER
using System.Collections.Generic;

namespace Cuemon.Extensions.Collections.Generic;

/// <summary>
/// Extension methods for the <see cref="Queue{T}"/> class.
/// </summary>
public static class QueueExtensions
{
    /// <summary>
    /// Returns a value that indicates whether there is an object at the beginning of the <paramref name="queue"/>, and if one is present, copies it to the result parameter. The object is not removed from the <paramref name="queue"/>.
    /// </summary>
    /// <typeparam name="T">Specifies the type of elements in the queue.</typeparam>
    /// <param name="queue">The <see cref="Queue{T}"/> to extend.</param>
    /// <param name="result">If present, the object at the beginning of the <see cref="Queue{T}"/>; otherwise, the <c>default</c> value of <typeparamref name="T"/>.</param>
    /// <returns><c>true</c> if there is an object at the beginning of the <see cref="Queue{T}"/>; <c>false</c> if the <see cref="Queue{T}"/> is empty.</returns>
    public static bool TryPeek<T>(this Queue<T> queue, out T result)
    {
        if (queue.Count == 0)
        {
            result = default;
            return false;
        }
        result = queue.Peek();
        return true;
    }
}
#endif
