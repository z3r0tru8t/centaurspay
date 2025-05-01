namespace SecureApiDemo.DTOs;

public class RegisterRequest
{
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Role { get; set; } = "User"; // varsayılan rol
}
