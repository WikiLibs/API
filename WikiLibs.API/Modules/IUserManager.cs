using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.API.Modules
{
    public interface IUserManager : IModule, ICRUDOperations<Data.Models.User, string, string>
    {
        Data.Models.User Get(string email, string pass);
    }
}
