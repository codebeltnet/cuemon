using System;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Mvc.Filters.Cacheable;
using Cuemon.Configuration;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cuemon.Extensions.AspNetCore.Mvc.Assets;
public class FakeCacheableFilter : ICacheableAsyncResultFilter
{
    public Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        throw new NotImplementedException();
    }
}

public class ConfigurableFakeCacheableFilter : FakeCacheableFilter, IConfigurable<FakeCacheableOptions>
{
    public ConfigurableFakeCacheableFilter(Action<FakeCacheableOptions> setup)
    {
        Options = Patterns.Configure(setup);
    }

    public FakeCacheableOptions Options { get; }
}
