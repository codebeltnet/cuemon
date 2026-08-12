using Cuemon.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;

namespace Cuemon.AspNetCore.Mvc.Filters.Headers;
/// <summary>
/// Provides a convenient way to protect your API with an <see cref="ApiKeySentinelFilter"/>.
/// </summary>
/// <seealso cref="ApiKeySentinelOptions"/>
/// <seealso cref="ServiceFilterAttribute" />
public class ApiKeySentinelAttribute : ServiceFilterAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiKeySentinelAttribute"/> class.
    /// </summary>
    public ApiKeySentinelAttribute() : base(typeof(ApiKeySentinelFilter))
    {
    }
}
