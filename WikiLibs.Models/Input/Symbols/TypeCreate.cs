using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WikiLibs.Models.Input.Symbols
{
    public class TypeCreate : PostModel<TypeCreate, Data.Models.Symbols.Type>
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string DisplayName { get; set; }

        public override Data.Models.Symbols.Type CreateModel()
        {
            return (new Data.Models.Symbols.Type()
            {
                Name = Name,
                DisplayName = DisplayName
            });
        }
    }
}
