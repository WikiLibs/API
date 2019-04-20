using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Shared.Modules
{
    public interface IUserManager : IModule, ICRUDOperations<Data.Models.User, string>
    {
        Data.Models.User Get(string email, string pass);
    }
}
