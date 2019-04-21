using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WikiLibs.Shared.Modules.Auth
{
    public interface IAuthProvider
    {
        /// <summary>
        /// Crafts a redirect URI if any for this provider (usually external services would require redirect to external page)
        /// </summary>
        /// <returns>the redirect URI to redirect the user to</returns>
        string GetConnectionString();

        /// <summary>
        /// Login with legacy user and password
        /// Throws InvalidCredentials in case of login failure
        /// </summary>
        /// <param name="email">user email</param>
        /// <param name="pass">user password</param>
        /// <returns>bearer access token for the API</returns>
        Task<string> LegacyLogin(string email, string pass);

        /// <summary>
        /// Register a user in legacy mode
        /// Email management is done by this function
        /// </summary>
        /// <param name="usr">The user to register</param>
        Task LegacyRegister(Data.Models.User usr);

        /// <summary>
        /// Resets a user password by its email (legacy services only)
        /// </summary>
        /// <param name="email">email of the user</param>
        Task LegacyReset(string email);

        /// <summary>
        /// Handles email verification
        /// Should save the user to database if successfull
        /// </summary>
        /// <param name="code">verification code</param>
        Task LegacyVerifyEmail(string code);

        /// <summary>
        /// Login with external service
        /// If the user is not already registered in the database perform registration
        /// Throws InvalidCredentials in case of login failure
        /// </summary>
        /// <param name="code">the value of the 'code' query parameter for the callback of an external service</param>
        /// <returns>bearer access token for the API</returns>
        Task<string> Login(string code);
    }
}
