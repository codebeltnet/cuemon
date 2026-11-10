// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Bug", "S2259:Null pointers should not be dereferenced", Justification = "False-Positive", Scope = "member", Target = "~M:Cuemon.Data.Integrity.CacheValidator.#ctor(Cuemon.Data.Integrity.EntityInfo,System.Func{Cuemon.Security.Hash},Cuemon.Data.Integrity.EntityDataIntegrityMethod)")]
[assembly: SuppressMessage("Style", "IDE0130:Namespace does not match folder structure", Justification = "Intentional as these embark on IDecorator.", Scope = "namespace", Target = "~N:Cuemon.Data.Integrity")]
[assembly: SuppressMessage("Reliability", "CA2022:Avoid inexact read with 'Stream.Read'", Justification = "Not vital in this context.", Scope = "member", Target = "~M:Cuemon.Data.Integrity.DataIntegrityFactory.CreateIntegrity(System.IO.FileInfo,System.Action{Cuemon.Data.Integrity.FileIntegrityOptions})~Cuemon.Data.Integrity.IDataIntegrity")]
