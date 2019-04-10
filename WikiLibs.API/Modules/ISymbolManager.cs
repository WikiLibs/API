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

    public interface ISymbolManager : IModule
    {
        Data.Models.Symbol GetSymbol(string path);
        SymbolSearchResult SearchSymbols(int page, string path);
        string[] GetFirstLangs();
        string[] GetFirstLibs(string lang);

        /**
         * Attempts to delete a symbol
         * Returns null when succeeded, error code otherwise
         */
        void DeleteSymbol(Data.Models.Symbol sym);

        /**
         * Attempts to create a symbol
         * Returns null when succeeded, error code otherwise
         */
        void CreateSymbol(Data.Models.Symbol sym);

        void PatchSymbol(Data.Models.Symbol sym);
    }
}
