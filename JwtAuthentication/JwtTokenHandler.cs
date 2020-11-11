using System;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace JwtAuthentication
{
    public class JwtTokenHandler : IJwtTokenHandler
    {
        private readonly DateTime _expires = DateTime.UtcNow.AddDays(1);

        public async Task<string> GetToken(string userId)
        {
           var jwtToken = await Task.Run<string>(() =>
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = new RsaSecurityKey(RSA.Create(2048));
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, userId)
                    }),
                    Expires = _expires,
                    SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            });

           return jwtToken;
        }
        
    }
}