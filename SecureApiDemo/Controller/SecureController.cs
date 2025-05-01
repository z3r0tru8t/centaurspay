using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SecureApiDemo.Controller;

[ApiController]
[Route("api/[controller]")]
public class SecureController : ControllerBase
{
    // Sadece token’ı olan herkes erişebilir
    [Authorize]
    [HttpGet("user")]
    public IActionResult GetForUser()
    {
        var username = User.Identity?.Name;
        return Ok($"✅ Hoş geldin, {username}. Bu endpoint JWT token'ı olan herkes için.");
    }

    // Sadece Admin rolündekiler erişebilir
    [Authorize(Roles = "Admin")]
    [HttpGet("admin")]
    public IActionResult GetForAdmin()
    {
        return Ok("🔐 Bu endpoint sadece 'Admin' rolüne sahip kullanıcılar içindir.");
    }

    // Herkes erişebilir (anonim)
    [AllowAnonymous]
    [HttpGet("public")]
    public IActionResult Public()
    {
        return Ok("🌐 Bu endpoint herkes tarafından erişilebilir.");
    }
}
