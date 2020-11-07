using System.Linq;
using WikiLibs.Data.Models.Symbols;
using WikiLibs.Shared;
using System.Threading.Tasks;
using WikiLibs.Shared.Helpers;
using WikiLibs.Shared.Attributes;
using Microsoft.EntityFrameworkCore;
using WikiLibs.Shared.Exceptions;
using WikiLibs.Shared.Modules.Symbols;

namespace WikiLibs.Symbols
{
    [Module(Interface = typeof(ISymbolManager))]
    public class SymbolManager : BaseCRUDOperations<Data.Context, Symbol>, ISymbolManager
    {
        public ILangManager LangManager { get; }

        public ILibManager LibManager { get;  }

        public ICRUDOperations<Data.Models.Symbols.Type> TypeManager { get; }

        public SymbolManager(Data.Context db, Config cfg) : base(db)
        {
            MaxResults = cfg.MaxSymsPerPage;
            LangManager = new LangManager(db);
            TypeManager = new TypeManager(db);
            LibManager = new LibManager(db);
        }

        private void GetLibLangFromPath(Symbol sym, out string lib, out string lang)
        {
            string[] objs = sym.Path.Split('/');

            lib = objs[1];
            lang = objs[0];
        }

        public override async Task<Symbol> DeleteAsync(Symbol sym)
        {
            await base.DeleteAsync(sym);
            string lib;
            string lang;
            GetLibLangFromPath(sym, out lib, out lang);
            string libl = lang + "/" + lib;

            if (!Set.Any(sy => sy.Path.StartsWith(libl)))
                Context.SymbolLibs.RemoveRange(Context.SymbolLibs.Where(e => e.Name == libl));
            if (sym.Import != null && !Set.Any(sy => sy.Import != null && sy.Import.Name == sym.Import.Name))
                Context.SymbolImports.RemoveRange(Context.SymbolImports.Where(e => e.Name == sym.Import.Name));
            await SaveChanges();
            return (sym);
        }

        public async Task<Symbol> GetAsync(string path)
        {
            var sym = Set.Where(o => o.Path == path);

            if (sym == null || sym.Count() <= 0)
                throw new Shared.Exceptions.ResourceNotFound()
                {
                    ResourceType = typeof(Symbol),
                    ResourceName = path
                };
            var s = await sym.FirstAsync();
            ++s.Views;
            await SaveChanges();
            return (s);
        }

        public override async Task<Symbol> GetAsync(long key)
        {
            var res = await base.GetAsync(key);

            ++res.Views;
            await SaveChanges();
            return (res);
        }

        private bool CheckSymPath(Symbol sym)
        {
            string[] objs = sym.Path.Split('/');

            return (objs.Length > 2);
        }

        public override async Task<Symbol> PostAsync(Symbol sym)
        {
            if (!CheckSymPath(sym))
                throw new Shared.Exceptions.InvalidResource()
                {
                    PropertyName = "Path",
                    ResourceType = typeof(Symbol),
                    ResourceName = sym.Path
                };
            if (sym.User == null)
                throw new Shared.Exceptions.InvalidResource()
                {
                    PropertyName = "User",
                    ResourceType = typeof(Symbol),
                    ResourceName = sym.Path
                };
            if (Set.Any(o => o.Path == sym.Path))
                throw new Shared.Exceptions.ResourceAlreadyExists()
                {
                    ResourceId = Set.Where(o => o.Path == sym.Path).First().Id.ToString(),
                    ResourceType = typeof(Symbol),
                    ResourceName = sym.Path
                };

            string lib;
            string lang;
            GetLibLangFromPath(sym, out lib, out lang);
            string libl = lang + "/" + lib;
            if (!Context.SymbolLangs.Any(e => e.Name == lang))
                throw new Shared.Exceptions.InvalidResource()
                {
                    PropertyName = "Lang",
                    ResourceType = typeof(Symbol),
                    ResourceName = sym.Path
                };
            sym.Lang = Context.SymbolLangs.Where(e => e.Name == lang).FirstOrDefault();
            string type = sym.Type.Name;
            sym.Type = null;
            if (!Context.SymbolTypes.Any(e => e.Name == type))
                throw new Shared.Exceptions.InvalidResource()
                {
                    PropertyName = "Type",
                    ResourceType = typeof(Symbol),
                    ResourceName = sym.Path
                };
            sym.Type = Context.SymbolTypes.Where(e => e.Name == type).FirstOrDefault();
            Lib l = await LibManager.Get(e => e.Name == libl).FirstOrDefaultAsync();
            if (l == null)
                throw new ResourceNotFound
                {
                    ResourceId = "0",
                    ResourceName = libl,
                    ResourceType = typeof(Lib)
                };
            if (sym.Import != null)
            {
                Import import = null;
                if (!Context.SymbolImports.Any(e => e.Name == sym.Import.Name))
                    import = new Import() { Name = sym.Import.Name };
                else
                    import = Context.SymbolImports.Where(e => e.Name == sym.Import.Name).FirstOrDefault();
                sym.Import = import;
            }
            sym.Lib = l;
            return (await base.PostAsync(sym));
        }

