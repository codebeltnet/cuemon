using Microsoft.AspNetCore.Mvc.Filters;

namespace Cuemon.AspNetCore.Mvc.Filters.Cacheable;
/// <summary>
/// A filter tailored to the cacheable flows, that asynchronously surrounds execution of action results successfully returned from an action.
/// </summary>
public interface ICacheableAsyncResultFilter : IAsyncResultFilter
{
}
