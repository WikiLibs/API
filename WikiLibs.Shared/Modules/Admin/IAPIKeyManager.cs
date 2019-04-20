using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiLibs.Shared.Modules.Admin
{
    public interface IAPIKeyManager : ICRUDOperations<Data.Models.APIKey, string>
    {
        IQueryable<Data.Models.APIKey> GetAll();
        bool Exists(string key);
        Task UseAPIKey(string key);
    }
}
