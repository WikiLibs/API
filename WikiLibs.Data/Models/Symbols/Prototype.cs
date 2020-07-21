using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WikiLibs.Data.Models.Symbols
{
    public class Prototype : Model
    {
        public long SymbolId { get; set; }
        public string Data { get; set; }
        public string Description { get; set; }

        public virtual Symbol Symbol { get; set; }
        public virtual ICollection<PrototypeParam> Parameters { get; set; } = new HashSet<PrototypeParam>();
        public virtual ICollection<Exception> Exceptions { get; set; } = new HashSet<Exception>();
    }
}
