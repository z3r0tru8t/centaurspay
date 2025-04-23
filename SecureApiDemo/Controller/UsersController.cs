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
[HttpGet("get-user")]
public IActionResult GetUserByQuery([FromQuery] string id)
{
    // WARNING: This is intentionally vulnerable to SQL Injection for demo purposes
    string sqlQuery = $"SELECT * FROM Users WHERE Id = {id}"; // 👈 SQL Injection!

    // Bu satırda gerçek DB yok, simülasyon yapıyoruz
    var result = new
    {
        ExecutedQuery = sqlQuery,
        Message = "This is a simulation. In a real app, this would query the DB."
    };

    return Ok(result);
}
        public class LoginModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
