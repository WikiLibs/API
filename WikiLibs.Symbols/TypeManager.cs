using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.Data;
using WikiLibs.Shared;

namespace WikiLibs.Symbols
{
    public class TypeManager : BaseCRUDOperations<Context, Data.Models.Symbols.Type>
    {
        public TypeManager(Context ctx) : base(ctx)
        {
        }

        public override async Task<Data.Models.Symbols.Type> PatchAsync(long key, Data.Models.Symbols.Type mdl)
        {
            var m = await GetAsync(key);

            m.Name = mdl.Name;
            await SaveChanges();
            return (m);
        }
    }
}
