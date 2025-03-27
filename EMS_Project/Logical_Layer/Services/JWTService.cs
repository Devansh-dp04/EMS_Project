using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EMS_Project.Logical_Layer.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace EMS_Project.Logical_Layer.Services
{
    public class JWTService: IJWTService
    {
        private readonly IConfiguration _config;
        public JWTService(IConfiguration _configuration) {
            _config = _configuration;
        }       

        public string GenerateJWTToken(string email, string password, string role, int empid)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(ClaimTypes.Role, role),
                new Claim("EmployeeId", empid.ToString())


            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:ExpiryInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
