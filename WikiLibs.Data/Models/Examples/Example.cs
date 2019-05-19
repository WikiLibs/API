using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WikiLibs.Data.Models.Symbols;

namespace WikiLibs.Data.Models.Examples
{
    public class Example : Model
    {
        [Required]
        public virtual Symbol Symbol { get; set; }
        public string Description { get; set; }
        public virtual User User { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModificationDate { get; set; }

        public virtual ICollection<ExampleCodeLine> Code { get; set; } = new HashSet<ExampleCodeLine>();
    }
}
