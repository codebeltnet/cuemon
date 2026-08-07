using Microsoft.AspNetCore.Mvc;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml.Assets;
[ApiController]
[Route("[controller]")]
public class FakeController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new WeatherForecast());
    }

    [HttpPost]
    public IActionResult Post(WeatherForecast model)
    {
        return CreatedAtAction(nameof(Post), model);
    }
}
