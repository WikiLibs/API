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
            if (options.Count == null || options.Count.Value <= 0)
                return (MaxSymsPerPage);
            return (options.Count.Value > MaxSymsPerPage ? MaxSymsPerPage : options.Count.Value);
        }
    }
}
