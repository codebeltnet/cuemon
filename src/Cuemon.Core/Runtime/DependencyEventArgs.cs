using System;

namespace Cuemon.Runtime;
/// <summary>
/// Provides data for dependency related operations.
/// </summary>
public class DependencyEventArgs : EventArgs
{
    private DependencyEventArgs()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DependencyEventArgs"/> class.
    /// </summary>
    public DependencyEventArgs(DateTime utcLastModified)
    {
        UtcLastModified = utcLastModified;
    }

    /// <summary>
    /// Gets the <see cref="DateTime"/> value from when a <see cref="Dependency"/> was last changed, or a <see cref="DateTime.MinValue"/> if an empty event.
    /// </summary>
    /// <value>The <see cref="DateTime"/> value from when a <see cref="Dependency"/> was last changed, or a <see cref="DateTime.MinValue"/> if an empty event.</value>
    /// <remarks>This property is measured in Coordinated Universal Time (UTC) (also known as Greenwich Mean Time).</remarks>
    public DateTime UtcLastModified { get; } = DateTime.MinValue;

    /// <summary>
    /// Represents an event with no event data.
    /// </summary>
    public new static readonly DependencyEventArgs Empty = new();
}
