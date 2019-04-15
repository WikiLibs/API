using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Models
{
    public abstract class PatchModel<Model, DataModel>
        where Model : PatchModel<Model, DataModel>, new()
        where DataModel : new()
    {
        public abstract DataModel CreatePatch(in DataModel current);

        public ICollection<DataModel> PatchCollection(in ICollection<DataModel> currents)
        {
            var mdls = new HashSet<DataModel>();

            foreach (var mdl in currents)
                mdls.Add(CreatePatch(mdl));
            return (mdls);
        }
    }
}
