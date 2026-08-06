// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Performance", "CA1847:Use char literal for a single character lookup", Justification = "Not supported in .NET Standard 2.0 (and not an issue with an extra pico-second).", Scope = "member", Target = "~M:Cuemon.AspNetCore.Authentication.Basic.BasicAuthorizationHeader.#ctor(System.String,System.String)")]
[assembly: SuppressMessage("Style", "IDE0130:Namespace does not match folder structure", Justification = "Intentional as these embark on IDecorator.", Scope = "namespace", Target = "~N:Cuemon.AspNetCore.Authentication")]
[assembly: SuppressMessage("Major Code Smell", "S107:Methods should not have too many parameters", Justification = "By design to support the Digest protocol.", Scope = "member", Target = "~M:Cuemon.AspNetCore.Authentication.Digest.DigestAuthorizationHeader.#ctor(System.String,System.String,System.String,System.String,System.String,System.String,System.String,System.String,System.String,System.String)")]