        public override async Task<Symbol> PatchAsync(long key, Symbol sym)
        {
            var s = await GetAsync(key);

            s.LastModificationDate = sym.LastModificationDate;
            if (sym.Type != null)
            {
                if (!Context.SymbolTypes.Any(e => e.Name == sym.Type.Name))
                    throw new Shared.Exceptions.InvalidResource()
                    {
                        PropertyName = "Type",
                        ResourceType = typeof(Symbol),
                        ResourceName = sym.Path
                    };
                s.Type = Context.SymbolTypes.Where(e => e.Name == sym.Type.Name).FirstOrDefault();
            }
            if (sym.Import != null)
            {
                if (s.Import != null && Context.Symbols.Where(e => e.Import != null && e.Import.Name == s.Import.Name).Count() == 1) //This line fucks everything up
                    Context.RemoveRange(Context.SymbolImports.Where(e => e.Name == s.Import.Name));
                Import import = null;
                if (!Context.SymbolImports.Any(e => e.Name == sym.Import.Name))
                    import = new Import() { Name = sym.Import.Name };
                else
                    import = Context.SymbolImports.Where(e => e.Name == sym.Import.Name).FirstOrDefault();
                s.Import = import;
            }
            await SaveChanges();
            if (sym.Prototypes != null)
            {
                Context.RemoveRange(s.Prototypes);
                await SaveChanges();
                foreach (var proto in sym.Prototypes)
                {
                    foreach (var param in proto.Parameters)
                        param.Id = 0;
                    foreach (var ex in proto.Exceptions)
                        ex.Id = 0;
                    proto.Id = 0;
                    proto.Symbol = s;
                    s.Prototypes.Add(proto);
                }
                await SaveChanges();
            }
            if (sym.Symbols != null)
            {
                Context.RemoveRange(s.Symbols);
                await SaveChanges();
                foreach (var r in sym.Symbols)
                {
                    r.Id = 0;
                    s.Symbols.Add(r);
                }
                await SaveChanges();
            }
            return (s);
        }

        public PageResult<SymbolListItem> SearchSymbols(string path, PageOptions options)
        {
            return (base.ToPageResult<SymbolListItem>(options,
                Set.Where(sym => sym.Path.Contains(path))
                   .OrderByDescending(o => o.Views).ThenBy(o => o.Path)));
        }

        public PageResult<SymbolListItem> GetSymbolsForLib(long id, PageOptions options)
        {
            return (base.ToPageResult<SymbolListItem>(options,
                Set.Where(sym => sym.LibId == id)
                   .OrderBy(sym => sym.Path)));
        }

