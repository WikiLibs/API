using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WikiLibs.Shared.Modules
{
    public interface IUserManager : IModule, ICRUDOperations<Data.Models.User, string>
    {
        Task<Data.Models.User> GetAsync(string email, string pass);
        Task<Data.Models.User> GetByEmailAsync(string email);
    }
}
