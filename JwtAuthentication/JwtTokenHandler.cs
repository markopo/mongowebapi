using System;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace JwtAuthentication
{
    public class JwtTokenHandler : IJwtTokenHandler
    {
        private readonly DateTime _expires = DateTime.UtcNow.AddDays(2);
        private readonly string _jwtSecret;
        
        public JwtTokenHandler(string jwtSecret)
        {
            _jwtSecret = jwtSecret;
        }

        public async Task<string> GetToken(string userId)
        {
           var jwtToken = await Task.Run<string>(() =>
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSecret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new []
                    {
                        new Claim(ClaimTypes.Name, userId)
                    }),
                    Expires = _expires,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            });

           return jwtToken;
        }
        
    }
}