// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Bug", "S2259:Null pointers should not be dereferenced", Justification = "The variable, fieldCount, is initialized to -1 in case of record being null - hence, the for loop will never iterate.", Scope = "member", Target = "~M:Cuemon.Data.DataTransferColumnCollection.#ctor(System.Data.IDataRecord)")]
[assembly: SuppressMessage("Major Bug", "S1751:Loops with at most one iteration should be refactored", Justification = "This is by design and how a reader is implemented. While there are lines to be read, we built a token, and the token is being read. When read, it returns true and proceeds to next line.", Scope = "member", Target = "~M:Cuemon.Data.DsvDataReader.Read~System.Boolean")]
[assembly: SuppressMessage("Major Bug", "S1751:Loops with at most one iteration should be refactored", Justification = "This is by design and how a reader is implemented. While there are lines to be read, we built a token, and the token is being read. When read, it returns true and proceeds to next line.", Scope = "member", Target = "~M:Cuemon.Data.DsvDataReader.ReadAsync~System.Threading.Tasks.Task{System.Boolean}")]
[assembly: SuppressMessage("Major Code Smell", "S907:\"goto\" statement should not be used", Justification = "Legacy implementation.", Scope = "member", Target = "~M:Cuemon.Data.Xml.XmlDataReader.ReadNext(System.Boolean)~System.Boolean")]
[assembly: SuppressMessage("Major Code Smell", "S1168:Empty arrays and collections should be returned instead of null", Justification = "By design. Not relevant in this context.", Scope = "member", Target = "~P:Cuemon.Data.DsvDataReader.NullRead")]
[assembly: SuppressMessage("Minor Code Smell", "S3236:Caller information arguments should not be provided explicitly", Justification = "Intentional.", Scope = "member", Target = "~M:Cuemon.Data.DataManager.OpenConnection(System.Data.IDbCommand)")]
[assembly: SuppressMessage("Style", "IDE0130:Namespace does not match folder structure", Justification = "Intentional as these embark on IDecorator.", Scope = "namespace", Target = "~N:Cuemon.Data")]
