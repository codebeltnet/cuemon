using System;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore.Http;
/// <summary>
/// Extension methods for the <see cref="int"/> struct hidden behind the <see cref="IDecorator{T}"/> interface.
/// </summary>
/// <seealso cref="IDecorator{T}"/>
/// <seealso cref="Decorator{T}"/>
public static class Int32DecoratorExtensions
{
    /// <summary>
    /// Determines whether the enclosed <see cref="int"/> of the <paramref name="decorator"/> is within the informational range.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <returns><c>true</c> if the enclosed <see cref="int"/> of the <paramref name="decorator"/> was in the <b>Information</b> range (100-199); otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static bool IsInformationStatusCode(this IDecorator<int> decorator)
    {
        Validator.ThrowIfNull(decorator);
        return (decorator.Inner >= StatusCodes.Status100Continue && decorator.Inner <= 199);
    }

    /// <summary>
    /// Determines whether the enclosed <see cref="int"/> of the <paramref name="decorator"/> is within the successful range.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <returns><c>true</c> if the enclosed <see cref="int"/> of the <paramref name="decorator"/> was in the <b>Successful</b> range (200-299); otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static bool IsSuccessStatusCode(this IDecorator<int> decorator)
    {
        Validator.ThrowIfNull(decorator);
        return (decorator.Inner >= StatusCodes.Status200OK && decorator.Inner <= 299);
    }

    /// <summary>
    /// Determines whether the enclosed <see cref="int"/> of the <paramref name="decorator"/> is within the redirecting range.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <returns><c>true</c> if the enclosed <see cref="int"/> of the <paramref name="decorator"/> was in the <b>Redirection</b> range (300-399); otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static bool IsRedirectionStatusCode(this IDecorator<int> decorator)
    {
        Validator.ThrowIfNull(decorator);
        return (decorator.Inner >= StatusCodes.Status300MultipleChoices && decorator.Inner <= 399);
    }

    /// <summary>
    /// Determines whether the enclosed <see cref="int"/> of the <paramref name="decorator"/> equals a 304 Not Modified.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <returns><c>true</c> if the enclosed <see cref="int"/> of the <paramref name="decorator"/> is <b>NotModified</b> (304); otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static bool IsNotModifiedStatusCode(this IDecorator<int> decorator)
    {
        Validator.ThrowIfNull(decorator);
        return (decorator.Inner == StatusCodes.Status304NotModified);
    }

    /// <summary>
    /// Determines whether the enclosed <see cref="int"/> of the <paramref name="decorator"/> is within the client error related range.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <returns><c>true</c> if the enclosed <see cref="int"/> of the <paramref name="decorator"/> was in the <b>Client Error</b> range (400-499); otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static bool IsClientErrorStatusCode(this IDecorator<int> decorator)
    {
        Validator.ThrowIfNull(decorator);
        return (decorator.Inner >= StatusCodes.Status400BadRequest && decorator.Inner <= 499);
    }

    /// <summary>
    /// Determines whether the enclosed <see cref="int"/> of the <paramref name="decorator"/> is within the server error related range.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <returns><c>true</c> if the enclosed <see cref="int"/> of the <paramref name="decorator"/> was in the <b>Server Error</b> range (500-599); otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static bool IsServerErrorStatusCode(this IDecorator<int> decorator)
    {
        Validator.ThrowIfNull(decorator);
        return (decorator.Inner >= StatusCodes.Status500InternalServerError && decorator.Inner <= 599);
    }
}
