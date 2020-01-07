using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.Shared.Modules.File;

namespace WikiLibs.Shared.Modules
{
    public interface IUserManager : IModule, ICRUDOperations<Data.Models.User, string>, IFileManager<Data.Models.User, ImageFile>
    {
        Task<Data.Models.User> GetAsync(string email, string pass);
        Task<Data.Models.User> GetByEmailAsync(string email);
    }
}
