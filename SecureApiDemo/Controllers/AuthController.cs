using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SecureApiDemo.Models;
using SecureApiDemo.DTOs;
using SecureApiDemo.Data;
using System.Security.Cryptography;

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
        return Unauthorized("Geçersiz kullanıcı adı veya şifre.");

    // ✅ JWT oluştur
    var token = GenerateJwtToken(user);

    // ✅ Refresh Token üret & kaydet
    var refreshToken = GenerateRefreshToken();
    user.RefreshToken = refreshToken;
    user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
    _context.SaveChanges();

    return Ok(new { token, refreshToken });
}


[HttpPost("register")]
public IActionResult Register([FromBody] RegisterRequest request)
{
    try
    {
        // Kullanıcı adı mevcut mu kontrolü
        var exists = _context.Users.Any(u => u.Username == request.Username);
        if (exists)
            return BadRequest(new { message = "Bu kullanıcı adı zaten alınmış." });

        // Şifreyi güvenli biçimde hashle
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // Yeni kullanıcı nesnesi oluştur
        var user = new User
        {
            Username = request.Username,
            Password = hashedPassword,
            Role = request.Role,
            RefreshToken = "", // RefreshToken alanı boş başlatılıyor
            RefreshTokenExpiry = DateTime.UtcNow // Başlangıç değeri
        };

        // Kaydet
        _context.Users.Add(user);
        _context.SaveChanges();

        return Ok(new { message = "Kayıt başarılı." });
    }
    catch (Exception ex)
    {
        // Loglama yapılabilir: ex.Message
        return StatusCode(500, new { message = "Sunucu hatası: " + ex.Message });
    }
}



[HttpPost("refresh-token")]
public IActionResult RefreshToken([FromBody] RefreshTokenRequest model)
{
    var user = _context.Users.FirstOrDefault(u =>
        u.RefreshToken == model.RefreshToken &&
        u.RefreshTokenExpiry > DateTime.UtcNow);

    if (user == null)
        return Unauthorized(new { message = "Refresh token geçersiz veya süresi dolmuş." });

    var newAccessToken = GenerateJwtToken(user);
    var newRefreshToken = GenerateRefreshToken();

    user.RefreshToken = newRefreshToken;
    user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
    _context.SaveChanges();

    return Ok(new { token = newAccessToken, refreshToken = newRefreshToken });
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

        private string GenerateRefreshToken()
{
    var randomBytes = new byte[64];
    using var rng = RandomNumberGenerator.Create();
    rng.GetBytes(randomBytes);
    return Convert.ToBase64String(randomBytes);
}
    }
}
