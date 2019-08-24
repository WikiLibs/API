using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.Shared.Helpers;

namespace WikiLibs.Shared.Modules
{
    public interface ISymbolManager : IModule, ICRUDOperations<Data.Models.Symbols.Symbol>
    {
        ICRUDOperations<Data.Models.Symbols.Lang> LangManager { get; }
        ICRUDOperations<Data.Models.Symbols.Type> TypeManager { get; }

        Data.Models.Symbols.Symbol Get(string path);
        PageResult<SymbolListItem> SearchSymbols(string path, PageOptions options);
        string[] GetFirstLangs();
        string[] GetFirstLibs(string lang);

        /// <summary>
        /// Call this function every time you are done pushing a set of symbols to optimize their access times by using symbol id instead of path
        /// </summary>
        Task OptimizeAsync();
    }
}
