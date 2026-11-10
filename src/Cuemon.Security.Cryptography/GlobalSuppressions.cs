// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Preserves the public API name and follows the SHA naming convention used by System.Security.Cryptography.", Scope = "type", Target = "~T:Cuemon.Security.Cryptography.SHA512256")]
