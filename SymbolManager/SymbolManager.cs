using System;
using System.Collections.Generic;
using System.Linq;
using API.Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace SymbolManager
{
    [API.Module(typeof(API.Modules.ISymbolManager))]
    public class SymbolManager : API.Modules.ISymbolManager
    {
        private static int N = 0;
        private WikiLibs.DB.Context _db;

        public SymbolManager(WikiLibs.DB.Context db)
        {
            _db = db;
        }

        private string GetLibLangPath(Symbol sym)
        {
            string[] objs = sym.Path.Split('/');

            return (objs[0] + '/' + objs[1] + '/');
        }

        public int DeleteSymbol(Symbol sym)
        {
            var s = _db.Symbols.Find(new object[] { sym.Path });

            if (s == null)
                return (404);
            _db.Remove(s);
            _db.SaveChanges();
            if (_db.Symbols.Where(sy => sy.Lang == sym.Lang).Count() <= 0)
                _db.InfoTable.RemoveRange(_db.InfoTable.Where(sy =>
                    sy.Type == WikiLibs.DB.EInfoType.LANG && sy.Data == sym.Lang));
            string libl = GetLibLangPath(sym);
            if (_db.Symbols.Where(sy => sy.Path.StartsWith(libl)).Count() <= 0)
                _db.InfoTable.RemoveRange(_db.InfoTable.Where(sy =>
                    sy.Type == WikiLibs.DB.EInfoType.LIB && sy.Data == libl));
            return (200);
        }

        public string[] GetFirstLangs()
        {
            return (_db.InfoTable.Where(o => o.Type == WikiLibs.DB.EInfoType.LANG)
                .Take(N)
                .OrderBy(o => o.Data)
                .Select(o => o.Data)
                .ToArray());
        }

        public string[] GetFirstLibs(string lang)
        {
            return (_db.InfoTable.Where(o => o.Type == WikiLibs.DB.EInfoType.LIB && o.Data.StartsWith(lang + "/"))
                .Take(N)
                .OrderBy(o => o.Data)
                .Select(o => o.Data)
                .ToArray());
        }

        private Symbol ConvertSym(WikiLibs.DB.Symbol sym)
        {
            Symbol res = new Symbol
            {
                Path = sym.Path,
                Lang = sym.Lang,
                Type = sym.Type,
                Prototypes = JsonConvert.DeserializeObject<Symbol.Prototype[]>(sym.Prototypes),
                UserID = sym.UserID,
                Symbols = JsonConvert.DeserializeObject<string[]>(sym.Symbols),
                Date = sym.Date
            };

            return (res);
        }

        public Symbol GetSymbol(string path)
        {
            var sym = _db.Symbols.Find(new object[] { path });

            if (sym == null)
                return (null);
            return (ConvertSym(sym));
        }

        class Cfg
        {
            public int N { get; set; }
        }

        public void LoadConfig(IConfiguration cfg)
        {
            Cfg c = new Cfg();
            cfg.Bind("SymbolManager", c);
            N = c.N;
        }

        public string[] SearchSymbols(string path)
        {
            return (_db.Symbols.Where(sym => sym.Path.Contains(path))
                .Take(N)
                .Select(sym => sym.Path)
                .ToArray());
        }

        private bool CheckSymPath(Symbol sym)
        {
            string[] objs = sym.Path.Split('/');

            return (objs.Length > 2);
        }

        private int AddSymbol(Symbol sym)
        {
            var s = new WikiLibs.DB.Symbol
            {
                Lang = sym.Lang,
                Path = sym.Path,
                Prototypes = JsonConvert.SerializeObject(sym.Prototypes),
                Type = sym.Type,
                UserID = sym.UserID,
                Symbols = JsonConvert.SerializeObject(sym.Symbols),
                Date = DateTime.UtcNow
            };
            string libl = GetLibLangPath(sym);

            if (_db.InfoTable.Where(o => o.Type == WikiLibs.DB.EInfoType.LANG && o.Data == s.Lang).Count() <= 0)
                _db.InfoTable.Add(new WikiLibs.DB.Info { Type = WikiLibs.DB.EInfoType.LANG, Data = s.Lang });
            if (_db.InfoTable.Where(o => o.Type == WikiLibs.DB.EInfoType.LIB && o.Data == libl).Count() <= 0)
                _db.InfoTable.Add(new WikiLibs.DB.Info { Type = WikiLibs.DB.EInfoType.LIB, Data = libl });
            _db.Symbols.Add(s);
            _db.SaveChanges();
            return (200);
        }

        public int SetSymbol(Symbol sym)
        {
            if (!CheckSymPath(sym))
                return (400); //Bad request the symbol path is abnormal
            var s = _db.Symbols.Find(new object[] { sym.Path });
            if (s == null)
                return (AddSymbol(sym));
            s.Prototypes = JsonConvert.SerializeObject(sym.Prototypes);
            s.Type = sym.Type;
            s.Symbols = JsonConvert.SerializeObject(sym.Symbols);
            s.Date = DateTime.UtcNow;
            _db.SaveChanges();
            return (200);
        }
    }
}
