using System;
using Microsoft.Extensions.Options;

namespace Cuemon.Extensions.DependencyInjection.Assets;
public class FakeServiceTransient : FakeService
{
    public FakeServiceTransient(IOptions<FakeServiceTransientOptions> setup) : base(setup)
    {
    }

    public override string Lifetime => "Transient";
}
