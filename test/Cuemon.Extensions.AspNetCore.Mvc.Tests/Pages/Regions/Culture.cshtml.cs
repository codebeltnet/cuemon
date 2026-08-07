using System;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Cuemon.Extensions.AspNetCore.Mvc.Pages.Regions;
public class CultureModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public CultureModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }

    public RegionInfo Region { get; }

    public CultureInfo Culture { get; set; }

    public DateTime Timestamp { get; }
}
