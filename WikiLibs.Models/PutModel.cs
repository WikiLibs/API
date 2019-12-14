using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Models
{
    public abstract class PutModel<Model, DataModel>
        where Model : PutModel<Model, DataModel>, new()
        where DataModel : new()
    {
        public abstract DataModel CreateModel();
        public abstract DataModel CreatePatch(in DataModel current);
    }
}
