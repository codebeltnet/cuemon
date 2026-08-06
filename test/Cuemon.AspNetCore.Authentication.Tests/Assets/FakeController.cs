using System.IO;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cuemon.AspNetCore.Authentication.Assets;
[Authorize]
[ApiController]
[Route("[controller]")]
public class FakeController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Unit Test");
    }

    [HttpPost]
    public IActionResult Post()
    {
        using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
        {
            var body = reader.ReadToEndAsync().GetAwaiter().GetResult();
        }
        return Ok("Unit Test");
    }

    [AllowAnonymous]
    [HttpGet("anonymous")]
    public IActionResult GetAnonymous()
    {
        return Ok("Unit Test");
    }
}
