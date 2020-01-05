using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Models.Output.Symbols
{
    public class Type : GetModel<Type, Data.Models.Symbols.Type>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }

        public override void Map(in Data.Models.Symbols.Type model)
        {
            Id = model.Id;
            Name = model.Name;
            DisplayName = model.DisplayName;
        }
    }
}
