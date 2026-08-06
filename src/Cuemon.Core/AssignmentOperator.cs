namespace Cuemon;
/// <summary>
/// Defines the most common assignment operators for numeric operands.
/// </summary>
/// <remarks>
/// For more information please refer to this Wikibooks article: http://en.wikibooks.org/wiki/C_Sharp_Programming/Operators.
/// </remarks>
public enum AssignmentOperator
{
    /// <summary>
    /// An assignment operation, such as (x = y).
    /// </summary>
    Assign,
    /// <summary>
    /// An addition compound assignment operation, such as (x += y).
    /// </summary>
    Addition,
    /// <summary>
    /// A subtraction compound assignment operation, such as (x -= y).
    /// </summary>
    Subtraction,
    /// <summary>
    /// A multiplication compound assignment operation, such as (x *= y).
    /// </summary>
    Multiplication,
    /// <summary>
    /// An division compound assignment operation, such as (x /= y).
    /// </summary>
    Division,
    /// <summary>
    /// An arithmetic remainder compound assignment operation, such as (x %= y).
    /// </summary>
    Remainder,
    /// <summary>
    /// A bitwise or logical AND compound assignment operation, such as (x &amp;= y).
    /// </summary>
    And,
    /// <summary>
    /// A bitwise or logical OR compound assignment, such as (x |= y).
    /// </summary>
    Or,
    /// <summary>
    /// A bitwise or logical XOR compound assignment operation, such as (x ^= y).
    /// </summary>
    ExclusiveOr,
    /// <summary>
    /// A bitwise left-shift compound assignment, such as (x &lt;&lt;= y).
    /// </summary>
    LeftShift,
    /// <summary>
    /// A bitwise left-shift compound assignment, such as (x &gt;&gt;= y).
    /// </summary>
    RightShift
}
