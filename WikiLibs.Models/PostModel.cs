using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Models
{
    public abstract class PostModel<Model, DataModel>
        where Model : PostModel<Model, DataModel>, new()
        where DataModel : new()
    {
        public abstract DataModel CreateModel();
    }
}
