using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Shared.Attributes;
using WikiLibs.Shared.Helpers;

namespace WikiLibs.Symbols
{
    [Configurator]
    public class Config
    {
        public int MaxSymsPerPage { get; set; }

        public int GetMaxSymbols(PageOptions options)
        {
            if (options.PageSize <= 0)
                return (MaxSymsPerPage);
            return (options.PageSize > MaxSymsPerPage ? MaxSymsPerPage : options.PageSize);
        }
    }
}
