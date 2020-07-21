using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Data.Models.Symbols
{
    public class Exception : Model
    {
        public long PrototypeId { get; set; }
        public long? RefId { get; set; }
        public string RefPath { get; set; }
        public string Description { get; set; }

        public virtual Prototype Prototype { get; set; }
        public virtual Symbol Ref { get; set; }
    }
}
