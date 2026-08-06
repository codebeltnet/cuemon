using System;
namespace Cuemon.Reflection;
/// <summary>
/// Represent the argument given to a member in the context of reflection.
/// </summary>
/// <seealso cref="MemberParser"/>.
public class MemberArgument
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MemberArgument"/> class.
    /// </summary>
    /// <param name="name">The name of the parameter.</param>
    /// <param name="value">The argument value.</param>
    /// <param name="priority">The priority of this member when performing filtering and/or ordering.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="name"/> cannot be null.
    /// </exception>
    public MemberArgument(string name, object value, int priority = 0)
    {
        Validator.ThrowIfNull(name);
        Name = name;
        Value = value;
        Priority = priority;
    }

    /// <summary>
    /// Gets the name of a parameter.
    /// </summary>
    /// <value>The name of a parameter.</value>
    public string Name { get; }

    /// <summary>
    /// Gets or sets the value of the argument.
    /// </summary>
    /// <value>The value of the argument.</value>
    public object Value { get; set; }

    /// <summary>
    /// Gets or sets the priority of this member when performing filtering and/or ordering.
    /// </summary>
    /// <value>The priority of this member when performing filtering and/or ordering.</value>
    public int Priority { get; set; }

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override string ToString()
    {
        return $"[{Name}, {Value?.ToString() ?? ""}, {Priority}]";
    }
}
