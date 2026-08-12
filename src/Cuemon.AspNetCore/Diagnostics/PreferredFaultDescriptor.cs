using System;

namespace Cuemon.AspNetCore.Diagnostics;
/// <summary>
/// Specifies the preferred output format of an <see cref="Exception"/> raised in the context of either vanilla ASP.NET or ASP.NET MVC.
/// </summary>
public enum PreferredFaultDescriptor
{
    /// <summary>
    /// Produces the original format based on <see cref="HttpExceptionDescriptor"/>.
    /// </summary>
    FaultDetails,

    /// <summary>
    /// Produces machine-readable format for specifying errors in HTTP API responses based on https://tools.ietf.org/html/rfc7807.
    /// </summary>
    ProblemDetails
}
