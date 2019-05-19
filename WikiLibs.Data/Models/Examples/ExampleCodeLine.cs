using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Data.Models
{
    public class ExampleCodeLine : Model
    {
        public virtual Example Example { get; set; }
        public string Data { get; set; }
        public string Comment { get; set; }
    }
}
