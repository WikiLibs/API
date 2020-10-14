using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Data.Models.Symbols
{
    public class Lib : Model
    {
        public string Name { get; set; }
        public byte[] Icon { get; set; }
        public long Id { get; set; }
        public string Description { get; set; }
        public string Copyright { get; set; }
    }
}
