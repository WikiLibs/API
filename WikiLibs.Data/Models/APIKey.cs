using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WikiLibs.Data.Models
{
    public class APIKey
    {
        [Key]
        public string Key { get; set; }
        public string Description { get; set; }
    }
}
