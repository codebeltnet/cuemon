using System;
using System.Collections.Generic;

namespace Cuemon.Collections.Generic
{
    /// <summary>
    /// Provides static helper methods for wrapping, projecting, and concatenating arguments as arrays and enumerable sequences.
    /// </summary>
    public static class Arguments
    {
        /// <summary>
        /// Concatenates two arrays.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the input arrays.</typeparam>
        /// <param name="args1">The first array to concatenate.</param>
        /// <param name="args2">The array to concatenate to the first array.</param>
        /// <returns>
        /// A new array that contains the elements of <paramref name="args1"/> followed by the elements of <paramref name="args2"/>.
        /// </returns>
        public static T[] Concat<T>(T[] args1, T[] args2)
        {
            if (args1 == null) { return Array.Empty<T>(); }
            if (args2 == null) { return args1; }
            if (args1.Length == 0 || args2.Length == 0) { return args1.Length == 0 ? args2 : args1; }
            var result = new T[args1.Length + args2.Length];
            args1.CopyTo(result, 0);
            args2.CopyTo(result, args1.Length);
            return result;
        }

        /// <summary>
        /// Returns the specified arguments as an array of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements in <paramref name="args"/>.</typeparam>
        /// <param name="args">The arguments to return as an array of <typeparamref name="T"/>.</param>
        /// <returns>The specified <paramref name="args"/>.</returns>
        /// <remarks>This method performs no conversion. It simply returns the supplied arguments as a <typeparamref name="T"/> array.</remarks>
        public static T[] ToArrayOf<T>(params T[] args)
        {
            return args;
        }

        /// <summary>
        /// Returns the specified arguments as an array of <see cref="object"/>.
        /// </summary>
        /// <param name="args">The arguments to return as an array of <see cref="object"/>.</param>
        /// <returns>The specified <paramref name="args"/>.</returns>
        /// <remarks>This method performs no conversion. It simply returns the supplied arguments as an array of <see cref="object"/>.</remarks>
        public static object[] ToArray(params object[] args)
        {
            return args;
        }

        /// <summary>
        /// Returns the specified arguments as an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements in <paramref name="args"/>.</typeparam>
        /// <param name="args">The arguments to return as an <see cref="IEnumerable{T}"/>.</param>
        /// <returns>The specified <paramref name="args"/> exposed as an <see cref="IEnumerable{T}"/>.</returns>
        /// <remarks>This method performs no conversion. It simply exposes the supplied arguments through the <see cref="IEnumerable{T}"/> interface.</remarks>
        public static IEnumerable<T> ToEnumerableOf<T>(params T[] args)
        {
            return args;
        }

        /// <summary>
        /// Returns the specified arguments as an <see cref="IEnumerable{T}"/> of <see cref="object"/>.
        /// </summary>
        /// <param name="args">The arguments to return as an enumerable sequence of <see cref="object"/>.</param>
        /// <returns>The specified <paramref name="args"/> exposed as an <see cref="IEnumerable{T}"/> of <see cref="object"/>.</returns>
        /// <remarks>This method performs no conversion. It simply exposes the supplied arguments through the <see cref="IEnumerable{T}"/> interface.</remarks>
        public static IEnumerable<object> ToEnumerable(params object[] args)
        {
            return args;
        }

        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> with the specified <paramref name="arg"/> as the only element.
        /// </summary>
        /// <typeparam name="T">The type of the element of <paramref name="arg"/>.</typeparam>
        /// <param name="arg">The <typeparamref name="T"/> to type as <see cref="IEnumerable{T}"/>.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> with the specified <paramref name="arg"/> as the only element.</returns>
        /// <remarks>The <see cref="Yield{T}"/> method has no effect other than to change the compile-time type of <paramref name="arg"/> from <typeparamref name="T"/> to <see cref="IEnumerable{T}"/>.</remarks>
        public static IEnumerable<T> Yield<T>(T arg)
        {
            yield return arg;
        }
    }
}
