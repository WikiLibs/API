using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Models.Input.Symbols
{
    public class TypeCreate : PostModel<TypeCreate, Data.Models.Symbols.Type>
    {
        public string Name { get; set; }

        public override Data.Models.Symbols.Type CreateModel()
        {
            return (new Data.Models.Symbols.Type()
            {
                Name = Name
            });
        }
    }
}
