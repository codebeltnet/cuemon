using System;
using Cuemon.AspNetCore.Http;
using Cuemon.Configuration;
using Cuemon.Data.Integrity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cuemon.AspNetCore.Mvc.Filters.Cacheable;
/// <summary>
/// Specifies options that is related to the <see cref="HttpLastModifiedHeaderFilter" />.
/// </summary>
/// <seealso cref="HttpCacheableFilter"/>
public class HttpLastModifiedHeaderOptions : IParameterObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HttpLastModifiedHeaderOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="HttpEntityTagHeaderOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="LastModifiedProvider"/></term>
    ///         <description><code>
    ///            (timestamp, context) =>
    ///            {
    ///                Decorator.Enclose(context.Response).TryAddOrUpdateLastModifiedHeader(context.Request, timestamp.Modified ?? timestamp.Created);
    ///            };
    ///         </code></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public HttpLastModifiedHeaderOptions()
    {
        LastModifiedProvider = (timestamp, context) =>
        {
            Decorator.Enclose(context.Response).AddOrUpdateLastModifiedHeader(context.Request, timestamp.Modified ?? timestamp.Created);
        };
    }

    /// <summary>
    /// Gets a value indicating whether this instance has an <see cref="LastModifiedProvider"/>.
    /// </summary>
    /// <value><c>true</c> if this instance has an <see cref="LastModifiedProvider"/>; otherwise, <c>false</c>.</value>
    public bool HasLastModifiedProvider => LastModifiedProvider != null;

    /// <summary>
    /// Gets or sets the delegate that is invoked when a result of a <see cref="ResultExecutingContext"/> is an <see cref="ObjectResult"/> and the value is an <see cref="IEntityDataTimestamp"/> implementation.
    /// </summary>
    /// <value>The delegate that provides an HTTP Last-Modified header.</value>
    public Action<IEntityDataTimestamp, HttpContext> LastModifiedProvider { get; set; }
}
