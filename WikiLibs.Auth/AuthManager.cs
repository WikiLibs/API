using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WikiLibs.Shared.Attributes;
using WikiLibs.Shared.Modules;
using WikiLibs.Shared.Modules.Admin;
using WikiLibs.Shared.Modules.Auth;
using WikiLibs.Shared.Modules.Smtp;

namespace WikiLibs.Auth
{
    [Module(Interface = typeof(IAuthManager))]
    public class AuthManager : IAuthManager
    {
        private Dictionary<string, IAuthProvider> _providers = new Dictionary<string, IAuthProvider>();
        private readonly Config _config;
        public Data.Models.Group DefaultGroup { get; }

        public AuthManager(IAdminManager admin, IUserManager umgr, ISmtpManager smgr, Config cfg)
        {
            _config = cfg;
            DefaultGroup = admin.GroupManager.Get(cfg.DefaultGroupName);
            _providers["internal"] = new AuthProviderInternal(umgr, smgr, this);
        }

        public IAuthProvider GetAuthenticator(string serviceName)
        {
            if (!_providers.ContainsKey(serviceName))
                return (null);
            return (_providers[serviceName]);
        }

        public string GenToken(string uuid)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.Internal.TokenSecret);
            var desc = new SecurityTokenDescriptor
            {
                Issuer = _config.Internal.TokenIssuer,
                Audience = _config.Internal.TokenAudiance,
                Expires = DateTime.UtcNow.AddMinutes(_config.Internal.TokenLifeMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature),
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, uuid)
                })
            };
            var token = handler.CreateToken(desc);
            return (handler.WriteToken(token));
        }

        public string Refresh(string uid)
        {
            return (GenToken(uid));
        }

        [ModuleConfigurator]
        public static void SetupJWTAuth(IServiceCollection services, Config jwtCfg)
        {
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtCfg.Internal.TokenSecret)),
                    ValidateIssuer = true,
                    ValidIssuer = jwtCfg.Internal.TokenIssuer,
                    ValidateLifetime = true,
                    ValidateAudience = true,
                    ValidAudience = jwtCfg.Internal.TokenAudiance
                };
            });
        }
    }
}
