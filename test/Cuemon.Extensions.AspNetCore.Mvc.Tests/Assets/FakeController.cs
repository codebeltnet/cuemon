using System;
using Microsoft.AspNetCore.Mvc;

namespace Cuemon.Extensions.AspNetCore.Mvc.Assets;
[ApiController]
[Route("[controller]")]
public class FakeController : ControllerBase
{
    [HttpHead]
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(Guid.NewGuid().ToString("N").WithLastModifiedHeader(o => o.TimestampProvider = _ => DateTime.UtcNow));
    }
}
