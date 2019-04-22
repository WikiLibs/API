using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.Data.Models;
using WikiLibs.Shared.Modules.Auth;
using WikiLibs.Shared.Service;

namespace WikiLibs.Auth
{
    public class AuthProviderInternal : IAuthProvider
    {
        public AuthProviderInternal(IModuleManager mgr)
        {

        }

        public string GetConnectionString()
        {
            return (null);
        }

        public Task<string> LegacyLogin(string email, string pass)
        {
            throw new NotImplementedException();
        }

        public Task LegacyRegister(User usr)
        {
            throw new NotImplementedException();
        }

        public Task LegacyReset(string email)
        {
            throw new NotImplementedException();
        }

        public Task LegacyVerifyEmail(string code)
        {
            throw new NotImplementedException();
        }

        public Task<string> Login(string code)
        {
            return (Task.FromResult<string>(null));
        }
    }
}
