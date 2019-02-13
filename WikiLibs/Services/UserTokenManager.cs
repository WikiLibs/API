using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WikiLibs.Services
{
    public interface ITokenManager
    {
        string GenToken(string uuid);
    }

    public class UserTokenManager : ITokenManager
    {
        //TODO : Token ban system

        private Helpers.JWT _jwtConfig;

        public UserTokenManager(Helpers.JWT jwt)
        {
            _jwtConfig = jwt;
        }

        public string GenToken(string uuid)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = System.Text.Encoding.ASCII.GetBytes(_jwtConfig.Secret);
            var desc = new SecurityTokenDescriptor
            {
                Issuer = _jwtConfig.Authority,
                Expires = DateTime.UtcNow.AddMinutes(_jwtConfig.Lifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature),
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, uuid)
                })
            };
            var token = handler.CreateToken(desc);
            return (handler.WriteToken(token));
        }
    }
}
