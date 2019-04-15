﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WikiLibs.Data.Models;

namespace WikiLibs.Models.Input
{
    public class SymbolCreate : PostModel<SymbolCreate, Symbol>
    {
        public class Prototype
        {
            public class Parameter
            {
                [Required]
                [JsonProperty(PropertyName = "prototype")]
                public string Proto { get; set; }
                [Required]
                public string Description { get; set; }
                [Required]
                public string Path { get; set; }
            }

            [Required]
            [JsonProperty(PropertyName = "prototype")]
            public string Proto { get; set; }
            [Required]
            public string Description { get; set; }
            [Required]
            public Parameter[] Parameters { get; set; }
        }

        [Required]
        public string Path { get; set; }
        [Required]
        public string Lang { get; set; }
        [Required]
        public string Type { get; set; }
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
                Lang = Lang,
                Path = Path,
                Type = Type,
            };
            foreach (var proto in Prototypes)
            {
                var p = new Data.Models.Prototype
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
                        Path = par.Path,
                        Prototype = p
                    };
                    p.Parameters.Add(param);
                }
            }
            foreach (var sref in Symbols)
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