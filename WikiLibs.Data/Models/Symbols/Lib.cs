using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Data.Models.Symbols
{
    public class Lib : Model
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string LangName { get; set; }
        public byte[] Icon { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public string Copyright { get; set; }

        public virtual User User { get; set; }
    }
}
