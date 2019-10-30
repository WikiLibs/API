using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Data.Models.Symbols
{
    public class Lang : Model
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public byte[] Icon { get; set; }
    }
}
