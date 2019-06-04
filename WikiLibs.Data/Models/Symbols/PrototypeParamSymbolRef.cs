using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Data.Models.Symbols
{
    public class PrototypeParamSymbolRef : Model
    {
        public long PrototypeParamId { get; set; }
        public long? RefId { get; set; }
        public string RefPath { get; set; }

        public virtual PrototypeParam PrototypeParam { get; set; }
        public virtual Symbol Ref { get; set; }
    }
}
