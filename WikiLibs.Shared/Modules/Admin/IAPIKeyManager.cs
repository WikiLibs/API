using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Shared.Modules.Admin
{
    public interface IAPIKeyManager : ICRUDOperations<Data.Models.APIKey, string, string>
    {
        ICollection<Data.Models.APIKey> GetAll();
        bool Exists(string key);
    }
}
