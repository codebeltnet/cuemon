using System.Collections.Generic;
using Cuemon.Reflection;

namespace Cuemon.Diagnostics;
/// <summary>
/// Represents a base class for profiler related operations.
/// </summary>
public abstract class Profiler
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Profiler"/> class.
    /// </summary>
    protected Profiler()
    {
    }

    /// <summary>
    /// Gets or sets the information about the member being profiled.
    /// </summary>
    /// <value>The information about the member being profiled.</value>
    public MethodDescriptor Member { get; set; }

    /// <summary>
    /// Gets or sets the data associated with the <see cref="Member"/> being profiled.
    /// </summary>
    /// <value>The data associated with the <see cref="Member"/> being profiled.</value>
    public IDictionary<string, object> Data { get; set; }
}
