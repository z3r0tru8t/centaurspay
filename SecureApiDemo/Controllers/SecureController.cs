using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureApiDemo.Data;

namespace SecureApiDemo.Controllers;

[ApiController]
[Route("api/secure")]
public class SecureController : ControllerBase
{
    private readonly AppDbContext _context;

    public SecureController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("admin-data")]
    [Authorize(Policy = "AdminOnly")]
    public IActionResult GetAdminData()
    {
        return Ok("📦 Sadece Admin'lere özel veriler buradadır.");
    }

    [HttpGet("user-data")]
    [Authorize(Policy = "UserOnly")]
    public IActionResult GetUserData()
    {
        return Ok("🧾 Sadece kullanıcıya özel bilgiler.");
    }

    [HttpGet("everyone")]
    [Authorize] // Herkes erişebilir, login yeterli
    public IActionResult GetAll()
    {
        return Ok("🔓 Login olan herkes bu veriyi görebilir.");
    }

   [HttpGet("user/{id}")]
[Authorize]
public IActionResult GetUserById(int id)
{
    var user = _context.Users.FirstOrDefault(u => u.Id == id);
    if (user == null)
        return NotFound("Kullanıcı bulunamadı.");

    var currentUsername = User.Identity?.Name;

    if (user.Username != currentUsername && !User.IsInRole("Admin"))
        return StatusCode(403, "Bu istifadeciye giris icazeniz yoxdur XXX."); // ✅ doğru kullanım

    return Ok(new
    {
        user.Id,
        user.Username,
        user.Role
    });
}
}
