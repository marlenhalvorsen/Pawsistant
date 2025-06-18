using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Library.Shared.Auth;
using PawsistantAPI.Repository.config;
using Microsoft.AspNetCore.Identity;
using Library.Shared.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;



[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context; 
    private readonly IPasswordHasher<ApplicationUser> _hasher;
    private readonly UserManager<ApplicationUser> _userManager;
    //private readonly ILogger _logger;

    public AuthController(AppDbContext context, IPasswordHasher<ApplicationUser> hasher, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _hasher = hasher;
        _userManager = userManager;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDTO model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // Returns validation errors
        }

        // Check if user credentials are valid (you would check against a DB in a real scenario)
        if (model.Email == "testuser@example.com" && model.Password == "password")
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, model.Email),
                // You can add more claims, e.g., roles, if needed
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecretKeyHere"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "YourIssuer",
                audience: "YourAudience",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        return Unauthorized();
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
    {
        try { 
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser == null)
                return BadRequest(ModelState);

        
        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FirstName = dto.FirstName, 
            LastName = dto.LastName
        };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, "User");

            return Ok("User registered successfully.");
        }
        catch (Exception ex)
        {
            // Midlertidig fejl-log
            return StatusCode(500, $"Server error: {ex.Message}");
        }

    }

}
