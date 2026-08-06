using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Mvc.Filters.Diagnostics;
using Cuemon.AspNetCore.Mvc.Filters.Headers;
using Cuemon.Extensions.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cuemon.AspNetCore.Mvc.Assets;
[ApiController]
[Route("[controller]")]
public class FakeController : ControllerBase
{
    private readonly IServerTiming _serverTiming;

    public FakeController(IServerTiming serverTiming = null)
    {
        _serverTiming = serverTiming;
    }

    [HttpGet]
    [BearerThrottlingSentinel(10, 5, TimeUnit.Seconds)]
    public IActionResult Get()
    {
        return Ok("Unit Test");
    }

    [HttpGet("it")]
    public IActionResult GetIt()
    {
        return Ok("Unit Test");
    }

    [ApiKeySentinel]
    [HttpGet("it-apikeysentinelattribute")]
    public IActionResult GetItApiKeySentinelAttribute()
    {
        return Ok("Unit Test");
    }

    [HttpGet("oneSecond")]
    public async Task<IActionResult> GetAfter1Second()
    {
        var delay = TimeSpan.FromSeconds(1);
        await Task.Delay(delay);
        _serverTiming?.AddServerTiming("sapIntegration", delay);
        return Ok("Unit Test");
    }

    [HttpGet("oneSecondNoServerTimingFilter")]
    public async Task<IActionResult> GetAfter1SecondNoServerTimingFilter()
    {
        var delay = TimeSpan.FromSeconds(1);
        await Task.Delay(delay);
        return Ok("Unit Test");
    }

    [ServerTiming(Name = "action-result", Description = "action-description", DesiredLogLevel = LogLevel.Information)]
    [HttpGet("oneSecondAttribute")]
    public async Task<IActionResult> GetAfter1SecondDecorated()
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
        return Ok("Unit Test");
    }

    [ServerTiming]
    [HttpGet("oneSecondAttributeWithDefaults")]
    public async Task<IActionResult> GetAfter1SecondDecoratedWithDefaults()
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
        return Ok("Unit Test");
    }

    [HttpGet("getResponse400")]
    public IActionResult GetBadRequest()
    {
        throw new ValidationException("Unit Test");
    }

    [HttpGet("getCacheByEtag")]
    public IActionResult GetEtag()
    {
        return Ok("Unit Test".WithEntityTagHeader(o => o.ChecksumProvider = s => Convertible.GetBytes(Generate.HashCode32(s))));
    }

    [HttpGet("getCacheByLastModified")]
    public IActionResult GetLastModified()
    {
        return Ok("Unit Test".WithLastModifiedHeader(o => o.TimestampProvider = s => DateTime.UnixEpoch));
    }
}
