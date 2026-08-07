using System;
using System.Globalization;

namespace Cuemon.Extensions.AspNetCore.Mvc.Models;
public class RegionModel
{
    public RegionModel()
    {
    }

    public RegionModel(string region)
    {
        Region = new RegionInfo(region);
    }

    public RegionModel(string region, string culture)
    {
        Region = new RegionInfo(region);
        Culture = new CultureInfo(culture);
        Timestamp = DateTime.UtcNow;
        Number = 1.1M;
    }

    public decimal Number { get; }

    public DateTime Timestamp { get; }

    public RegionInfo Region { get; }

    public CultureInfo Culture { get; set; }
}
