using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WikiLibs.DB
{
    public class Symbol
    {
        [Key]
        public string Path { get; set; }
        public string UserID { get; set; }
        public string Lang { get; set; }
        public string Type { get; set; }
        public string Prototypes { get; set; }
        public string Symbols { get; set; }
    }
}
