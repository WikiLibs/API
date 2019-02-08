using System;
using System.Collections.Generic;
using System.Text;

namespace API.Modules
{
    public interface ISymbolManager : IModule
    {
        Entities.Symbol GetSymbol(string path);
        string[] SearchSymbols(string path);
        string[] GetFirstLangs();
        string[] GetFirstLibs(string lang);

        /**
         * Attempts to delete a symbol
         * Returns null when succeeded, error code otherwise
         */
        int DeleteSymbol(Entities.Symbol sym);

        /**
         * Attempts to update/create a symbol
         * Returns null when succeeded, error code otherwise
         */
        int SetSymbol(Entities.Symbol sym);
    }
}
