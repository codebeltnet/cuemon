#if NETSTANDARD2_0_OR_GREATER
using System.Collections.Generic;
using Cuemon.Collections.Generic;

namespace Cuemon.Extensions.Collections.Generic
{
    /// <summary>
    /// Extension methods for the <see cref="Stack{T}"/> class.
    /// </summary>
    public static class StackExtensions
    {
        /// <summary>
        /// Returns a value that indicates whether there is an object at the top of the <see cref="Stack{T}"/>, and if one is present, copies it to the result parameter, and removes it from the <see cref="Stack{T}"/>.
        /// </summary>
        /// <typeparam name="T">Specifies the type of elements in the stack.</typeparam>
        /// <param name="stack">The <see cref="Stack{T}"/> to extend.</param>
        /// <param name="result">If present, the object at the top of the <see cref="Stack{T}"/>; otherwise, the <c>default</c> value of <typeparamref name="T"/>.</param>
        /// <returns><c>true</c> if there is an object at the top of the <see cref="Stack{T}"/>; <c>false</c> if the <see cref="Stack{T}"/> is empty.</returns>
        public static bool TryPop<T>(this Stack<T> stack, out T result)
        {
            return Decorator.EncloseToExpose(stack).TryPop(out result);
        }
    }
}
#endif
