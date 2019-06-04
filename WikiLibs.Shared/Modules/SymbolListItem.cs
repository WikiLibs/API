using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Shared.Modules
{
    public class SymbolListItem
    {
        public long Id { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
    }

    public class SymbolReference
    {
        public long Id { get; set; }
        public string Path { get; set; }
    }
}
