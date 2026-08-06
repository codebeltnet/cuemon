using Microsoft.AspNetCore.Mvc;

namespace Cuemon.Extensions.AspNetCore.Mvc.Controllers;
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
