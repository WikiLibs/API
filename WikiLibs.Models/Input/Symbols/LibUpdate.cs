using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Data.Models.Symbols;

namespace WikiLibs.Models.Input.Symbols
{
    public class LibUpdate : PatchModel<LibUpdate, Lib>
    {
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Copyright { get; set; }

        public override Lib CreatePatch(in Lib current)
        {
            return (new Lib()
            {
                DisplayName = DisplayName != null ? DisplayName : current.DisplayName,
                Id = current.Id,
                Description = Description != null ? Description : current.Description,
                Copyright = Copyright != null ? Copyright : current.Copyright
            });
        }
    }
}