using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.Data;
using WikiLibs.Data.Models.Symbols;
using WikiLibs.Shared;

namespace WikiLibs.Symbols
{
    public class TypeManager : BaseCRUDOperations<Context, Data.Models.Symbols.Type>, ICRUDOperations<Data.Models.Symbols.Type>
    {
        public TypeManager(Context ctx) : base(ctx)
        {
        }

        public override async Task<Data.Models.Symbols.Type> PatchAsync(long key, Data.Models.Symbols.Type mdl)
        {
            var m = await GetAsync(key);

            if (Set.Any(e => e.Id != key && e.Name == mdl.Name))
                throw new Shared.Exceptions.ResourceAlreadyExists()
                {
                    ResourceId = key.ToString(),
                    ResourceName = m.Name,
                    ResourceType = typeof(Data.Models.Symbols.Type)
                };
            m.Name = mdl.Name;
            m.DisplayName = mdl.DisplayName;
            await SaveChanges();
            return (m);
        }

        public override Task<Data.Models.Symbols.Type> PostAsync(Data.Models.Symbols.Type mdl)
        {
            if (Set.Any(e => e.Name == mdl.Name))
                throw new Shared.Exceptions.ResourceAlreadyExists()
                {
                    ResourceId = mdl.Name,
                    ResourceName = mdl.Name,
                    ResourceType = typeof(Lang)
                };
            return (base.PostAsync(mdl));
        }
    }
}
