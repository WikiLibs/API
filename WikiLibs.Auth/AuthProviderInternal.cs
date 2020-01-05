using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WikiLibs.Data.Models;
using WikiLibs.Shared.Modules;
using WikiLibs.Shared.Modules.Auth;
using WikiLibs.Shared.Modules.Smtp;

namespace WikiLibs.Auth
{
    public class AuthProviderInternal : IAuthProvider
    {
        private readonly IUserManager _userManager;
        private readonly ISmtpManager _smtpManager;
        private readonly AuthManager _manager;

        public AuthProviderInternal(IUserManager umgr, ISmtpManager smgr, AuthManager manager)
        {
            _userManager = umgr;
            _smtpManager = smgr;
            _manager = manager;
        }

        public string GetConnectionString()
        {
            return (null);
        }


        public async Task<string> LegacyLogin(string email, string pass)
        {
            var usr = await _userManager.GetAsync(email, pass);

            if (usr == null || usr.Confirmation != null || usr.IsBot)
                throw new InvalidCredentials();
            return (_manager.GenToken(usr.Id));
        }

        public async Task LegacyRegister(User usr)
        {
            usr.Group = _manager.DefaultGroup;
            usr.Id = Guid.NewGuid().ToString();
            usr.Confirmation = Guid.NewGuid().ToString().Replace("{", "").Replace("-", "")
                + "."
                + usr.Id.Replace("{", "")
                + "."
                + DateTime.UtcNow.Millisecond.ToString();
            usr.RegistrationDate = DateTime.UtcNow;
            await _userManager.PostAsync(usr);
            await _smtpManager.SendAsync(new Mail()
            {
                Subject = "WikiLibs API Server",
                Template = Shared.Modules.Smtp.Models.UserRegistration.Template,
                Model = new Shared.Modules.Smtp.Models.UserRegistration()
                {
                    ConfirmCode = usr.Confirmation,
                    UserName = usr.FirstName + " " + usr.LastName,
                    Link = _manager.Config.Internal.RegistrationUrlBase + "/auth/internal/confirm/" + usr.Confirmation
                },
                Recipients = new List<Recipient>()
                {
                    new Recipient()
                    {
                        Email = usr.Email,
                        Name = usr.FirstName + " " + usr.LastName
                    }
                }
            });
        }

        public async Task LegacyReset(string email)
        {
            var usr = await _userManager.GetByEmailAsync(email);

            if (usr == null || usr.IsBot)
                throw new Shared.Exceptions.ResourceNotFound()
                {
                    ResourceId = email,
                    ResourceName = "User",
                    ResourceType = typeof(User)
                };
            usr.Pass = Shared.Helpers.PasswordUtils.NewPassword(Shared.Helpers.PasswordOptions.Standard);
            await _userManager.SaveChanges();
            await _smtpManager.SendAsync(new Mail()
            {
                Subject = "WikiLibs API Server",
                Template = Shared.Modules.Smtp.Models.UserReset.Template,
                Model = new Shared.Modules.Smtp.Models.UserReset()
                {
                    NewPassword = usr.Pass
                },
                Recipients = new List<Recipient>()
                {
                    new Recipient()
                    {
                        Email = usr.Email,
                        Name = usr.FirstName + " " + usr.LastName
                    }
                }
            });
        }

        public async Task LegacyVerifyEmail(string code)
        {
            if (code == null)
                throw new InvalidCredentials();
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
