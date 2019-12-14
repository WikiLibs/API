using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Models
{
    public abstract class PutModel<Model, DataModel, KeyType>
        where Model : PutModel<Model, DataModel, KeyType>, new()
        where DataModel : new()
    {
        public abstract DataModel CreateModel(KeyType id);
        public abstract DataModel CreatePatch(in DataModel current);
    }
}
