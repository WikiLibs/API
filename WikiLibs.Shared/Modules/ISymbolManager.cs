using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Shared.Helpers;

namespace WikiLibs.Shared.Modules
{
    public interface ISymbolManager : IModule, ICRUDOperations<Data.Models.Symbols.Symbol>
    {
        Data.Models.Symbols.Symbol Get(string path);
        PageResult<string> SearchSymbols(string path, PageOptions options);
        string[] GetFirstLangs();
        string[] GetFirstLibs(string lang);
    }
}
