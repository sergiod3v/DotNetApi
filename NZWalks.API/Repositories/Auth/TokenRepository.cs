using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace NZWalks.API.Repositories {
    public class TokenRepository : ITokenRepository {
        private readonly IConfiguration configuration;

        public TokenRepository(IConfiguration configuration) {
            this.configuration = configuration;
        }

        public string CreateToken(IdentityUser user, List<string> roles) {
            List<Claim> claims = [];

            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            foreach (string role in roles) {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            byte[]? jwtKey = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);

            SymmetricSecurityKey? key = new(jwtKey);

            SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}