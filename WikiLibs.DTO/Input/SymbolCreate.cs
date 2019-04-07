using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WikiLibs.Data.Models;

namespace WikiLibs.DTO.Input
{
    public class SymbolCreate : IPostDTO<Data.Models.Symbol>
    {
        public class Prototype
        {
            public class Parameter
            {
                [Required]
                public string prototype { get; set; }
                [Required]
                public string description { get; set; }
                [Required]
                public string path { get; set; }
            }

            [Required]
            public string prototype { get; set; }
            [Required]
            public string description { get; set; }
            [Required]
            public Parameter[] parameters { get; set; }
        }

        [Required]
        public string path { get; set; }
        [Required]
        public string lang { get; set; }
        [Required]
        public string type { get; set; }
        [Required]
        public Prototype[] prototypes { get; set; }
        [Required]
        public string[] symbols { get; set; }

        public Symbol CreateNew()
        {
            var sym = new Symbol()
            {
                LastModificationDate = DateTime.UtcNow,
                CreationDate = DateTime.UtcNow,
                Lang = lang,
                Path = path,
                Type = type,
            };
            foreach (var proto in prototypes)
            {
                var p = new Data.Models.Prototype
                {
                    Data = proto.prototype,
                    Description = proto.description,
                    Symbol = sym
                };
                foreach (var par in proto.parameters)
                {
                    var param = new PrototypeParam()
                    {
                        Data = par.prototype,
                        Description = par.description,
                        Path = par.path,
                        Prototype = p
                    };
                    p.Parameters.Add(param);
                }
            }
            foreach (var sref in symbols)
            {
                var symRef = new SymbolRef()
                {
                    Path = sref,
                    Symbol = sym
                };
                sym.Symbols.Add(symRef);
            }
            return (sym);
        }
    }
}
