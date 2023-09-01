using JwtStore.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtStore.Services
{
    public class JwtToken
    {
        public string Create(User user)
        {
            var handler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(Configuration.PrivateKey);

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key)
                , SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = GenerateClaims(user),
                SigningCredentials = credentials,
                Expires = DateTime.UtcNow.AddHours(2)
            };

            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }
        private static ClaimsIdentity GenerateClaims(User user)
        {
            var ci = new ClaimsIdentity();

            ci.AddClaim(new Claim("id",user.id.ToString()));
            ci.AddClaim(new Claim(ClaimTypes.GivenName,user.Name.ToString()));
            ci.AddClaim(new Claim(ClaimTypes.Name,user.Email.ToString())); // o ClaimTypes.Name é o identificador do ASP.NET, trabalha como se fosse um Id
            ci.AddClaim(new Claim(ClaimTypes.Email,user.Email.ToString()));
            ci.AddClaim(new Claim("image",user.Image.ToString()));

            foreach (var role in user.Roles)
            {
                ci.AddClaim(new Claim(ClaimTypes.Role, role));
            }


            return ci;
        }
    }
}
