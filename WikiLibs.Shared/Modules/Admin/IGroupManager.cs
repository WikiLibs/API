using System;
using System.Linq;
using System.Text;

namespace WikiLibs.Shared.Modules.Admin
{
    public interface IGroupManager : ICRUDOperations<Data.Models.Group>
    {
        Data.Models.Group Get(string name);
        IQueryable<Data.Models.Group> GetAll();
    }
}
