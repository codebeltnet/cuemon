using Cuemon.Configuration;

namespace Cuemon.Extensions.AspNetCore.Mvc.Assets;
public class FakeCacheableOptions : IParameterObject
{
    public FakeCacheableOptions()
    {
        Greeting = "Hello!";
    }

    public string Greeting { get; set; }
}
