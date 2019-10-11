﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.Shared.Helpers;

namespace WikiLibs.Shared.Modules.Symbols
{
    public interface ISymbolManager : IModule, ICRUDOperations<Data.Models.Symbols.Symbol>
    {
        ILangManager LangManager { get; }
        ICRUDOperations<Data.Models.Symbols.Type> TypeManager { get; }

        Task<Data.Models.Symbols.Symbol> GetAsync(string path);
        PageResult<SymbolListItem> SearchSymbols(string path, PageOptions options);
        PageResult<SymbolListItem> GetSymbolsForLib(long id, PageOptions options);

        /// <summary>
        /// Call this function every time you are done pushing a set of symbols to optimize their access times by using symbol id instead of path
        /// </summary>
        Task OptimizeAsync();
    }
}