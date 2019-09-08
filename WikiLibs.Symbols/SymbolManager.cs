﻿using System;
using System.Collections.Generic;
using System.Linq;
using WikiLibs.Data.Models.Symbols;
using WikiLibs.Shared;
using System.Threading.Tasks;
using WikiLibs.Shared.Helpers;
using WikiLibs.Shared.Attributes;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using WikiLibs.Shared.Modules.Symbols;

namespace WikiLibs.Symbols
{
    [Module(Interface = typeof(ISymbolManager))]
    public class SymbolManager : BaseCRUDOperations<Data.Context, Symbol>, ISymbolManager
    {
        public ILangManager LangManager { get; }

        public ICRUDOperations<Data.Models.Symbols.Type> TypeManager { get; }

        public SymbolManager(Data.Context db, Config cfg) : base(db)
        {
            MaxResults = cfg.MaxSymsPerPage;
            LangManager = new LangManager(db);
            TypeManager = new TypeManager(db);
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
                if (Context.Symbols.Where(e => e.Import != null && e.Import.Name == s.Import.Name).Count() == 1)
                    Context.RemoveRange(Context.SymbolImports.Where(e => e.Name == s.Import.Name));
                Import import = null;
                if (!Context.SymbolImports.Any(e => e.Name == sym.Import.Name))
                    import = new Import() { Name = sym.Import.Name };
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
            return (base.ToPageResult<SymbolListItem>(options,
                Set.Where(sym => sym.Path.Contains(path))
                   .OrderBy(o => o.Path)));
        }

        public PageResult<SymbolListItem> GetSymbolsForLib(long id, PageOptions options)
        {
            return (base.ToPageResult<SymbolListItem>(options,
                Set.Where(sym => sym.LibId == id)
                   .OrderBy(sym => sym.Path)));
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
