using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Data.Models.Symbols;

namespace WikiLibs.Models.Input.Symbols
{
    public class LangUpdate : PatchModel<LangUpdate, Lang>
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }

        public override Lang CreatePatch(in Lang current)
        {
            return (new Lang()
            {
                Name = Name != null ? Name : current.Name,
                DisplayName = DisplayName != null ? DisplayName : current.DisplayName,
                Id = current.Id
            });
        }
    }
}
