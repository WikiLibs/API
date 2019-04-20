using System;
using WikiLibs.Shared.Modules.Auth;

namespace WikiLibs.Auth
{
    public class AuthManager : IAuthManager
    {
        public IAuthProvider GetAuthenticator(string serviceName)
        {
            throw new NotImplementedException();
        }

        public string Refresh(string uid)
        {
            throw new NotImplementedException();
        }
    }
}
