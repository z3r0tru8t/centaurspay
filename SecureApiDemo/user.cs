namespace Cybercentaurs.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; } // Prod için hash kullanılır!
    public string Role { get; set; } // "Admin", "User"
}
