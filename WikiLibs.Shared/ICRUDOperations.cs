using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.Shared.Helpers;

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
        IQueryable<DataModel> Get(Expression<Func<DataModel, bool>> expression);
        PageResult<T> ToPageResult<T>(PageOptions options, IQueryable<DataModel> models)
            where T : IPageResultModel<T, DataModel>, new();
    }

    public interface ICRUDOperations<DataModel> : ICRUDOperations<DataModel, long>
        where DataModel : Data.Model<long>
    {
    }
}
