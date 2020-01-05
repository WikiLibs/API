using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WikiLibs.Data.Models.Symbols;

namespace WikiLibs.Data.Models.Examples
{
    public class Example : Model
    {
        public long SymbolId { get; set; }
        public long? RequestId { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModificationDate { get; set; }

        public virtual User User { get; set; }
        public virtual Symbol Symbol { get; set; }
        public virtual ExampleRequest Request { get; set; }
        public virtual ICollection<ExampleCodeLine> Code { get; set; } = new HashSet<ExampleCodeLine>();
    }
}
