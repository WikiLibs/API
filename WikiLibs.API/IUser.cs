using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikiLibs.API
{
    public interface IUser
    {
        bool HasPermission(string name);

        /// <summary>
        /// Generates a new token
        /// </summary>
        /// <param name="uuid">If null generate token for current connected user, otherwise specify the UUID in the token</param>
        /// <returns>The newly generated JWT token</returns>
        string GenToken(string uuid = null);

        Data.Models.User User { get; }
    }
}
