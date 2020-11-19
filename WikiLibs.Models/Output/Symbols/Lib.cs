using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using WikiLibs.Data.Models.Symbols;

namespace WikiLibs.Models.Output.Symbols
{
    public class Lib : GetModel<Lib, Data.Models.Symbols.Lib>
    {
        public string DisplayName { get; set; }
        public long Id { get; set; }
        public string Description { get; set; }
        public string Copyright { get; set; }
        public string UserId { get; set; }

        public override void Map(in Data.Models.Symbols.Lib model)
        {
            DisplayName = model.DisplayName;
            Id = model.Id;
            Description = model.Description;
            Copyright = model.Copyright;
            UserId = model.UserId;
        }
    }
}