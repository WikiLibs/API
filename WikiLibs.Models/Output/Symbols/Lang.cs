using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Data.Models.Symbols;

namespace WikiLibs.Models.Output.Symbols
{
    public class Lang : GetModel<Lang, Data.Models.Symbols.Lang>
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public override void Map(in Data.Models.Symbols.Lang model)
        {
            Id = model.Id;
            Name = model.Name;
        }
    }
}
