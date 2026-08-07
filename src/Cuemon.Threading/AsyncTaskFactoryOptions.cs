using System;
using System.Threading.Tasks;

namespace Cuemon.Threading;
/// <summary>
/// Configuration options for <see cref="ParallelFactory"/>.
/// </summary>
public class AsyncTaskFactoryOptions : AsyncWorkloadOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncTaskFactoryOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="AsyncTaskFactoryOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="AsyncWorkloadOptions.PartitionSize"/></term>
    ///         <description>2 x <see cref="Environment.ProcessorCount"/></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="Scheduler"/></term>
    ///         <description><see cref="TaskScheduler.Current"/></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="CreationOptions"/></term>
    ///         <description><see cref="TaskCreationOptions.LongRunning"/></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public AsyncTaskFactoryOptions()
    {
        CreationOptions = TaskCreationOptions.LongRunning;
        Scheduler = TaskScheduler.Current;
    }

    /// <summary>
    /// Gets or sets the <see cref="TaskCreationOptions"/> used to create the task.
    /// </summary>
    /// <value>The <see cref="TaskCreationOptions"/> used to create the task.</value>
    public TaskCreationOptions CreationOptions { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="TaskScheduler"/> that is used to schedule the task.
    /// </summary>
    /// <value>The <see cref="TaskScheduler"/> that is used to schedule the task.</value>
    public TaskScheduler Scheduler { get; set; }
}
