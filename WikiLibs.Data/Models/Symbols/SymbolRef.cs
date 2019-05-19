using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WikiLibs.Data.Models.Symbols
{
    public class SymbolRef : Model
    {
        [Required]
        public virtual Symbol Symbol { get; set; }
        public string Path { get; set; }
    }
}
