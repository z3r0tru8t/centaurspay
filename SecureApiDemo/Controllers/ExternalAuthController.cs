using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using SecureApiDemo.Data;
using SecureApiDemo.Models;
using SecureApiDemo.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace SecureApiDemo.Controllers
{
    [Route("external-login")]
    public class ExternalAuthController : Controller
    {
        private readonly AppDbContext _db;
        private readonly JwtTokenService _jwtService;

        public ExternalAuthController(AppDbContext db, JwtTokenService jwtService)
        {
            _db = db;
            _jwtService = jwtService;
        }

        [HttpGet("google")]
        public IActionResult GoogleLogin()
        {
            var props = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
            return Challenge(props, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = result.Principal?.Identities?.FirstOrDefault()?.Claims;

            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (email == null || name == null)
                return Unauthorized("Google login bilgileri eksik.");

            var userFromDb = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (userFromDb == null)
            {
                userFromDb = new User
                {
                    Email = email,
                    Username = name,
                    Role = "User"
                };

                _db.Users.Add(userFromDb);
                await _db.SaveChangesAsync();
            }

            var token = _jwtService.GenerateToken(userFromDb.Id, userFromDb.Email, userFromDb.Username, userFromDb.Role);

            return Ok(new
            {
                access_token = token,
                email = userFromDb.Email,
                name = userFromDb.Username
            });
        }
    }
}
