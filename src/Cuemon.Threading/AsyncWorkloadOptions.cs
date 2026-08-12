using System;

namespace Cuemon.Threading;
/// <summary>
/// Configuration options for <see cref="ParallelFactory"/>.
/// </summary>
public class AsyncWorkloadOptions : AsyncOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncWorkloadOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="AsyncWorkloadOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="PartitionSize"/></term>
    ///         <description>2 x <see cref="Environment.ProcessorCount"/></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public AsyncWorkloadOptions()
    {
        PartitionSize = 2 * Environment.ProcessorCount;
    }

    /// <summary>
    /// Gets or sets the size of the partition to allocate work to a set of tasks.
    /// </summary>
    /// <value>The size of the partition to allocate work to a set of tasks.</value>
    public int PartitionSize { get; set; }
}
