using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureApiDemo.Data;
using System.Linq;

namespace SecureApiDemo.Controllers;

[ApiController]
[Route("api/secure")]
public class SecureController : ControllerBase
{
    private readonly AppDbContext _db;

    public SecureController(AppDbContext db)
    {
        _db = db;
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
    [Authorize] // Login olan herkes
    public IActionResult GetAll()
    {
        return Ok("🔓 Login olan herkes bu veriyi görebilir.");
    }

    [HttpGet("user/{id}")]
    [Authorize]
    public IActionResult GetUserById(int id)
    {
        var user = _db.Users.FirstOrDefault(u => u.Id == id);
        if (user == null)
            return NotFound("Kullanıcı bulunamadı.");

        var currentUsername = User.Identity?.Name;

        if (user.Username != currentUsername && !User.IsInRole("Admin"))
            return Forbid("Bu kullanıcıya erişim yetkiniz yok.");

        return Ok(new
        {
            user.Id,
            user.Username,
            user.Role
        });
    }

    [HttpGet("all-users")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetAllUsers()
    {
        var users = _db.Users
            .Select(u => new
            {
                u.Id,
                u.Username,
                u.Email,
                u.Role
            }).ToList();

        return Ok(users);
    }
}
