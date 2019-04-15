using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Models
{
    public abstract class GetModel<Model, DataModel>
        where Model : GetModel<Model, DataModel>, new()
        where DataModel : new()
    {
        public abstract void Map(in DataModel model);

        public static Model CreateModel(in DataModel model)
        {
            var mdl = new Model();

            mdl.Map(model);
            return (mdl);
        }

        public static ICollection<Model> CreateModels(in ICollection<DataModel> models)
        {
            var lst = new HashSet<Model>();

            foreach (var mdl in models)
                lst.Add(CreateModel(mdl));
            return (lst);
        }
    }
}
