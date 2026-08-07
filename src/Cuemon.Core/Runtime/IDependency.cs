using System;
using System.Threading.Tasks;

namespace Cuemon.Runtime;
/// <summary>
/// Specifies that this object supports a method to control dependency related operations.
/// </summary>
public interface IDependency
{
    /// <summary>
    /// Occurs when a <see cref="IDependency"/> object has changed.
    /// </summary>
    event EventHandler<DependencyEventArgs> DependencyChanged;

    /// <summary>
    /// Gets the time when the dependency was last changed.
    /// </summary>
    /// <value>The time when the dependency was last changed.</value>
    /// <remarks>This property is measured in Coordinated Universal Time (UTC) (also known as Greenwich Mean Time).</remarks>
    DateTime? UtcLastModified { get; }

    /// <summary>
    /// Gets a value indicating whether the <see cref="IDependency"/> object has changed.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if the <see cref="IDependency"/> object has changed; otherwise, <c>false</c>.
    /// </value>
    bool HasChanged { get; }

    /// <summary>
    /// Starts and performs the necessary dependency tasks of this instance.
    /// </summary>
    void Start();

    /// <summary>
    /// Starts and performs the necessary dependency tasks of this instance.
    /// </summary>
    /// <returns>The task object representing the asynchronous operation.</returns>
    Task StartAsync();
}
