﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using WikiLibs.Data.Models.Symbols;

namespace WikiLibs.Models.Input.Symbols
{
    public class SymbolMerge : PutModel<SymbolMerge, Symbol, string>
    {
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
        }

        [Required]
        public string Type { get; set; }
        public string Import { get; set; }
        [Required]
        public Prototype[] Prototypes { get; set; }
        [Required]
        public string[] Symbols { get; set; }

        public override Symbol CreateModel(string key)
        {
            return (new SymbolCreate()
            {
                Path = key,
                Import = Import,
                Type = Type,
                Prototypes = Prototypes.Select((e) => new SymbolCreate.Prototype
                {
                    Description = e.Description,
                    Proto = e.Proto,
                    Parameters = e.Parameters.Select((p) => new SymbolCreate.Prototype.Parameter()
                    {
                        Description = p.Description,
                        Proto = p.Proto,
                        Ref = p.Ref
                    }).ToArray()
                }).ToArray(),
                Symbols = Symbols
            }.CreateModel());
        }

        public override Symbol CreatePatch(in Symbol current)
        {
            List<Prototype> lst = Prototypes.ToList();
            foreach (var proto in current.Prototypes)
            {
                if (!Prototypes.Any((e) => e.Proto == proto.Data))
                    lst.Add(new Prototype()
                    {
                        Description = proto.Description,
                        Proto = proto.Data,
                        Parameters = proto.Parameters.Select((p) => new Prototype.Parameter()
                        {
                            Description = p.Description,
                            Proto = p.Data,
                            Ref = p.SymbolRef != null ? p.SymbolRef.RefPath : null
                        }).ToArray()
                    });
            }
            var sym = new SymbolUpdate()
            {
                Import = Import,
                Type = Type,
                Prototypes = lst.Select((e) => new SymbolUpdate.Prototype
                {
                    Description = e.Description,
                    Proto = e.Proto,
                    Parameters = e.Parameters.Select((p) => new SymbolUpdate.Prototype.Parameter()
                    {
                        Description = p.Description,
                        Proto = p.Proto,
                        Ref = p.Ref
                    }).ToArray()
                }).ToArray(),
                Symbols = Symbols
            }.CreatePatch(current);
            return (sym);
        }
    }
}
