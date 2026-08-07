using System;
using System.Collections.Generic;
using Cuemon.Configuration;
using Microsoft.Net.Http.Headers;

namespace Cuemon.AspNetCore.Mvc.Filters.Cacheable;
/// <summary>
/// Specifies options that is related to the <see cref="HttpCacheableFilter" />.
/// </summary>
public class HttpCacheableOptions : IValidatableParameterObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HttpCacheableOptions"/> class.
    /// </summary>
    /// <remarks>
    /// The following table shows the initial property values for an instance of <see cref="HttpCacheableOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Initial Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="Filters"/></term>
    ///         <description><c>new List{ICacheableAsyncResultFilter}()</c></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="CacheControl"/></term>
    ///         <description><c>new CacheControlHeaderValue() { MaxAge = TimeSpan.FromMinutes(5), MustRevalidate = true, Private = true };</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public HttpCacheableOptions()
    {
        Filters = new List<ICacheableAsyncResultFilter>();
        CacheControl = new CacheControlHeaderValue() { MaxAge = TimeSpan.FromMinutes(5), MustRevalidate = true, Private = true };
    }

    /// <summary>
    /// Gets the filters that will be invoked one by one in <see cref="HttpCacheableFilter.OnResultExecutionAsync"/>.
    /// </summary>
    /// <value>The filters that will be invoked by <see cref="HttpCacheableFilter"/>.</value>
    public IList<ICacheableAsyncResultFilter> Filters { get; set; }

    /// <summary>
    /// Gets or sets the Cache-Control header that is applied to objects implementing the <see cref="ICacheableObjectResult"/> interface.
    /// </summary>
    /// <value>The Cache-Control header that is applied to objects implementing the <see cref="ICacheableObjectResult"/> interface.</value>
    public CacheControlHeaderValue CacheControl { get; set; }

    /// <summary>
    /// Gets a value indicating whether this instance has an <see cref="CacheControl"/>.
    /// </summary>
    /// <value><c>true</c> if this instance has an <see cref="CacheControl"/>; otherwise, <c>false</c>.</value>
    public bool UseCacheControl => CacheControl != null;

    /// <summary>
    /// Determines whether the public read-write properties of this instance are in a valid state.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// <see cref="Filters"/> cannot be null.
    /// </exception>
    /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
    public void ValidateOptions()
    {
        Validator.ThrowIfInvalidState(Filters == null);
    }
}
