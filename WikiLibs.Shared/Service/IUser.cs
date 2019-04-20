using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikiLibs.Shared.Service
{
    public interface IUser
    {
        bool HasPermission(string name);

        bool IsExternal { get; }

        Data.Models.User User { get; }
        string UserId { get; }
    }
}
