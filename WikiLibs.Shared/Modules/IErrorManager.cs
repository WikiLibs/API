using System.Collections.Generic;
using System.Threading.Tasks;

namespace WikiLibs.Shared.Modules
{
    public interface IErrorManager : IModule
    {
        public Task PostAsync(Data.Models.Error error);
        public Task CleanupAsync();
        public Task DeleteAsync(long key);
        public IEnumerable<Data.Models.Error> GetLatestErrors();
    }
}
