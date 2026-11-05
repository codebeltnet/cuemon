// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S3881:\"IDisposable\" should be implemented correctly", Justification = "The lifetime of this is tracked separately by ActiveHandler.", Scope = "type", Target = "~T:Cuemon.Extensions.Net.Http.TrackingHttpMessageHandler")]
[assembly: SuppressMessage("Usage", "CA2215:Dispose methods should call base class dispose", Justification = "The lifetime of this is tracked separately by ActiveHandler.", Scope = "member", Target = "~M:Cuemon.Extensions.Net.Http.TrackingHttpMessageHandler.Dispose(System.Boolean)")]
