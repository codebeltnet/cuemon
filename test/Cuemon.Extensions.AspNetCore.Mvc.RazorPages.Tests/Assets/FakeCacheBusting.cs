using System;
using Cuemon.AspNetCore.Configuration;

namespace Cuemon.Extensions.AspNetCore.Mvc.RazorPages.Assets;
public class FakeCacheBusting : ICacheBusting
{
    public FakeCacheBusting()
    {
        Version = Guid.Empty.ToString("N");
    }

    public string Version { get; }
}
