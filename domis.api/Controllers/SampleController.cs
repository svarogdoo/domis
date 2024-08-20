using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace domis.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SampleController : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("public-endpoint")]
    public IActionResult PublicEndpoint()
    {
        return Ok("This endpoint is accessible by everyone.");
    }

    [Authorize]
    [HttpGet("user-endpoint")]
    public IActionResult UserEndpoint()
    {
        return Ok("This endpoint is accessible by authenticated users.");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin-endpoint")]
    public IActionResult AdminEndpoint()
    {
        return Ok("This endpoint is accessible by users with the Administrator role.");
    }
}