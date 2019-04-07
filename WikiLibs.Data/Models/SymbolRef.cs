using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WikiLibs.Data.Models
{
    public class SymbolRef
    {
        public long Id { get; set; }
        [Required]
        public virtual Symbol Symbol { get; set; }
        public string Path { get; set; }
    }
}
