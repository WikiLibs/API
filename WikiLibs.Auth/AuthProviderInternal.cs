﻿using System;
using System.Threading.Tasks;
using WikiLibs.Data.Models;
using WikiLibs.Shared.Modules;
using WikiLibs.Shared.Modules.Auth;

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

            if (usr == null || usr.Confirmation != null)
                throw new InvalidCredentials();
            return (_manager.GenToken(usr.Id));
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
