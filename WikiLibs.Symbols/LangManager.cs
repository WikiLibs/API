using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.Data;
using WikiLibs.Data.Models.Symbols;
using WikiLibs.Shared;
using WikiLibs.Shared.Helpers;
using WikiLibs.Shared.Modules;
using WikiLibs.Shared.Modules.Symbols;

namespace WikiLibs.Symbols
{
    public class LangManager : BaseCRUDOperations<Context, Lang>, ILangManager
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

        public PageResult<LangListItem> GetFirstLangs(PageOptions options)
        {
            return (base.ToPageResult<LangListItem>(options, Set.OrderBy(e => e.Name)));
        }

        public PageResult<LibListItem> GetFirstLibs(long lang, PageOptions options)
        {
            var lng = Context.SymbolLangs.Find(new object[] { lang });
            var lngName = lng.Name + "/";
            return (base.ToPageResult<LibListItem, Lib>(options,
                Context.SymbolLibs.Where(e => e.Name.StartsWith(lngName))
                                  .OrderBy(e => e.Name)));
        }
    }
}
