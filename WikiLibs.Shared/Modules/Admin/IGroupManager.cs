using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Shared.Modules.Admin
{
    public interface IGroupManager : ICRUDOperations<Data.Models.Group>
    {
        Data.Models.Group Get(string name);
        ICollection<Data.Models.Group> GetAll();
    }
}
