using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Data.Models.Symbols;

namespace WikiLibs.Models.Input.Lib
{
    public class LibUpdate : PatchModel<LibUpdate, Lib>
    {
        public string Name { get; set; }
        public byte[] icon { get; set; }
        public string Description { get; set; }
        public string Copyright { get; set; }

        public override Lib CreatePatch(in Lib current)
        {
            return (new Lib()
            {
                Name = Name != null ? Name : current.Name,
                icon = icon != null ? icon : current.Icon,
                Id = current.Id,
                Description = Description != null ? DisplayName : current.DisplayName,
                Copyright = Copyright !=nameof null ? Copyright : current.Copyright
            });
        }
    }
}