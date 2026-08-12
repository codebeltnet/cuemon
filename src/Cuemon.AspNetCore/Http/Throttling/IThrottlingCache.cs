using System.Collections.Generic;

namespace Cuemon.AspNetCore.Http.Throttling;
/// <summary>
/// Specifies the contract for the storage of a throttling cache.
/// </summary>
/// <seealso cref="ThrottleRequest" />
public interface IThrottlingCache : IDictionary<string, ThrottleRequest>
{
}
