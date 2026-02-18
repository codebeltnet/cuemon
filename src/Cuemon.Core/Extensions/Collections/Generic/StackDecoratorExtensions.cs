#if NETSTANDARD2_0_OR_GREATER
using System.Collections.Generic;

namespace Cuemon.Collections.Generic
{
    /// <summary>
    /// Extension methods for the <see cref="Stack{T}"/> class hidden behind the <see cref="IDecorator{T}"/> interface.
    /// </summary>
    /// <seealso cref="IDecorator{T}"/>
    /// <seealso cref="Decorator{T}"/>
    public static class StackDecoratorExtensions
    {
        /// <summary>
        /// Returns a value that indicates whether there is an object at the top of the enclosed <see cref="Stack{T}"/> of the <paramref name="decorator"/>, and if one is present, copies it to the result parameter, and removes it from the enclosed <see cref="Stack{T}"/> of the <paramref name="decorator"/>.
        /// </summary>
        /// <typeparam name="T">Specifies the type of elements in the stack.</typeparam>
        /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
        /// <param name="result">If present, the object at the top of the enclosed <see cref="Stack{T}"/>; otherwise, the <c>default</c> value of <typeparamref name="T"/>.</param>
        /// <returns><c>true</c> if there is an object at the top of the enclosed <see cref="Stack{T}"/>; <c>false</c> if the enclosed <see cref="Stack{T}"/> is empty.</returns>
        public static bool TryPop<T>(this IDecorator<Stack<T>> decorator, out T result)
        {
            Validator.ThrowIfNull(decorator);
            var stack = decorator.Inner;
            if (stack.Count > 0)
            {
                result = stack.Pop();
                return true;
            }
            result = default;
            return false;
        }
    }
}
#endif
