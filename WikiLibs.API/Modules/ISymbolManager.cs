using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.API.Modules
{
    public struct SymbolSearchResult
    {
        public bool HasNext;
        public string[] Symbols;
    }

    public interface ISymbolManager : IModule, ICRUDOperations<Data.Models.Symbol, string, string>
    {
        SymbolSearchResult SearchSymbols(int page, string path);
        string[] GetFirstLangs();
        string[] GetFirstLibs(string lang);
    }
}
