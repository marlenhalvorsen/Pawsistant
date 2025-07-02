using Microsoft.AspNetCore.Mvc;
using Library.Shared.Auth;
using PawsistantAPI.Repository.config;
using Microsoft.AspNetCore.Authorization;
using PawsistantAPI.Services.Interfaces;
using PawsistantAPI.Model;


[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IAuthService _authService;
    //private readonly ILogger _logger;

    public AuthController(AppDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                var token = await _authService.LoginAsync(loginDto);
            Console.WriteLine($"Generated token: {token ?? "<null>"}");

            // Put JWT as HTTP-only cookie
            Response.Cookies.Append("X-Access-Token", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(30)
                });

                return Ok(new { Message = "Login Successful.", Email = loginDto.Email });
            }
            catch (Exception ex)
            {
                return Unauthorized($"Login failed: {ex.Message}");
            }
        }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterUserAsync(dto);

            if (!result)
            {
                return BadRequest("Registration failed");
            }

            return Ok(result);
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Server error: {ex.Message}");
        }
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        return Ok(new { Email = User.Identity?.Name });
    }

    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("X-Access-Token");
        return Ok("Logged out successfully");
    }

}
