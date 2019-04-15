using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WikiLibs.Data.Models
{
    public class Permission : Model
    {
        [Required]
        public virtual Group Group { get; set; }
        public string Perm { get; set; }
    }
}
