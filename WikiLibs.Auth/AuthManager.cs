using System;
using System.Collections.Generic;
using WikiLibs.Shared.Attributes;
using WikiLibs.Shared.Modules.Auth;
using WikiLibs.Shared.Service;

namespace WikiLibs.Auth
{
    [Module(typeof(IAuthManager))]
    public class AuthManager : IAuthManager
    {
        private Dictionary<string, IAuthProvider> _providers = new Dictionary<string, IAuthProvider>();
        private readonly IModuleManager _modules;

        public AuthManager(IModuleManager mgr, Config cfg)
        {
            _modules = mgr;
            _providers["internal"] = new AuthProviderInternal(_modules, cfg);
        }

        public IAuthProvider GetAuthenticator(string serviceName)
        {
            return (null);
        }

        public string Refresh(string uid)
        {
            throw new NotImplementedException();
        }
    }
}
