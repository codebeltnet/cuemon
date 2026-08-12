using System;

namespace Cuemon.Diagnostics;
/// <summary>
/// Specifies the level of sensitive details to include when serializing an <see cref="ExceptionDescriptor"/>.
/// </summary>
[Flags]
public enum FaultSensitivityDetails
{
    /// <summary>
    /// Specifies that all sensitive details are excluded. This is the default and should always be used when in the confines of a Production environment.
    /// </summary>
    None = 0,
    /// <summary>
    /// Specifies that the <see cref="ExceptionDescriptor.Failure"/> property is included in the serialized result.
    /// </summary>
    Failure = 1,
    /// <summary>
    /// Specifies that <see cref="Exception.StackTrace"/> of the <see cref="ExceptionDescriptor.Failure"/> property is included in the serialized result.
    /// </summary>
    StackTrace = 2,
    /// <summary>
    /// Specifies that <see cref="Exception.Data"/> of the <see cref="ExceptionDescriptor.Failure"/> property is included in the serialized result.
    /// </summary>
    Data = 4,
    /// <summary>
    /// Specifies that the <see cref="ExceptionDescriptor.Evidence"/> property is included in the serialized result.
    /// </summary>
    Evidence = 8,
    /// <summary>
    /// Specifies that the <see cref="ExceptionDescriptor.Failure"/> property and the associated <see cref="Exception.StackTrace"/> is included in the serialized result.
    /// </summary>
    FailureWithStackTrace = Failure | StackTrace,
    /// <summary>
    /// Specifies that the <see cref="ExceptionDescriptor.Failure"/> property and the associated <see cref="Exception.Data"/> is included in the serialized result.
    /// </summary>
    FailureWithData = Failure | Data,
    /// <summary>
    /// Specifies that the <see cref="ExceptionDescriptor.Failure"/> property and the associated <see cref="Exception.StackTrace"/> and <see cref="Exception.Data"/> is included in the serialized result.
    /// </summary>
    FailureWithStackTraceAndData = Failure | StackTrace | Data,
    /// <summary>
    /// Specifies that all details should be included when serializing an <see cref="ExceptionDescriptor"/>. Should not be used in a Production environment.
    /// </summary>
    All = Failure | StackTrace | Data | Evidence
}
