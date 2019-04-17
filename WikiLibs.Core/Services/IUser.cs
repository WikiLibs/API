using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikiLibs.Core.Services
{
    public interface IUser
    {
        bool HasPermission(string name);

        /// <summary>
        /// Returns true if this user is logged in through external service, false otherwise
        /// </summary>
        bool IsExternal();

        Data.Models.User User { get; }

        string UserId { get; }
    }
}
