using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Data.Models.Symbols;

namespace WikiLibs.Models.Output.Lib
{
    public class Lib : GetModel<Lib, Data.Models.Symbols.Lib>
    {
        public string Name { get; set; }
        public byte[] Icon { get; set; }
        public long Id { get; set; }
        public string Description { get; set; }
        public string Copyright { get; set; }

        public override void Map(in Data.Models.Symbols.Lib model)
        {
            Name = model.Name;
            Icon = model.Icon;
            Id = model.Id;
            Description = model.Description;
            Copyright = model.Copyright
        }
    }
}