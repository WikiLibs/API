using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikiLibs.Data.Models.Symbols;

namespace WikiLibs.Models.Output.Symbols
{
    public class LightweightSymbol : GetModel<LightweightSymbol, Data.Models.Symbols.Symbol>
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public string FirstPrototype { get; set; }
        public string Path { get; set; }

        public override void Map(in Data.Models.Symbols.Symbol model)
        {
            Id = model.Id;
            Type = model.Type.DisplayName;
            FirstPrototype = model.Prototypes.Count > 0 ? model.Prototypes.First().Data : null;
            Path = model.Path;
        }
    }
}
