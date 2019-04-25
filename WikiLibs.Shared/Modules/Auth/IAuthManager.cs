using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Shared.Modules.Auth
{
    public interface IAuthManager : IModule
    {
        IAuthProvider GetAuthenticator(string serviceName);

        /// <summary>
        /// Refresh a given user
        /// </summary>
        /// <param name="uid">The user ID to refresh the token</param>
        /// <returns>The new access token for the API</returns>
        string Refresh(string uid);
    }
}
