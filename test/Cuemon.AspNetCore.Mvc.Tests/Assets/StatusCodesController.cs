using System;
using System.Reflection;
using Cuemon.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cuemon.AspNetCore.Mvc.Assets;
[ApiController]
[Route("[controller]")]
public class StatusCodesController : ControllerBase
{
    [HttpGet("400")]
    public IActionResult Get_400()
    {
        throw new BadRequestException(new ArgumentNullException());
    }

    [HttpGet("401")]
    public IActionResult Get_401()
    {
        throw new UnauthorizedException(new AccessViolationException());
    }

    [HttpGet("403")]
    public IActionResult Get_403()
    {
        throw new ForbiddenException(new UnauthorizedAccessException());
    }

    [HttpGet("404")]
    public IActionResult Get_404()
    {
        throw new NotFoundException(new NullReferenceException());
    }

    [HttpGet("405")]
    public IActionResult Get_405()
    {
        throw new MethodNotAllowedException(new ArgumentException());
    }

    [HttpGet("406")]
    public IActionResult Get_406()
    {
        throw new NotAcceptableException(new ArgumentException());
    }

    [HttpGet("409")]
    public IActionResult Get_409()
    {
        throw new ConflictException(new AmbiguousMatchException());
    }

    [HttpGet("410")]
    public IActionResult Get_410()
    {
        throw new GoneException(new NotImplementedException());
    }

    [HttpGet("412")]
    public IActionResult Get_412()
    {
        throw new PreconditionFailedException(new ArgumentOutOfRangeException());
    }

    [HttpGet("413")]
    public IActionResult Get_413()
    {
        throw new PayloadTooLargeException(new ArgumentOutOfRangeException());
    }

    [HttpGet("415")]
    public IActionResult Get_415()
    {
        throw new UnsupportedMediaTypeException(new ArgumentOutOfRangeException());
    }

    [HttpGet("428")]
    public IActionResult Get_428()
    {
        throw new PreconditionRequiredException(new ArgumentException());
    }

    [HttpGet("429")]
    public IActionResult Get_429()
    {
        throw new TooManyRequestsException(new OverflowException());
    }

    [HttpGet("XXX")]
    public IActionResult Get_XXX()
    {
        throw new NotSupportedException();
    }
}
