using System;

namespace Cuemon.Threading;

/// <summary>
/// Specifies the rules of a for-loop control flow statement.
/// </summary>
/// <typeparam name="TOperand">The type of the number used with the loop control variable.</typeparam>
public class ForLoopRuleset<TOperand> where TOperand : struct, IComparable<TOperand>, IEquatable<TOperand>, IConvertible
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ForLoopRuleset{TOperand}"/> class.
    /// </summary>
    /// <exception cref="TypeArgumentOutOfRangeException">
    /// <typeparamref name="TOperand"/> is outside the range of allowed types.<br/>
    /// Allowed types are: <see cref="byte"/>, <see cref="decimal"/>, <see cref="double"/>, <see cref="short"/>, <see cref="int"/>, <see cref="long"/>, <see cref="sbyte"/>, <see cref="float"/>, <see cref="ushort"/>, <see cref="uint"/> or <see cref="ulong"/>.
    /// </exception>
    public ForLoopRuleset()
    {
        Calculator.ValidAsNumericOperand<TOperand>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ForLoopRuleset{TOperand}"/> class.
    /// </summary>
    /// <param name="from">The rules of a for-loop control flow statement.</param>
    /// <param name="to">The conditional value of the loop control variable.</param>
    /// <param name="step">The value to assign the loop control variable.</param>
    /// <param name="relation">The relation between the loop control variable <paramref name="from"/> and <paramref name="to"/>.</param>
    /// <param name="assignment">The assignment statement of the loop control variable using <paramref name="step"/>.</param>
    /// <param name="condition">The function delegate that represents the condition section of the for loop. Default value is <see cref="AdvancedParallelFactory.Condition{T}"/>.</param>
    /// <param name="iterator">The function delegate that represents the iterator section of the for loop. Default value is <see cref="AdvancedParallelFactory.Iterator{T}"/>.</param>
    /// <exception cref="TypeArgumentOutOfRangeException">
    /// <typeparamref name="TOperand"/> is outside the range of allowed types.<br/>
    /// Allowed types are: <see cref="byte"/>, <see cref="decimal"/>, <see cref="double"/>, <see cref="short"/>, <see cref="int"/>, <see cref="long"/>, <see cref="sbyte"/>, <see cref="float"/>, <see cref="ushort"/>, <see cref="uint"/> or <see cref="ulong"/>.
    /// </exception>
    public ForLoopRuleset(TOperand from, TOperand to, TOperand step, RelationalOperator relation = RelationalOperator.LessThan, AssignmentOperator assignment = AssignmentOperator.Addition, Func<TOperand, RelationalOperator, TOperand, bool> condition = null, Func<TOperand, AssignmentOperator, TOperand, TOperand> iterator = null)
    {
        Calculator.ValidAsNumericOperand<TOperand>();
        From = from;
        To = to;
        Step = step;
        Relation = relation;
        Assignment = assignment;
        Condition = condition ?? AdvancedParallelFactory.Condition;
        Iterator = iterator ?? AdvancedParallelFactory.Iterator;
    }

    /// <summary>
    /// Gets or sets the initial value of the loop control variable.
    /// </summary>
    /// <value>The initial value of the loop control variable.</value>
    public TOperand From { get; set; }

    /// <summary>
    /// Gets or sets the relation between the loop control variable <see cref="From"/> and <see cref="To"/>.
    /// </summary>
    /// <value>The relation between the loop control variable.</value>
    public RelationalOperator Relation { get; set; }

    /// <summary>
    /// Gets or sets the conditional value of the loop control variable.
    /// </summary>
    /// <value>The conditional value of the loop control variable.</value>
    public TOperand To { get; set; }

    /// <summary>
    /// Gets or sets the assignment statement of the loop control variable using <see cref="Step"/>.
    /// </summary>
    /// <value>The assignment statement of the loop control variable.</value>
    public AssignmentOperator Assignment { get; set; }

    /// <summary>
    /// Gets or sets the number to assign the loop control variable.
    /// </summary>
    /// <value>The number to assign the loop control variable.</value>
    public TOperand Step { get; set; }

    /// <summary>
    /// Gets or sets the function delegate of a for-condition.
    /// </summary>
    /// <value>The function delegate of a for-condition.</value>
    public Func<TOperand, RelationalOperator, TOperand, bool> Condition { get; set; }

    /// <summary>
    /// Gets or sets the function delegate of a for-iterator.
    /// </summary>
    /// <value>The function delegate of a for-iterator.</value>
    public Func<TOperand, AssignmentOperator, TOperand, TOperand> Iterator { get; set; }
}
