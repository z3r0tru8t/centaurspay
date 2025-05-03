using Microsoft.AspNetCore.Mvc;
using SecureApiDemo.Data;

namespace SecureApiDemo.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TestController> _logger;

        public TestController(AppDbContext context, ILogger<TestController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("sql-injection")]
        public IActionResult SqlInjectionTest([FromQuery] string username)
        {
            _logger.LogInformation("🧪 Gelen input: {Input}", username);

            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user == null)
                return NotFound("Kullanıcı bulunamadı.");

            return Ok(new
            {
                user.Id,
                user.Username,
                user.Role
            });
        }
    }
}
