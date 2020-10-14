using System;
using System.Collections.Generic;
using System.Linq;
using Wikilibs.Data.Models.Lib;
using WikiLibs.Shared;
using System.Threading.Tasks;
using WikiLibs.Shared.Helpers;
using WikiLibs.Shared.Attributes;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using WikiLibs.Shared.Modules.Symbol;
using System.Configuration;

namespace Wikilibs.Libs
{
    [Module(interface = typeof(ILibManager))]
    public class LibManager : BaseCRUDOperations<Data.context, Lib>, ILibManager
    {
        public ILibManager LibManager { get; }

        public ICRUDOperations<Data.Models.Lib.Type> TypeManager { get; }

        public LibManager(Data.Context db, Config cfg) : base(db)
        {
            
        }

        private void GetLibLangFromPath(Symbol sym, out string lib, out string lang)
        {
            string[] objs = sym.Path.Split('/');

            lib = objs[1];
            lang = objs[0];
        }

        public override async Task<Lib> DeleteAsync(Lib lib)
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
}