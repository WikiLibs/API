using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.API
{
    public interface ICRUDOperations<DataModel, GetKey, DeleteKey>
    {
        void Post(DataModel mdl);
        void Patch(DataModel mdl);
        void Delete(DataModel mdl);
        void Delete(DeleteKey key);
        DataModel Get(GetKey key);
    }
}
