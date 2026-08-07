using System;

namespace Cuemon.Threading;
/// <summary>
/// Provides a factory based way to work with advanced scenarios that encapsulate and re-use existing code while adding support for typically long-running parallel loops and regions.
/// </summary>
/// <seealso cref="ParallelFactory"/>
public static partial class AdvancedParallelFactory
{
    /// <summary>
    /// Provides a default implementation of a for-iterator callback method.
    /// </summary>
    /// <typeparam name="T">The type of the counter in a for-loop.</typeparam>
    /// <param name="current">The current value of the counter in a for-loop.</param>
    /// <param name="assignment">One of the enumeration values that specifies the rules to apply as the assignment operator for left-hand operand <paramref name="current"/> and right-hand operand <paramref name="step"/>.</param>
    /// <param name="step">The value to assign to <paramref name="current"/> according to the rule specified by <paramref name="assignment"/>.</param>
    /// <returns>The computed result of <paramref name="current"/> having the <paramref name="assignment"/> of <paramref name="step"/>.</returns>
    public static T Iterator<T>(T current, AssignmentOperator assignment, T step) where T : struct, IComparable<T>, IEquatable<T>, IConvertible
    {
        Calculator.ValidAsNumericOperand<T>();
        return Calculator.Calculate(current, assignment, step);
    }

    /// <summary>
    /// Provides a default implementation of a for-condition callback method.
    /// </summary>
    /// <typeparam name="T">The type of the counter in a for-loop.</typeparam>
    /// <param name="current">The current value of the counter in a for-loop.</param>
    /// <param name="relational">One of the enumeration values that specifies the rules to apply as the relational operator for left-hand operand <paramref name="current"/> and right-hand operand <paramref name="repeats"/>.</param>
    /// <param name="repeats">The amount of repeats to do according to the rules specified by <paramref name="relational"/>.</param>
    /// <returns><c>true</c> if <paramref name="current"/> does not meet the condition of <paramref name="relational"/> and <paramref name="repeats"/>; otherwise <c>false</c>.</returns>
    public static bool Condition<T>(T current, RelationalOperator relational, T repeats) where T : struct, IComparable<T>, IEquatable<T>, IConvertible
    {
        Calculator.ValidAsNumericOperand<T>();
        switch (relational)
        {
            case RelationalOperator.Equal:
                return current.Equals(repeats);
            case RelationalOperator.GreaterThan:
                return current.CompareTo(repeats) > 0;
            case RelationalOperator.GreaterThanOrEqual:
                return current.CompareTo(repeats) >= 0;
            case RelationalOperator.LessThan:
                return current.CompareTo(repeats) < 0;
            case RelationalOperator.LessThanOrEqual:
                return current.CompareTo(repeats) <= 0;
            case RelationalOperator.NotEqual:
                return !current.Equals(repeats);
            default:
                throw new ArgumentOutOfRangeException(nameof(relational));
        }
    }
}
