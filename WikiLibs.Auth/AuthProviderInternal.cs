using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.Data.Models;
using WikiLibs.Shared.Modules;
using WikiLibs.Shared.Modules.Auth;
using WikiLibs.Shared.Service;

namespace WikiLibs.Auth
{
    public class AuthProviderInternal : IAuthProvider
    {
        private readonly IUserManager _userManager;
        private readonly ISmtpManager _smtpManager;
        private readonly Config _config;

        public AuthProviderInternal(IModuleManager mgr, Config cfg)
        {
            _userManager = mgr.GetModule<IUserManager>();
            _smtpManager = mgr.GetModule<ISmtpManager>();
            _config = cfg;
        }

        public string GetConnectionString()
        {
            return (null);
        }

        private string GenToken(string uuid)
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

        public async Task<string> LegacyLogin(string email, string pass)
        {
            var usr = await _userManager.GetAsync(email, pass);

            if (usr == null || usr.Confirmation != null)
                throw new InvalidCredentials();
            return (GenToken(usr.Id));
        }

        public async Task LegacyRegister(User usr)
        {
            usr.Id = new Guid().ToString();
            usr.Confirmation = new Guid().ToString().Replace("{", "").Replace("-", "")
                + "."
                + usr.Id.Replace("{", "")
                + "."
                + DateTime.UtcNow.Millisecond.ToString();
            usr.RegistrationDate = DateTime.UtcNow;
            await _userManager.PostAsync(usr);
            _smtpManager.SendEmailMessage(new EmailMessage()
            {
                Body = "Please confirm your email address using this code: '" + usr.Confirmation + "'",
                Subject = "Email confirmation required",
                To = usr.EMail
            });
        }

        public Task LegacyReset(string email)
        {
            throw new NotImplementedException();
        }

        public async Task LegacyVerifyEmail(string code)
        {
            var arr = code.Split('.');

            if (arr.Length != 3)
                throw new InvalidCredentials();
            var uid = arr[1];
            var usr = await _userManager.GetAsync(uid);
            if (usr.Confirmation != code)
                throw new InvalidCredentials();
            usr.Confirmation = null;
            await _userManager.SaveChanges();
        }

        public Task<string> Login(string code)
        {
            return (Task.FromResult<string>(null));
        }
    }
}
