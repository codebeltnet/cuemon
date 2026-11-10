// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Performance", "CA1848:Use the LoggerMessage delegates", Justification = "Should happen rarely.", Scope = "member", Target = "~M:Cuemon.Extensions.AspNetCore.Authentication.AuthorizationResponseHandler.HandleAsync(Microsoft.AspNetCore.Http.RequestDelegate,Microsoft.AspNetCore.Http.HttpContext,Microsoft.AspNetCore.Authorization.AuthorizationPolicy,Microsoft.AspNetCore.Authorization.Policy.PolicyAuthorizationResult)~System.Threading.Tasks.Task")]
