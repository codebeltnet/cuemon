namespace Cuemon.AspNetCore.Mvc;
/// <summary>
/// Provides a base class for <see cref="ICacheableObjectResult"/> related operations.
/// </summary>
/// <seealso cref="ICacheableObjectResult" />
internal abstract class CacheableObjectResult : ICacheableObjectResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CacheableObjectResult"/> class.
    /// </summary>
    /// <param name="instance">The object to make cacheable.</param>
    protected CacheableObjectResult(object instance)
    {
        Value = instance;
    }

    /// <summary>
    /// Gets or sets the value of the cacheable object.
    /// </summary>
    /// <value>The value of the cacheable object.</value>
    public object Value { get; set; }
}

/// <summary>
/// Provides a base class for <see cref="ICacheableObjectResult"/> related operations.
/// </summary>
/// <typeparam name="T">The type of the object to make cacheable.</typeparam>
/// <seealso cref="CacheableObjectResult" />
internal abstract class CacheableObjectResult<T> : CacheableObjectResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CacheableObjectResult{T}"/> class.
    /// </summary>
    /// <param name="instance">The object to make cacheable.</param>
    protected CacheableObjectResult(T instance) : base(instance)
    {
        Value = instance;
    }
}
