using System;
using Cuemon.AspNetCore.Configuration;

namespace Cuemon.AspNetCore.Razor.TagHelpers.Assets;
public class FakeCacheBusting : ICacheBusting
{
    public FakeCacheBusting()
    {
        Version = Guid.Empty.ToString("N");
    }

    public string Version { get; }
}
