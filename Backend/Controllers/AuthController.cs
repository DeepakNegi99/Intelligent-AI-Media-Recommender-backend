using Backend.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    // Hardcoded credentials for now
    private const string AdminUsername = "admin";
    private const string AdminPassword = "pass123";

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (request.Username == AdminUsername && request.Password == AdminPassword)
        {
            var token = Guid.NewGuid().ToString(); // You can replace this with JWT later
            return Ok(new { token });
        }
        return Unauthorized("Invalid username or password");
    }
}
