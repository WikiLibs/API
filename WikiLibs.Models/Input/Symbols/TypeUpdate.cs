using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Data.Models.Symbols;

namespace WikiLibs.Models.Input.Symbols
{
    public class TypeUpdate : PatchModel<TypeUpdate, Data.Models.Symbols.Type>
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }

        public override Data.Models.Symbols.Type CreatePatch(in Data.Models.Symbols.Type current)
        {
            return (new Data.Models.Symbols.Type()
            {
                Name = Name != null ? Name : current.Name,
                DisplayName = DisplayName != null ? DisplayName : current.DisplayName
                Id = current.Id
            });
        }
    }
}
