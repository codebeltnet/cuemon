using System;

namespace Cuemon;
/// <summary>
/// Specifies the system states to capture runtime.
/// </summary>
[Flags]
public enum SystemSnapshots
{
    /// <summary>
    /// Captures nothing.
    /// </summary>
    None = 0,
    /// <summary>
    /// Captures thread information about a system.
    /// </summary>
    CaptureThreadInfo = 1,
    /// <summary>
    /// Captures process information about a system.
    /// </summary>
    CaptureProcessInfo = 2,
    /// <summary>
    /// Captures environment information about a system.
    /// </summary>
    CaptureEnvironmentInfo = 4,
    /// <summary>
    /// Captures all available information about a system. Includes <see cref="CaptureThreadInfo"/>, <see cref="CaptureProcessInfo"/> and <see cref="CaptureEnvironmentInfo"/>
    /// </summary>
    CaptureAll = CaptureThreadInfo | CaptureProcessInfo | CaptureEnvironmentInfo
}
