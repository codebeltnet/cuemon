using System;
using System.Net;
using Cuemon.AspNetCore.Mvc;
using Cuemon.AspNetCore.Mvc.Filters.Cacheable;
using Cuemon.AspNetCore.Mvc.Filters.Diagnostics;
using Cuemon.AspNetCore.Mvc.Filters.Headers;
using Cuemon.AspNetCore.Mvc.Filters.Throttling;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cuemon.Extensions.AspNetCore.Mvc.Filters;
/// <summary>
/// Extension methods for the <see cref="FilterCollection"/> class.
/// </summary>
public static class FilterCollectionExtensions
{
    /// <summary>
    /// Adds a <see cref="HttpCacheableFilter"/> to the <paramref name="filters"/> handled in the MVC request pipeline that will invoke filters implementing the <see cref="ICacheableObjectResult"/> interface
    /// </summary>
    /// <param name="filters">The <see cref="FilterCollection"/> to extend.</param>
    /// <returns>A <see cref="IFilterMetadata"/> representing the added type.</returns>
    public static IFilterMetadata AddHttpCacheable(this FilterCollection filters)
    {
        return filters.Add<HttpCacheableFilter>();
    }

    /// <summary>
    /// Adds a <see cref="FaultDescriptorFilter"/> to the <paramref name="filters"/> handled in the MVC request pipeline that, after an action has faulted, provides developer friendly information about an <see cref="Exception"/> along with a correct <see cref="HttpStatusCode"/>.
    /// </summary>
    /// <param name="filters">The <see cref="FilterCollection"/> to extend.</param>
    /// <returns>A <see cref="IFilterMetadata"/> representing the added type.</returns>
    public static IFilterMetadata AddFaultDescriptor(this FilterCollection filters)
    {
        return filters.Add<FaultDescriptorFilter>();
    }

    /// <summary>
    /// Adds a <see cref="ServerTimingFilter"/> to the <paramref name="filters"/> handled in the MVC request pipeline that performs time measure profiling of action methods.
    /// </summary>
    /// <param name="filters">The <see cref="FilterCollection"/> to extend.</param>
    /// <returns>A <see cref="IFilterMetadata"/> representing the added type.</returns>
    public static IFilterMetadata AddServerTiming(this FilterCollection filters)
    {
        return filters.Add<ServerTimingFilter>();
    }

    /// <summary>
    /// Adds an <see cref="UserAgentSentinelFilter"/> to the <paramref name="filters"/> handled in the MVC request pipeline that provides an User-Agent sentinel on action methods.
    /// </summary>
    /// <param name="filters">The <see cref="FilterCollection"/> to extend.</param>
    /// <returns>A <see cref="IFilterMetadata"/> representing the added type.</returns>
    public static IFilterMetadata AddUserAgentSentinel(this FilterCollection filters)
    {
        return filters.Add<UserAgentSentinelFilter>();
    }

    /// <summary>
    /// Adds a <see cref="ThrottlingSentinelFilter"/> to the <paramref name="filters"/> handled in the MVC request pipeline that provides an API throttling on action methods.
    /// </summary>
    /// <param name="filters">The <see cref="FilterCollection"/> to extend.</param>
    /// <returns>A <see cref="IFilterMetadata"/> representing the added type.</returns>
    public static IFilterMetadata AddThrottlingSentinel(this FilterCollection filters)
    {
        return filters.Add<ThrottlingSentinelFilter>();
    }

    /// <summary>
    /// Adds an <see cref="ApiKeySentinelFilter"/> to the <paramref name="filters"/> handled in the MVC request pipeline that provides an API key sentinel on action methods.
    /// </summary>
    /// <param name="filters">The <see cref="FilterCollection"/> to extend.</param>
    /// <returns>A <see cref="IFilterMetadata"/> representing the added type.</returns>
    public static IFilterMetadata AddApiKeySentinel(this FilterCollection filters)
    {
        return filters.Add<ApiKeySentinelFilter>();
    }
}
