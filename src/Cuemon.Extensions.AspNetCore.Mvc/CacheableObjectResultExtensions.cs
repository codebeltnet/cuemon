using System;
using Cuemon.AspNetCore.Mvc;
using Cuemon.AspNetCore.Mvc.Filters.Cacheable;
using Cuemon.Data.Integrity;

namespace Cuemon.Extensions.AspNetCore.Mvc;
/// <summary>
/// Extension methods for the <see cref="ICacheableObjectResult"/> interface.
/// </summary>
/// <seealso cref="ICacheableObjectResult"/>
public static class CacheableObjectResultExtensions
{
    /// <summary>
    /// Encapsulates the specified <paramref name="instance" /> within a timestamp based object that is processed by a Last-Modified filter implementation.
    /// </summary>
    /// <typeparam name="T">The type of the object to make cacheable.</typeparam>
    /// <param name="instance">The instance to make cacheable.</param>
    /// <param name="setup">The <see cref="TimeBasedObjectResultOptions{T}" /> that needs to be configured.</param>
    /// <returns>An <see cref="ICacheableObjectResult" /> implementation.</returns>
    /// <seealso cref="IEntityDataTimestamp" />
    /// <seealso cref="CacheableObjectResult{T}" />
    /// <seealso cref="HttpLastModifiedHeaderFilter"/>
    /// <seealso cref="HttpCacheableFilter"/>
    public static ICacheableObjectResult WithLastModifiedHeader<T>(this T instance, Action<TimeBasedObjectResultOptions<T>> setup)
    {
        return CacheableFactory.CreateHttpLastModified(instance, setup);
    }

    /// <summary>
    /// Encapsulates the specified <paramref name="instance" /> within an integrity based object that is processed by an HTTP ETag filter implementation.
    /// </summary>
    /// <typeparam name="T">The type of the object to make cacheable.</typeparam>
    /// <param name="instance">The instance to make cacheable.</param>
    /// <param name="setup">The <see cref="ContentBasedObjectResultOptions{T}" /> that needs to be configured.</param>
    /// <returns>An <see cref="ICacheableObjectResult" /> implementation.</returns>
    /// <seealso cref="IEntityDataIntegrity" />
    /// <seealso cref="CacheableObjectResult{T}" />
    /// <seealso cref="HttpEntityTagHeaderFilter"/>
    /// <seealso cref="HttpCacheableFilter"/>
    public static ICacheableObjectResult WithEntityTagHeader<T>(this T instance, Action<ContentBasedObjectResultOptions<T>> setup)
    {
        return CacheableFactory.CreateHttpEntityTag(instance, setup);
    }

    /// <summary>
    /// Encapsulates the specified <paramref name="instance" /> within a timestamp and integrity based object that is processed by both HTTP Last-Modified and HTTP ETag filters implementation.
    /// </summary>
    /// <typeparam name="T">The type of the object to make cacheable.</typeparam>
    /// <param name="instance">The instance to make cacheable.</param>
    /// <param name="setup">The <see cref="CacheableObjectResultOptions{T}" /> that needs to be configured.</param>
    /// <returns>An <see cref="ICacheableObjectResult" /> implementation.</returns>
    /// <seealso cref="IEntityDataTimestamp" />
    /// <seealso cref="IEntityDataIntegrity" />
    /// <seealso cref="IEntityInfo" />
    /// <seealso cref="CacheableObjectResult{T}" />
    /// <seealso cref="HttpLastModifiedHeaderFilter"/>
    /// <seealso cref="HttpEntityTagHeaderFilter"/>
    /// <seealso cref="HttpCacheableFilter"/>
    public static ICacheableObjectResult WithCacheableHeaders<T>(this T instance, Action<CacheableObjectResultOptions<T>> setup)
    {
        return CacheableFactory.Create(instance, setup);
    }
}
