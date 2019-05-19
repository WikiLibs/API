using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WikiLibs.Data.Models.Examples;

namespace WikiLibs.Data.Models.Symbols
{
    public class Symbol : Model
    {
        public string Path { get; set; }
        public virtual User User { get; set; }
        public string Lang { get; set; }
        public string Type { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModificationDate { get; set; }

        public virtual ICollection<SymbolRef> Symbols { get; set; } = new HashSet<SymbolRef>();
        public virtual ICollection<Example> Examples { get; set; } = new HashSet<Example>();
        public virtual ICollection<Prototype> Prototypes { get; set; } = new HashSet<Prototype>();
    }
}
