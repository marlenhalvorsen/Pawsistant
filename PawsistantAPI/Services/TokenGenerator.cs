using PawsistantAPI.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PawsistantAPI.Services.Interfaces;

namespace PawsistantAPI.Services
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly IConfiguration _config;

        public TokenGenerator(IConfiguration config)
        {
            _config = config;
            
        }
        public string GenerateToken(string email, IList<string> roles)
        {
            var jwtSecretKey = _config["Jwt:SecretKey"];
            var jwtIssuer = _config["Jwt:Issuer"];
            var jwtAudience = _config["Jwt:Audience"];

            var claims = new List<Claim> { 
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, email)
                };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );
            Console.WriteLine($"Generated token: {token}");

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
