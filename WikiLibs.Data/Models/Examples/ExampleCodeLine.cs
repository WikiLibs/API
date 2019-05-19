using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WikiLibs.Data.Models.Examples
{
    public class ExampleCodeLine : Model
    {
        [Required]
        public virtual Example Example { get; set; }
        public string Data { get; set; }
        public string Comment { get; set; }
    }
}
