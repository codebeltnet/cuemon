using System;
using Microsoft.Extensions.Options;

namespace Cuemon.Extensions.DependencyInjection.Assets;
public class FakeServiceScoped : FakeService
{
    public FakeServiceScoped(IOptions<FakeServiceScopedOptions> setup) : base(setup)
    {
    }

    public override string Lifetime => "Scoped";
}
