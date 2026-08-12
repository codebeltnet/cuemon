using System;
using Microsoft.Extensions.Options;

namespace Cuemon.Extensions.DependencyInjection.Assets;
public class FakeServiceSingleton : FakeService
{
    public FakeServiceSingleton(IOptions<FakeServiceSingletonOptions> setup) : base(setup)
    {
    }

    public override string Lifetime => "Singleton";
}
