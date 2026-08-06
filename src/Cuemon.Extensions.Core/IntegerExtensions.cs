using System;
using System.Collections.Generic;

namespace Cuemon.Extensions;
/// <summary>
/// Extension methods for the <see cref="short"/>, <see cref="int"/> and <see cref="long"/> structs.
/// </summary>
public static class IntegerExtensions
{
    /// <summary>
    /// Returns the smaller of two 32-bit signed integers.
    /// </summary>
    /// <param name="value">The first of two 32-bit signed integers to compare.</param>
    /// <param name="maximum">The second of two 32-bit signed integers to compare.</param>
    /// <returns>Parameter <paramref name="value"/> or <paramref name="maximum"/>, whichever is smaller.</returns>
    public static int Min(this int value, int maximum) => Math.Min(value, maximum);

    /// <summary>
    /// Returns the smaller of two 64-bit signed integers.
    /// </summary>
    /// <param name="value">The first of two 64-bit signed integers to compare.</param>
    /// <param name="maximum">The second of two 64-bit signed integers to compare.</param>
    /// <returns>Parameter <paramref name="value"/> or <paramref name="maximum"/>, whichever is smaller.</returns>
    public static long Min(this long value, long maximum) => Math.Min(value, maximum);

    /// <summary>
    /// Returns the smaller of two 16-bit signed integers.
    /// </summary>
    /// <param name="value">The first of two 16-bit signed integers to compare.</param>
    /// <param name="maximum">The second of two 16-bit signed integers to compare.</param>
    /// <returns>Parameter <paramref name="value"/> or <paramref name="maximum"/>, whichever is smaller.</returns>
    public static short Min(this short value, short maximum) => Math.Min(value, maximum);

    /// <summary>
    /// Returns the larger of two 32-bit signed integers.
    /// </summary>
    /// <param name="value">The first of two 32-bit signed integers to compare.</param>
    /// <param name="minimum">The second of two 32-bit signed integers to compare.</param>
    /// <returns>Parameter <paramref name="value"/> or <paramref name="minimum"/>, whichever is larger.</returns>
    public static int Max(this int value, int minimum) => Math.Max(value, minimum);

    /// <summary>
    /// Returns the larger of two 64-bit signed integers.
    /// </summary>
    /// <param name="value">The first of two 64-bit signed integers to compare.</param>
    /// <param name="minimum">The second of two 64-bit signed integers to compare.</param>
    /// <returns>Parameter <paramref name="value"/> or <paramref name="minimum"/>, whichever is larger.</returns>
    public static long Max(this long value, long minimum) => Math.Max(value, minimum);

    /// <summary>
    /// Returns the larger of two 16-bit signed integers.
    /// </summary>
    /// <param name="value">The first of two 16-bit signed integers to compare.</param>
    /// <param name="minimum">The second of two 16-bit signed integers to compare.</param>
    /// <returns>Parameter <paramref name="value"/> or <paramref name="minimum"/>, whichever is larger.</returns>
    public static short Max(this short value, short minimum) => Math.Max(value, minimum);

    /// <summary>
    /// Determines whether the specified <paramref name="value"/> is a prime number.
    /// </summary>
    /// <param name="value">The positive integer to determine whether is a prime number.</param>
    /// <returns><c>true</c> if the specified <paramref name="value"/> is a prime number; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> has a value smaller than 0.
    /// </exception>
    public static bool IsPrime(this int value)
    {
        return Condition.IsPrime(value);
    }

    /// <summary>
    /// Determines whether the specified <paramref name="source"/> is a sequence of countable integrals (hence, integrals being either incremented or decremented with the same cardinality through out the sequence).
    /// </summary>
    /// <param name="source">The value to test for a sequence of countable characters.</param>
    /// <returns><c>true</c> if the specified <paramref name="source"/> is a sequence of countable integrals (hence, integrals being either incremented or decremented with the same cardinality through out the sequence); otherwise, <c>false</c>.</returns>
    public static bool IsCountableSequence(this IEnumerable<int> source)
    {
        return Condition.IsCountableSequence(source);
    }

    /// <summary>
    /// Determines whether the specified <paramref name="source"/> is a sequence of countable integrals (hence, integrals being either incremented or decremented with the same cardinality through out the sequence).
    /// </summary>
    /// <param name="source">The value to test for a sequence of countable characters.</param>
    /// <returns><c>true</c> if the specified <paramref name="source"/> is a sequence of countable integrals (hence, integrals being either incremented or decremented with the same cardinality through out the sequence); otherwise, <c>false</c>.</returns>
    public static bool IsCountableSequence(this IEnumerable<long> source)
    {
        return Condition.IsCountableSequence(source);
    }


    /// <summary>
    /// Determines whether the specified <paramref name="value"/> is an even number.
    /// </summary>
    /// <param name="value">The value to evaluate.</param>
    /// <returns><c>true</c> if the specified <paramref name="value"/> is an even number; otherwise, <c>false</c>.</returns>
    public static bool IsEven(this int value)
    {
        return Condition.IsEven(value);
    }

    /// <summary>
    /// Determines whether the specified <paramref name="value"/> is an odd number.
    /// </summary>
    /// <param name="value">The value to evaluate.</param>
    /// <returns><c>true</c> if the specified <paramref name="value"/> is an odd number; otherwise, <c>false</c>.</returns>
    public static bool IsOdd(this int value)
    {
        return Condition.IsOdd(value);
    }
}