        public async Task OptimizeAsync()
        {
            //Pass 1 optimize symbol references
            //First clean duplications
            var dupes = Context.SymbolRefs.ToList().GroupBy(e => new { e.RefPath, e.SymbolId }).Where(q => q.Count() > 1).SelectMany(e => e.Skip(1));
            Context.RemoveRange(dupes);
            await SaveChanges();
            //Now identify references
            var srefs = Context.SymbolRefs.Where(e => e.RefId == null).ToList();
            foreach (var sref in srefs)
            {
                var symbol = Set.Where(o => o.Path == sref.RefPath).FirstOrDefault();
                if (symbol != null)
                    sref.RefId = symbol.Id;
                else
                {
                    string str = sref.Symbol.Lib.Name + "/" + sref.RefPath;
                    symbol = Set.Where(o => o.Path == str).FirstOrDefault();
                    if (symbol != null)
                        sref.RefId = symbol.Id;
                }
            }
            await SaveChanges();

            //Pass 2 optimize parameter to symbol references
            //First clean duplications
            var dupes1 = Context.PrototypeParamSymbolRefs.ToList().GroupBy(e => new { e.RefPath, e.PrototypeParamId }).Where(q => q.Count() > 1).SelectMany(e => e.Skip(1)); //EF Core is a peace of shit: unable to support GroupBy I mean M$ you have to go learn SQL you mother fucker
            Context.RemoveRange(dupes1);
            await SaveChanges();
            //Now identify references
            var sprefs = Context.PrototypeParamSymbolRefs.Where(e => e.RefId == null).ToList();
            foreach (var sref in sprefs)
            {
                var symbol = Set.Where(o => o.Path == sref.RefPath).FirstOrDefault();
                if (symbol != null)
                    sref.RefId = symbol.Id;
                else
                {
                    string str = sref.PrototypeParam.Prototype.Symbol.Lib.Name + "/" + sref.RefPath;
                    symbol = Set.Where(o => o.Path == str).FirstOrDefault();
                    if (symbol != null)
                        sref.RefId = symbol.Id;
                }
            }
            await SaveChanges();

            //Pass 3 optimize exceptions to symbol references
            //First clean duplications
            var dupes2 = Context.Exceptions.ToList().GroupBy(e => new { e.RefPath, e.PrototypeId }).Where(q => q.Count() > 1).SelectMany(e => e.Skip(1)); //EF Core is a peace of shit: unable to support GroupBy I mean M$ you have to go learn SQL you mother fucker
            Context.RemoveRange(dupes2);
            await SaveChanges();
            //Now identify references
            var serefs = Context.Exceptions.Where(e => e.RefId == null).ToList();
            foreach (var sref in serefs)
            {
                var symbol = Set.Where(o => o.Path == sref.RefPath).FirstOrDefault();
                if (symbol != null)
                    sref.RefId = symbol.Id;
                else
                {
                    string str = sref.Prototype.Symbol.Lib.Name + "/" + sref.RefPath;
                    symbol = Set.Where(o => o.Path == str).FirstOrDefault();
                    if (symbol != null)
                        sref.RefId = symbol.Id;
                }
            }
            await SaveChanges();
        }

        [ModuleInitializer(Debug = true, Release = true)]
        public static void InitSymbols(ISymbolManager mgr)
        {
            mgr.OptimizeAsync().Wait(); //EF Core cannot handle multiple connections at the same time from the same app => very nice for performance...
        }

        public PageResult<SymbolListItem> SearchSymbols(SearchQuery options)
        {
            var res = Set.AsQueryable();
            if (options.LangId != null)
                res = res.Where(sym => options.LangId.Value == sym.LangId);
            if (options.LibId != null)
                res = res.Where(sym => options.LibId.Value == sym.LibId);
            if (options.TypeId != null)
                res = res.Where(sym => options.TypeId == sym.TypeId);
            if (options.Path != null)
                res = res.Where(sym => sym.Path.Contains(options.Path));
            return (base.ToPageResult<SymbolListItem>(options.PageOptions,
                res.OrderByDescending(o => o.Views).ThenBy(o => o.Path)));
        }
    }
}
