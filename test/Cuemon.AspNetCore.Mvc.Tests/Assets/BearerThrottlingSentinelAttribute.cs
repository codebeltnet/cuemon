using System.Linq;
using Cuemon.AspNetCore.Mvc.Filters.Throttling;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Cuemon.AspNetCore.Mvc.Assets;
public class BearerThrottlingSentinelAttribute : ThrottlingSentinelAttribute
{
    public BearerThrottlingSentinelAttribute(int rateLimit, double window, TimeUnit windowUnit) : base(rateLimit, window, windowUnit)
    {
    }

    public override string UniqueContextResolver(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(HeaderNames.Authorization, out var authorization))
        {
            return authorization.ToString().Split(' ').Last();
        }
        return null;
    }
}
