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

            if (!Set.Any(sy => sy.Lang == sym.Lang))
                Context.InfoTable.RemoveRange(Context.InfoTable.Where(sy =>
                    sy.Type == EInfoType.LANG && sy.Data == lang));
            if (!Set.Any(sy => sy.Path.StartsWith(libl)))
                Context.InfoTable.RemoveRange(Context.InfoTable.Where(sy =>
                    sy.Type == EInfoType.LIB && sy.Data == libl));
            await SaveChanges();
            return (sym);
        }

        public string[] GetFirstLangs()
        {
            return (Context.InfoTable.Where(o => o.Type == EInfoType.LANG)
                .OrderBy(o => o.Data)
                .Take(_cfg.MaxSymsPerPage)
                .Select(o => o.Data)
                .ToArray());
        }

        public string[] GetFirstLibs(string lang)
        {
            return (Context.InfoTable.Where(o => o.Type == EInfoType.LIB && o.Data.StartsWith(lang + "/"))
                .OrderBy(o => o.Data)
                .Take(_cfg.MaxSymsPerPage)
                .Select(o => o.Data)
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
            if (Context.InfoTable.Where(o => o.Type == EInfoType.LANG && o.Data == lang).Count() <= 0)
                Context.InfoTable.Add(new Info { Type = EInfoType.LANG, Data = lang });
            if (Context.InfoTable.Where(o => o.Type == EInfoType.LIB && o.Data == libl).Count() <= 0)
                Context.InfoTable.Add(new Info { Type = EInfoType.LIB, Data = libl });
            sym.Lib = lib;
            sym.Lang = lang;
            return (await base.PostAsync(sym));
        }

        public override async Task<Symbol> PatchAsync(long key, Symbol sym)
        {
            var s = await GetAsync(key);

            s.LastModificationDate = sym.LastModificationDate;
            s.Type = sym.Type;
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

        public PageResult<SymbolSearchResult> SearchSymbols(string path, PageOptions options)
        {
            options.Page = options.Page != null ? options.Page.Value : 1;
            if (options.Page == 0)
                throw new Shared.Exceptions.InvalidResource()
                {
                    PropertyName = "PageNum",
                    ResourceName = "Symbol",
                    ResourceType = typeof(Symbol)
                };
            var data = Set.Where(sym => sym.Path.Contains(path))
                .OrderBy(o => o.Path)
                .Skip((options.Page.Value - 1) * _cfg.GetMaxSymbols(options));
            bool next = data.Count() > _cfg.GetMaxSymbols(options);
            var arr = data.Take(_cfg.GetMaxSymbols(options))
                .Select(sym => new SymbolSearchResult()
                {
                    Path = sym.Path,
                    Id = sym.Id,
                    Type = sym.Type
                });

            return (new PageResult<SymbolSearchResult>()
            {
                Data = arr,
                HasMorePages = next,
                Page = options.Page.Value,
                Count = _cfg.GetMaxSymbols(options)
            });
        }

        public async Task OptimizeAsync()
        {
            foreach (var sref in Context.SymbolRefs.Where(e => e.RefId == null))
            {
                var symbol = Get(sref.RefPath);
                if (symbol != null)
                    sref.RefId = symbol.Id;
            }
            foreach (var sref in Context.PrototypeParamSymbolRefs.Where(e => e.RefId == null))
            {
                var symbol = Get(sref.RefPath);
                if (symbol != null)
                    sref.RefId = symbol.Id;
            }
            await SaveChanges();
        }
    }
}
