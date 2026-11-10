// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S3358:Ternary operators should not be nested", Justification = "Clear enough.", Scope = "member", Target = "~M:Cuemon.AspNetCore.Mvc.ContentBasedObjectResult.#ctor(System.Object,System.Byte[],System.Boolean)")]
[assembly: SuppressMessage("Major Code Smell", "S1066:Collapsible \"if\" statements should be merged", Justification = "By design; easier for debug purposes and with clear scope.", Scope = "member", Target = "~M:Cuemon.AspNetCore.Mvc.Filters.Cacheable.HttpLastModifiedHeaderFilter.OnResultExecutionAsync(Microsoft.AspNetCore.Mvc.Filters.ResultExecutingContext,Microsoft.AspNetCore.Mvc.Filters.ResultExecutionDelegate)~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Major Code Smell", "S1066:Collapsible \"if\" statements should be merged", Justification = "By design; easier for debug purposes and with clear scope.", Scope = "member", Target = "~M:Cuemon.AspNetCore.Mvc.Filters.Cacheable.HttpEntityTagHeaderFilter.OnResultExecutionAsync(Microsoft.AspNetCore.Mvc.Filters.ResultExecutingContext,Microsoft.AspNetCore.Mvc.Filters.ResultExecutionDelegate)~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Critical Code Smell", "S3776:Cognitive Complexity of methods should not be too high", Justification = "If i invert the if-statement, the warning goes away - but the code becomes harder to read. So for now, i exclude it as 'by design'.", Scope = "member", Target = "~M:Cuemon.AspNetCore.Mvc.Filters.Diagnostics.FaultDescriptorFilter.OnException(Microsoft.AspNetCore.Mvc.Filters.ExceptionContext)")]
[assembly: SuppressMessage("Minor Code Smell", "S3236:Caller information arguments should not be provided explicitly", Justification = "Intentional.", Scope = "member", Target = "~M:Cuemon.AspNetCore.Mvc.Filters.Diagnostics.FaultDescriptorFilter.#ctor(Microsoft.Extensions.Options.IOptions{Cuemon.AspNetCore.Mvc.Filters.Diagnostics.MvcFaultDescriptorOptions})")]
[assembly: SuppressMessage("Performance", "CA1848:Use the LoggerMessage delegates", Justification = "Lack support for dynamic log-level.", Scope = "member", Target = "~M:Cuemon.AspNetCore.Mvc.Filters.Diagnostics.ServerTimingFilter.OnActionExecuted(Microsoft.AspNetCore.Mvc.Filters.ActionExecutedContext)")]
