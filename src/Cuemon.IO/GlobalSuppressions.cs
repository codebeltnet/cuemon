// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S2436:Types and methods should not have too many generic parameters", Justification = "By design; allow up to a max. of 5 generic parameters.", Scope = "member", Target = "~M:Cuemon.IO.StreamFactory.Create``5(System.Action{System.IO.StreamWriter,``0,``1,``2,``3,``4},``0,``1,``2,``3,``4,System.Action{Cuemon.IO.StreamWriterOptions})~System.IO.Stream")]
[assembly: SuppressMessage("Major Code Smell", "S2436:Types and methods should not have too many generic parameters", Justification = "By design; allow up to a max. of 5 generic parameters.", Scope = "member", Target = "~M:Cuemon.IO.StreamFactory.Create``4(System.Action{System.IO.StreamWriter,``0,``1,``2,``3},``0,``1,``2,``3,System.Action{Cuemon.IO.StreamWriterOptions})~System.IO.Stream")]
[assembly: SuppressMessage("Major Code Smell", "S2436:Types and methods should not have too many generic parameters", Justification = "By design; allow up to a max. of 5 generic parameters.", Scope = "member", Target = "~M:Cuemon.IO.StreamFactory.Create``4(System.Action{System.Buffers.IBufferWriter{System.Byte},``0,``1,``2,``3},``0,``1,``2,``3,System.Action{Cuemon.IO.BufferWriterOptions})~System.IO.Stream")]
[assembly: SuppressMessage("Major Code Smell", "S2436:Types and methods should not have too many generic parameters", Justification = "By design; allow up to a max. of 5 generic parameters.", Scope = "member", Target = "~M:Cuemon.IO.StreamFactory.Create``5(System.Action{System.Buffers.IBufferWriter{System.Byte},``0,``1,``2,``3,``4},``0,``1,``2,``3,``4,System.Action{Cuemon.IO.BufferWriterOptions})~System.IO.Stream")]
[assembly: SuppressMessage("Style", "IDE0130:Namespace does not match folder structure", Justification = "Intentional as these embark on IDecorator.", Scope = "namespace", Target = "~N:Cuemon.IO")]
[assembly: SuppressMessage("CodeQuality", "IDE0076:Invalid global 'SuppressMessageAttribute'", Justification = "Only applicable for TFM netstandard2.0.")]
