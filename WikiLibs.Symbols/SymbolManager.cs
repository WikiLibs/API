﻿using System;
using System.Collections.Generic;
using System.Linq;
using WikiLibs.Shared.Modules;
using WikiLibs.Data.Models;
using WikiLibs.Shared;
using System.Threading.Tasks;
using WikiLibs.Shared.Helpers;
using WikiLibs.Shared.Attributes;

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

        private string GetLibLangPath(Symbol sym)
        {
            string[] objs = sym.Path.Split('/');

            return (objs[0] + '/' + objs[1] + '/');
        }

        public override async Task<Symbol> DeleteAsync(Symbol sym)
        {
            await base.DeleteAsync(sym);
            if (!Set.Any(sy => sy.Lang == sym.Lang))
                Context.InfoTable.RemoveRange(Context.InfoTable.Where(sy =>
                    sy.Type == EInfoType.LANG && sy.Data == sym.Lang));
            string libl = GetLibLangPath(sym);
            if (!Set.Any(sy => sy.Path.StartsWith(libl)))
                Context.InfoTable.RemoveRange(Context.InfoTable.Where(sy =>
                    sy.Type == EInfoType.LIB && sy.Data == libl));
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
            if (Set.Any(o => o.Path == sym.Path))
                throw new Shared.Exceptions.ResourceAlreadyExists()
                {
                    ResourceId = Set.Where(o => o.Path == sym.Path).First().Id.ToString(),
                    ResourceType = typeof(Symbol),
                    ResourceName = sym.Path
                };
            string libl = GetLibLangPath(sym);
            if (Context.InfoTable.Where(o => o.Type == EInfoType.LANG && o.Data == sym.Lang).Count() <= 0)
                Context.InfoTable.Add(new Info { Type = EInfoType.LANG, Data = sym.Lang });
            if (Context.InfoTable.Where(o => o.Type == EInfoType.LIB && o.Data == libl).Count() <= 0)
                Context.InfoTable.Add(new Info { Type = EInfoType.LIB, Data = libl });
            return (await base.PostAsync(sym));
        }

        public override async Task<Symbol> PatchAsync(long key, Symbol sym)
        {
            var s = await GetAsync(key);

            s.LastModificationDate = sym.LastModificationDate;
            s.Type = sym.Type;
            foreach (var proto in s.Prototypes)
            {
                var edit = sym.Prototypes.Where(p => p.Id == proto.Id).SingleOrDefault();
                if (edit == null)
                {
                    Context.Remove(proto);
                    continue;
                }
                proto.Data = edit.Data;
                proto.Description = edit.Description;
                foreach (var param in proto.Parameters)
                {
                    var edit1 = edit.Parameters.Where(p => p.Id == param.Id).SingleOrDefault();
                    if (edit1 == null)
                    {
                        Context.Remove(param);
                        continue;
                    }
                    param.Path = edit1.Path;
                    param.Description = edit1.Description;
                    param.Data = edit1.Data;
                    foreach (var toRm in edit.Parameters.Where(p => p.Id == param.Id))
                        edit.Parameters.Remove(toRm);
                }
                foreach (var toRm in sym.Prototypes.Where(p => p.Id == proto.Id))
                    sym.Prototypes.Remove(toRm);
            }
            foreach (var proto in sym.Prototypes)
                s.Prototypes.Add(proto);
            await SaveChanges();
            return (s);
        }

        public PageResult<string> SearchSymbols(string path, PageOptions options)
        {
            var data = Set.Where(sym => sym.Path.Contains(path))
                .OrderBy(o => o.Path)
                .Skip(options.PageNum * _cfg.GetMaxSymbols(options));
            bool next = data.Count() > _cfg.GetMaxSymbols(options);
            var arr = data.Take(_cfg.GetMaxSymbols(options))
                .Select(sym => sym.Path);

            return (new PageResult<string>()
            {
                Data = arr,
                HasMorePages = next,
                PageNum = options.PageNum,
                PageSize = options.PageSize
            });
        }
    }
}
