using System;
using Cuemon.AspNetCore.Mvc.Filters.Cacheable;
using Cuemon.Data.Integrity;

namespace Cuemon.AspNetCore.Mvc;
/// <summary>
/// Provides access to factory methods for creating and configuring objects implementing the <see cref="ICacheableObjectResult"/> interface.
/// </summary>
public static class CacheableFactory
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
    public static ICacheableObjectResult CreateHttpLastModified<T>(T instance, Action<TimeBasedObjectResultOptions<T>> setup)
    {
        Validator.ThrowIfInvalidConfigurator(setup, out var options);
        return new TimeBasedObjectResult<T>(instance, options.TimestampProvider.Invoke(instance), options.ChangedTimestampProvider?.Invoke(instance));
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
    public static ICacheableObjectResult CreateHttpEntityTag<T>(T instance, Action<ContentBasedObjectResultOptions<T>> setup)
    {
        Validator.ThrowIfInvalidConfigurator(setup, out var options);
        return new ContentBasedObjectResult<T>(instance, options.ChecksumProvider.Invoke(instance), options.WeakChecksumProvider?.Invoke(instance) ?? false);
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
    public static ICacheableObjectResult Create<T>(T instance, Action<CacheableObjectResultOptions<T>> setup)
    {
        return new ContentTimeBasedObjectResult<T>(instance,
            (IEntityDataTimestamp)CreateHttpLastModified(instance, Patterns.ConfigureExchange<CacheableObjectResultOptions<T>, TimeBasedObjectResultOptions<T>>(setup)),
            (IEntityDataIntegrity)CreateHttpEntityTag(instance, Patterns.ConfigureExchange<CacheableObjectResultOptions<T>, ContentBasedObjectResultOptions<T>>(setup)));
    }
}
