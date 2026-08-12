namespace Cuemon.Threading;
/// <summary>
/// Defines the most common numerical relational operators.
/// </summary>
/// <remarks>
/// For more information please refer to this Wikipedia article: http://en.wikipedia.org/wiki/Relational_operator#Standard_relational_operators.
/// </remarks>
public enum RelationalOperator
{
    /// <summary>
    /// A comparison for equality (==).
    /// </summary>
    Equal,
    /// <summary>
    /// A comparison for inequality (!=).
    /// </summary>
    NotEqual,
    /// <summary>
    /// A comparison for greater than (&gt;).
    /// </summary>
    GreaterThan,
    /// <summary>
    /// A comparison for greater than or equal to (&gt;=).
    /// </summary>
    GreaterThanOrEqual,
    /// <summary>
    /// A comparison for less than (&lt;).
    /// </summary>
    LessThan,
    /// <summary>
    /// A comparison for less than or equal to (&lt;=).
    /// </summary>
    LessThanOrEqual
}
