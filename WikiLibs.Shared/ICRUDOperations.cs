using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WikiLibs.Shared
{
    public interface ICRUDOperations<DataModel, KeyType>
        where DataModel : Data.Model<KeyType>
    {
        Task<DataModel> PostAsync(DataModel mdl);
        Task<DataModel> PatchAsync(KeyType key, DataModel mdl);
        Task<DataModel> DeleteAsync(DataModel mdl);
        Task<DataModel> DeleteAsync(KeyType key);
        Task<DataModel> GetAsync(KeyType key);
        Task<int> SaveChanges();
    }

    public interface ICRUDOperations<DataModel> : ICRUDOperations<DataModel, long>
        where DataModel : Data.Model<long>
    {
    }
}
