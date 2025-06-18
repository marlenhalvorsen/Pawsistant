using Library.Shared.Auth;
using PawsistantAPI.Model;
using Microsoft.AspNetCore.Identity;
using PawsistantAPI.Services.Interfaces;

namespace PawsistantAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenGenerator _tokenGenerator;
        
        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ITokenGenerator tokenGenerator) 
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenGenerator = tokenGenerator; 

        }
        public async Task<string> LoginAsync(LoginDTO userDTO)
        {
            var user = await _userManager.FindByEmailAsync(userDTO.Email);
            if (user == null)
                throw new Exception("Invalid password or email");

            var isValidPasword = await _userManager.CheckPasswordAsync(user, userDTO.Password);
            if (!isValidPasword)
                throw new Exception("Invalid password");

            var roles = await _userManager.GetRolesAsync(user);

            var token = _tokenGenerator.GenerateToken(user.Email, roles);

            return token;
            
        }

        public async Task<bool> RegisterUserAsync(RegisterDTO userDTO)
        {
            var existingUser = await _userManager.FindByEmailAsync(userDTO.Email);
            if (existingUser != null)
                throw new Exception("Email already in use");

            var user = new ApplicationUser
            { 
                UserName = userDTO.Email,
                Email = userDTO.Email,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName
            };

            var result = await _userManager.CreateAsync(user, userDTO.Password);
            if(!result.Succeeded)
                throw new Exception("Failed to register user: " + string.Join(", ", result.Errors));

            if (!await _roleManager.RoleExistsAsync("User"))
            {
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }

            await _userManager.AddToRoleAsync(user, "User");

            return true;

        }
    }
}
