using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WikiLibs.Data.Models.Symbols;

namespace WikiLibs.Models.Input.Symbols
{
    public class LangCreate : PostModel<LangCreate, Lang>
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string DisplayName { get; set; }

        public override Lang CreateModel()
        {
            return (new Lang()
            {
                Name = Name,
                DisplayName = DisplayName
            });
        }
    }
}
