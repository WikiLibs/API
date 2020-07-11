using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WikiLibs.Data.Models;
using WikiLibs.Data.Models.Symbols;

namespace WikiLibs.Models.Input.Symbols
{
    public class SymbolCreate : PostModel<SymbolCreate, Symbol>
    {
        public class Exception
        {
            public string Description { get; set; }
            [Required]
            public string Ref { get; set; }
        }

        public class Prototype
        {
            public class Parameter
            {
                [Required]
                [JsonProperty(PropertyName = "prototype")]
                public string Proto { get; set; }
                public string Description { get; set; }
                public string Ref { get; set; }
            }

            [Required]
            [JsonProperty(PropertyName = "prototype")]
            public string Proto { get; set; }
            public string Description { get; set; }
            [Required]
            public Parameter[] Parameters { get; set; }
            public Exception[] Exceptions { get; set; }
        }

        [Required]
        public string Path { get; set; }
        [Required]
        public string Type { get; set; }
        public string Import { get; set; }
        [Required]
        public Prototype[] Prototypes { get; set; }
        [Required]
        public string[] Symbols { get; set; }

        public override Symbol CreateModel()
        {
            var sym = new Symbol()
            {
                LastModificationDate = DateTime.UtcNow,
                CreationDate = DateTime.UtcNow,
                Path = Path,
                Type = new Data.Models.Symbols.Type() { Name = Type },
                Import = Import != null ? new Import() { Name = Import } : null
            };
            foreach (var proto in Prototypes)
            {
                var p = new Data.Models.Symbols.Prototype
                {
                    Data = proto.Proto,
                    Description = proto.Description,
                    Symbol = sym
                };
                foreach (var par in proto.Parameters)
                {
                    var param = new PrototypeParam()
                    {
                        Data = par.Proto,
                        Description = par.Description,
                        SymbolRef = par.Ref != null ? new PrototypeParamSymbolRef() { RefPath = par.Ref } : null,
                        Prototype = p
                    };
                    p.Parameters.Add(param);
                }
                if (proto.Exceptions != null)
                {
                    foreach (var eref in proto.Exceptions)
                    {
                        var exref = new Data.Models.Symbols.Exception()
                        {
                            Description = eref.Description,
                            RefPath = eref.Ref,
                            Prototype = p
                        };
                        p.Exceptions.Add(exref);
                    }
                }
                sym.Prototypes.Add(p);
            }
            foreach (var sref in Symbols)
            {
                var symRef = new SymbolRef()
                {
                    RefPath = sref,
                    Symbol = sym
                };
                sym.Symbols.Add(symRef);
            }
            return (sym);
        }
    }
}
