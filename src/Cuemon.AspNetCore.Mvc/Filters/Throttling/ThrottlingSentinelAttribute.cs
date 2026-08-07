using System;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.AspNetCore.Http.Throttling;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Mvc.Filters.Throttling;
/// <summary>
/// Represents an attribute that is used to mark an action method to be protected by a throttling sentinel.
/// </summary>
/// <seealso cref="ActionFilterAttribute" />
public abstract class ThrottlingSentinelAttribute : ActionFilterAttribute, IFilterFactory
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ThrottlingSentinelAttribute"/> class.
    /// </summary>
    /// <param name="rateLimit">The allowed rate from within a given <paramref name="window"/>.</param>
    /// <param name="window">The duration of the window.</param>
    /// <param name="windowUnit">One of the enumeration values that specifies the time unit of <paramref name="window"/>.</param>
    protected ThrottlingSentinelAttribute(int rateLimit, double window, TimeUnit windowUnit)
    {
        var options = new ThrottlingSentinelOptions();
        RateLimit = rateLimit;
        Window = window;
        WindowUnit = windowUnit;
        UseRetryAfterHeader = options.UseRetryAfterHeader;
        RetryAfterScope = options.RetryAfterScope;
        TooManyRequestsMessage = options.TooManyRequestsMessage;
        RateLimitHeaderName = options.RateLimitHeaderName;
        RateLimitRemainingHeaderName = options.RateLimitRemainingHeaderName;
        RateLimitResetHeaderName = options.RateLimitResetHeaderName;
        RateLimitResetScope = options.RateLimitResetScope;
    }

    private int RateLimit { get; }

    private double Window { get; }

    private TimeUnit WindowUnit { get; }

    /// <summary>
    /// Gets or sets a value indicating whether to include a Retry-After HTTP header specifying how long to wait before making a new request.
    /// </summary>
    /// <value><c>true</c> to include a Retry-After HTTP header specifying how long to wait before making a new request; otherwise, <c>false</c>.</value>
    public bool UseRetryAfterHeader { get; set; }

    /// <summary>
    /// Gets or sets the message of a throttled request that has exceeded the rate limit.
    /// </summary>
    /// <value>The message of a throttled request that has exceeded the rate limit.</value>
    public string TooManyRequestsMessage { get; set; }

    /// <summary>
    /// Gets or sets the preferred Retry-After HTTP header value that conforms with RFC 2616.
    /// </summary>
    /// <value>The preferred Retry-After HTTP header value that conforms with RFC 2616.</value>
    public RetryConditionScope RetryAfterScope { get; set; }

    /// <summary>
    /// Gets or sets the name of the rate limit remaining HTTP header.
    /// </summary>
    /// <value>The name of the rate limit remaining HTTP header.</value>
    public string RateLimitRemainingHeaderName { get; set; }

    /// <summary>
    /// Gets or sets the name of the rate limit HTTP header.
    /// </summary>
    /// <value>The name of the rate limit HTTP header.</value>
    public string RateLimitHeaderName { get; set; }

    /// <summary>
    /// Gets or sets the name of the rate limit reset HTTP header.
    /// </summary>
    /// <value>The name of the rate limit reset HTTP header.</value>
    public string RateLimitResetHeaderName { get; set; }

    /// <summary>
    /// Gets or sets the preferred rate limit reset HTTP header value that conforms with RFC 7231.
    /// </summary>
    /// <value>The preferred rate limit reset HTTP header value that conforms with RFC 7231.</value>
    public RetryConditionScope RateLimitResetScope { get; set; }

    /// <summary>
    /// Creates an instance of the executable filter.
    /// </summary>
    /// <param name="serviceProvider">The request <see cref="IServiceProvider" />.</param>
    /// <returns>An instance of the executable filter.</returns>
    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    {
        var tc = serviceProvider.GetRequiredService<IThrottlingCache>();
        return new ThrottlingSentinelFilter(Options.Create(new ThrottlingSentinelOptions()
        {
            Quota = new ThrottleQuota(RateLimit, Window, WindowUnit),
            ContextResolver = UniqueContextResolver,
            UseRetryAfterHeader = UseRetryAfterHeader,
            RetryAfterScope = RetryAfterScope,
            TooManyRequestsMessage = TooManyRequestsMessage,
            RateLimitHeaderName = RateLimitHeaderName,
            RateLimitRemainingHeaderName = RateLimitRemainingHeaderName,
            RateLimitResetHeaderName = RateLimitResetHeaderName,
            RateLimitResetScope = RateLimitResetScope
        }), tc);
    }

    /// <summary>
    /// Resolves a unique context of the throttling middleware (eg. IP-address, Authorization header, etc.).
    /// </summary>
    /// <param name="context">The <see cref="HttpContext"/> to extract a unique context from.</param>
    /// <returns>A string that uniquely identifies the requester in need of throttling.</returns>
    public abstract string UniqueContextResolver(HttpContext context);

    /// <summary>
    /// Gets a value that indicates if the result of <see cref="IFilterFactory.CreateInstance(IServiceProvider)" /> can be reused across requests.
    /// </summary>
    /// <value><c>true</c> if this instance is reusable; otherwise, <c>false</c>.</value>
    public bool IsReusable => false;
}
