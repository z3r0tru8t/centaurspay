using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SecureApiDemo.Models;
using SecureApiDemo.DTOs;
using SecureApiDemo.Data;

namespace SecureApiDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

[HttpPost("login")]
public IActionResult Login([FromBody] LoginRequest request)
{
    var user = _context.Users.FirstOrDefault(u => u.Username == request.Username);
    if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        return Unauthorized(new { message = "Geçersiz kullanıcı adı veya şifre." });

    var token = GenerateJwtToken(user);

    // ✅ JWT’yi çerez olarak gönder
    Response.Cookies.Append("jwt", token, new CookieOptions
    {
        HttpOnly = true,
        SameSite = SameSiteMode.Strict,
        Secure = true // Not: Sadece HTTPS altında çalışır
    });

    // Token'ı JSON olarak da dönmek istersen:
    return Ok(new { message = "Giriş başarılı", token });
}





[HttpPost("register")]
public IActionResult Register([FromBody] RegisterRequest request)
{
    var exists = _context.Users.Any(u => u.Username == request.Username);
    if (exists)
        return BadRequest(new { message = "Bu kullanıcı adı zaten alınmış." });

    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

    var user = new User
    {
        Username = request.Username,
        Password = hashedPassword,
        Role = request.Role
    };

    _context.Users.Add(user);
    _context.SaveChanges();

    return Ok(new { message = "Kayıt başarılı." });
}




        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _config.GetSection("Jwt");

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiresInMinutes"]!)),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
