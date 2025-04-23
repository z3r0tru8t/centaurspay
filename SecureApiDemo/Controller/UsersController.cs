using Microsoft.AspNetCore.Mvc;

namespace SecureApiDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = new { Id = id, Name = "User" + id, Email = $"user{id}@demo.com" };
            return Ok(user); // IDOR - authentication yok
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            if (model.Username == "admin")
                return Ok(new { token = "hardcoded-super-secret-token" }); // Hardcoded token

            return Unauthorized();
        }

        public class LoginModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
