using System;
using System.Collections.Generic;
using System.Linq;
using WikiLibs.Shared.Modules;
using WikiLibs.Data.Models.Symbols;
using WikiLibs.Shared;
using System.Threading.Tasks;
using WikiLibs.Shared.Helpers;
using WikiLibs.Shared.Attributes;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace WikiLibs.Symbols
{
    [Module(Interface = typeof(ISymbolManager))]
    public class SymbolManager : BaseCRUDOperations<Data.Context, Symbol>, ISymbolManager
    {
        private Config _cfg;

        public SymbolManager(Data.Context db, Config cfg) : base(db)
        {
            _cfg = cfg;
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
            string libl = lang + "/" + lib + "/";

            if (!Set.Any(sy => sy.Path.StartsWith(libl)))
                Context.SymbolLibs.RemoveRange(Context.SymbolLibs.Where(e => e.Name == libl));
            await SaveChanges();
            return (sym);
        }

        public string[] GetFirstLangs()
        {
            return (Context.SymbolLangs
                .OrderBy(o => o.Name)
                .Take(_cfg.MaxSymsPerPage)
                .Select(o => o.Name)
                .ToArray());
        }

        public string[] GetFirstLibs(string lang)
        {
            return (Context.SymbolLibs.Where(o => o.Name.StartsWith(lang + "/"))
                .OrderBy(o => o.Name)
                .Take(_cfg.MaxSymsPerPage)
                .Select(o => o.Name)
                .ToArray());
        }

        public Symbol Get(string path)
        {
            var sym = Set.Where(o => o.Path == path);

            if (sym == null || sym.Count() <= 0)
                throw new Shared.Exceptions.ResourceNotFound()
                {
                    ResourceType = typeof(Symbol),
                    ResourceName = path
                };
            return (sym.First());
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
            string libl = lang + "/" + lib + "/";
            if (!Context.SymbolLangs.Any(e => e.Name == lang))
                throw new Shared.Exceptions.InvalidResource()
                {
                    PropertyName = "Lang",
                    ResourceType = typeof(Symbol),
                    ResourceName = sym.Path
                };
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
            Lib l = null;
            if (!Context.SymbolLibs.Any(e => e.Name == libl))
            {
                l = new Lib()
                {
                    Name = libl
                };
                Context.SymbolLibs.Add(l);
            }
            else
                l = Context.SymbolLibs.Where(e => e.Name == libl).FirstOrDefault();
            sym.Lib = l;
            sym.Lang = Context.SymbolLangs.Where(e => e.Name == lang).FirstOrDefault();
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
                if (s.Import != null)
                {
                    if (Context.SymbolImports.Where(e => e.Name == s.Import.Name).Count() == 1)
                        Context.SymbolImports.RemoveRange(Context.SymbolImports.Where(e => e.Name == s.Import.Name));
                }
                Import import = null;
                if (!Context.SymbolImports.Any(e => e.Name == sym.Import.Name))
                {
                    import = new Import()
                    {
                        Name = sym.Import.Name
                    };
                    Context.SymbolImports.Add(import);
                }
                else
                    import = Context.SymbolImports.Where(e => e.Name == sym.Import.Name).FirstOrDefault();
                s.Import = import;
            }
            if (sym.Prototypes != null)
            {
                Context.RemoveRange(s.Prototypes);
                foreach (var proto in sym.Prototypes)
                {
                    foreach (var param in proto.Parameters)
                        param.Id = 0;
                    proto.Id = 0;
                    proto.Symbol = s;
                    Context.Add(proto);
                }
            }
            if (sym.Symbols != null)
            {
                Context.RemoveRange(s.Symbols);
                foreach (var r in sym.Symbols)
                {
                    r.Id = 0;
                    Context.Add(r);
                }
            }
            await SaveChanges();
            return (s);
        }

        public PageResult<SymbolListItem> SearchSymbols(string path, PageOptions options)
        {
            options.EnsureValid(typeof(Symbol), "Symbol", _cfg.MaxSymsPerPage);
            var data = Set.Where(sym => sym.Path.Contains(path))
                .OrderBy(o => o.Path)
                .Skip((options.Page.Value - 1) * options.Count.Value);
            bool next = data.Count() > options.Count.Value;
            var arr = data.Take(options.Count.Value)
                .Select(sym => new SymbolListItem()
                {
                    Path = sym.Path,
                    Id = sym.Id,
                    Type = sym.Type.Name
                });

            return (new PageResult<SymbolListItem>()
            {
                Data = arr,
                HasMorePages = next,
                Page = options.Page.Value,
                Count = options.Count.Value
            });
        }

        public async Task OptimizeAsync()
        {
            var srefs = Context.SymbolRefs.Where(e => e.RefId == null);
            foreach (var sref in srefs)
            {
                var symbol = Set.Where(o => o.Path == sref.RefPath).AsNoTracking().FirstOrDefault();
                if (symbol != null)
                    sref.RefId = symbol.Id;
            }
            var sprefs = Context.PrototypeParamSymbolRefs.Where(e => e.RefId == null);
            foreach (var sref in sprefs)
            {
                var symbol = Set.Where(o => o.Path == sref.RefPath).AsNoTracking().FirstOrDefault();
                if (symbol != null)
                    sref.RefId = symbol.Id;
            }
            await SaveChanges();
        }

        [ModuleInitializer(Debug = true, Release = true)]
        public static void InitSymbols(ISymbolManager mgr)
        {
            mgr.OptimizeAsync().Wait();
        }
    }
}
