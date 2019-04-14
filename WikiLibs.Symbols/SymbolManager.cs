using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WikiLibs.API.Modules;
using WikiLibs.Data.Models;

namespace WikiLibs.Symbols
{
    [API.Module(typeof(ISymbolManager))]
    public class SymbolManager : ISymbolManager
    {
        private Data.Context _db;
        private Config _cfg;

        public SymbolManager(Data.Context db, Config cfg)
        {
            _db = db;
            _cfg = cfg;
        }

        private string GetLibLangPath(Symbol sym)
        {
            string[] objs = sym.Path.Split('/');

            return (objs[0] + '/' + objs[1] + '/');
        }

        public void DeleteSymbol(Symbol sym)
        {
            var s = _db.Symbols.Find(new object[] { sym.Id });

            if (s == null)
                throw new API.Exceptions.ResourceNotFound()
                {
                    ResourceType = typeof(Symbol),
                    ResourceId = sym.Id.ToString(),
                    ResourceName = sym.Path
                };
            _db.Remove(s);
            _db.SaveChanges();
            if (_db.Symbols.Where(sy => sy.Lang == sym.Lang).Count() <= 0)
                _db.InfoTable.RemoveRange(_db.InfoTable.Where(sy =>
                    sy.Type == EInfoType.LANG && sy.Data == sym.Lang));
            string libl = GetLibLangPath(sym);
            if (_db.Symbols.Where(sy => sy.Path.StartsWith(libl)).Count() <= 0)
                _db.InfoTable.RemoveRange(_db.InfoTable.Where(sy =>
                    sy.Type == EInfoType.LIB && sy.Data == libl));
        }

        public string[] GetFirstLangs()
        {
            return (_db.InfoTable.Where(o => o.Type == EInfoType.LANG)
                .OrderBy(o => o.Data)
                .Take(_cfg.MaxSymsPerPage)
                .Select(o => o.Data)
                .ToArray());
        }

        public string[] GetFirstLibs(string lang)
        {
            return (_db.InfoTable.Where(o => o.Type == EInfoType.LIB && o.Data.StartsWith(lang + "/"))
                .OrderBy(o => o.Data)
                .Take(_cfg.MaxSymsPerPage)
                .Select(o => o.Data)
                .ToArray());
        }

        public Symbol GetSymbol(string path)
        {
            var sym = _db.Symbols.Where(o => o.Path == path);

            if (sym == null || sym.Count() <= 0)
                throw new API.Exceptions.ResourceNotFound()
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

        public void CreateSymbol(Symbol sym)
        {
            if (!CheckSymPath(sym))
                throw new API.Exceptions.InvalidResource()
                {
                    PropertyName = "Path",
                    ResourceType = typeof(Symbol),
                    ResourceName = sym.Path
                };
            if (_db.Symbols.Any(o => o.Path == sym.Path))
                throw new API.Exceptions.ResourceAlreadyExists()
                {
                    ResourceId = _db.Symbols.Where(o => o.Path == sym.Path).First().Id.ToString(),
                    ResourceType = typeof(Symbol),
                    ResourceName = sym.Path
                };
            string libl = GetLibLangPath(sym);
            if (_db.InfoTable.Where(o => o.Type == EInfoType.LANG && o.Data == sym.Lang).Count() <= 0)
                _db.InfoTable.Add(new Info { Type = EInfoType.LANG, Data = sym.Lang });
            if (_db.InfoTable.Where(o => o.Type == EInfoType.LIB && o.Data == libl).Count() <= 0)
                _db.InfoTable.Add(new Info { Type = EInfoType.LIB, Data = libl });
            _db.Symbols.Add(sym);
            _db.SaveChanges();
        }

        public void PatchSymbol(Symbol sym)
        {
            var s = _db.Symbols.Find(new object[] { sym.Id });
            if (s == null)
                throw new API.Exceptions.ResourceNotFound()
                {
                    ResourceType = typeof(Symbol),
                    ResourceName = sym.Path,
                    ResourceId = sym.Id.ToString()
                };
            s.LastModificationDate = sym.LastModificationDate;
            s.Type = sym.Type;
            foreach (var proto in s.Prototypes)
            {
                var edit = sym.Prototypes.Where(p => p.Id == proto.Id).SingleOrDefault();
                if (edit == null)
                {
                    _db.Remove(proto);
                    continue;
                }
                proto.Data = edit.Data;
                proto.Description = edit.Description;
                foreach (var param in proto.Parameters)
                {
                    var edit1 = edit.Parameters.Where(p => p.Id == param.Id).SingleOrDefault();
                    if (edit1 == null)
                    {
                        _db.Remove(param);
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
            _db.SaveChanges();
        }

        public SymbolSearchResult SearchSymbols(int page, string path)
        {
            var data = _db.Symbols.Where(sym => sym.Path.Contains(path))
                .OrderBy(o => o.Path)
                .Skip(page * _cfg.MaxSymsPerPage);
            bool next = data.Count() > _cfg.MaxSymsPerPage;
            var arr = data.Take(_cfg.MaxSymsPerPage)
                .Select(sym => sym.Path)
                .ToArray();
            var res = new SymbolSearchResult();

            res.HasNext = next;
            res.Symbols = arr;
            return (res);
        }
    }
}
