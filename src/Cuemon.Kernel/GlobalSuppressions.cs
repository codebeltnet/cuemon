// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S3881:\"IDisposable\" should be implemented correctly", Justification = "Dispose(bool) is intentionally non-virtual to centralize disposal state, synchronization, and cleanup exception handling; derived classes extend cleanup through OnDisposeManagedResources and OnDisposeUnmanagedResources.", Scope = "type", Target = "~T:Cuemon.Disposable")]
[assembly: SuppressMessage("Globalization", "CA1304:Specify CultureInfo", Justification = "Delegates to the overload that explicitly uses CultureInfo.InvariantCulture; callers can specify a culture through the three-argument overload.", Scope = "member", Target = "~M:Cuemon.DateSpan.Parse(System.String)~Cuemon.DateSpan")]
[assembly: SuppressMessage("Style", "IDE0130:Namespace does not match folder structure", Justification = "By design.", Scope = "namespace", Target = "~N:System.Runtime.CompilerServices")]
[assembly: SuppressMessage("Style", "IDE0130:Namespace does not match folder structure", Justification = "By design.", Scope = "namespace", Target = "~N:Cuemon.IO")]
