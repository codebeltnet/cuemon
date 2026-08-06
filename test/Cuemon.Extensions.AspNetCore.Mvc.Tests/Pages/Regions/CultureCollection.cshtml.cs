using System.Globalization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Cuemon.Extensions.AspNetCore.Mvc.Pages.Regions;
public class CultureCollectionModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public CultureCollectionModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet(string regionName, string regionDisplayName)
    {
        Region = new RegionInfo(regionName);
    }

    public RegionInfo Region { get; set; }
}
