using Microsoft.IdentityModel.Tokens;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Services
{
    public class TokenHelper
    {

        public static string createRefreshToken(User user, string key, int duration = 7)
        {
            return createToken(user, key, duration, c => new ClaimsIdentity(new Claim[]
                   {
                    new Claim(ClaimTypes.Name, c.id.ToString()),
                   }));
        }

        public static string CreateAccessToken(User user, string key, int duration = 1)
        {
            return createToken(user, key, duration, c => new ClaimsIdentity(new Claim[]
                   {
                    new Claim(ClaimTypes.NameIdentifier, c.id.ToString()),
                    new Claim(ClaimTypes.Name, c.email),
                    new Claim(ClaimTypes.Role,c.roles.FirstOrDefault().slug)
                   }));
        }

        private static string createToken(User user, string key, int duration, Func<User, ClaimsIdentity> setSubject)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = setSubject.Invoke(user),
                Expires = DateTime.UtcNow.AddDays(duration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static JwtSecurityToken getInformationFromToken(string token, string key, TokenValidationParameters options)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = Encoding.ASCII.GetBytes(key);
            SecurityToken validatedToken;
            IPrincipal principal = tokenHandler.ValidateToken(token, options, out validatedToken);
            return tokenHandler.ReadJwtToken(token);
        }
    }
}
