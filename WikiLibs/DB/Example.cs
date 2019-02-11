using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WikiLibs.DB
{
    public class Example
    {
        [Key]
        public uint ID { get; set; }
        public string Path { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string UserID { get; set; }
    }
}
