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
        public string UserId { get; set; }
        public int LangId { get; set; }
        public int LibId { get; set; }
        public int TypeId { get; set; }
        public int? ImportId { get; set; }
        public int Views { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModificationDate { get; set; }

        public virtual User User { get; set; }
        public virtual Lang Lang { get; set; }
        public virtual Lib Lib { get; set; }
        public virtual Type Type { get; set; }
        public virtual Import Import { get; set; }
        public virtual ICollection<SymbolRef> Symbols { get; set; } = new HashSet<SymbolRef>();
        public virtual ICollection<Example> Examples { get; set; } = new HashSet<Example>();
        public virtual ICollection<Prototype> Prototypes { get; set; } = new HashSet<Prototype>();
    }
}
