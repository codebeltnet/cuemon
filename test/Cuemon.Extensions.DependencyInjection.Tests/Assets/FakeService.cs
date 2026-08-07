using System;
using Microsoft.Extensions.Options;

namespace Cuemon.Extensions.DependencyInjection.Assets;
public abstract class FakeService
{
    protected FakeService(IOptions<FakeOptions> setup)
    {
        var options = setup.Value;
        Id = Guid.NewGuid();
        Greeting = options.Greeting;
    }

    public Guid Id { get; }

    public abstract string Lifetime { get; }

    public string Greeting { get; }

    public override string ToString()
    {
        return Greeting;
    }
}
