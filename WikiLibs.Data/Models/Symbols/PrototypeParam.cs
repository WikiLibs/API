using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WikiLibs.Data.Models.Symbols
{
    public class PrototypeParam : Model
    {
        public long PrototypeId { get; set; }
        public string Data { get; set; }
        public string Description { get; set; }

        public virtual Prototype Prototype { get; set; }
        public virtual PrototypeParamSymbolRef SymbolRef { get; set; }
    }
}
