using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.Data;
using WikiLibs.Data.Models.Symbols;
using WikiLibs.Shared;

namespace WikiLibs.Symbols
{
    public class LangManager : BaseCRUDOperations<Context, Lang>, ICRUDOperations<Lang>
    {
        public LangManager(Context ctx) : base(ctx)
        {
        }

        public override async Task<Lang> PatchAsync(long key, Lang mdl)
        {
            var m = await GetAsync(key);

            m.Name = mdl.Name;
            await SaveChanges();
            return (m);
        }
    }
}
