using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiLibs.Shared.Modules.Admin
{
    public interface IAPIKeyManager : ICRUDOperations<Data.Models.ApiKey, string>
    {
        IQueryable<Data.Models.ApiKey> GetAll();
        bool Exists(string key);
        Task UseAPIKey(string key);
        IQueryable<Data.Models.ApiKey> GetAllOfDescription(string desc);
    }
}
