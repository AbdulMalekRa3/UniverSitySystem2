using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using University.Core.DTOs;

namespace UniverSity.Api.Helpers
{
    public class JwtTokenHelper : IJwtTokenHelper
    {
        private readonly IConfiguration _configuration;

        public JwtTokenHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(UserDTO user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var jwtSettings = _configuration?.GetSection("JwtSettings");
            if (jwtSettings == null)
                throw new InvalidOperationException("JwtSettings section not found in configuration");

            var secretKeyValue = jwtSettings["SecretKey"];
            if (string.IsNullOrEmpty(secretKeyValue))
                throw new InvalidOperationException("JWT SecretKey is not configured");

            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKeyValue));
            var creds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512Signature);

            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email ?? ""),
        new Claim(ClaimTypes.Role, user.Role.ToString())
    };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = creds,
                Issuer = issuer,
                Audience = audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }

    public interface IJwtTokenHelper
    {
        string GenerateToken(UserDTO user);
    }
}
