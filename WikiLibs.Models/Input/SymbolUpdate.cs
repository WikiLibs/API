using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikiLibs.Data.Models;

namespace WikiLibs.Models.Input
{
    public class SymbolUpdate : PatchModel<SymbolUpdate, Symbol>
    {
        public class Prototype
        {
            public class Parameter
            {
                [JsonProperty(PropertyName = "prototype")]
                public string Proto { get; set; }
                public string Description { get; set; }
                public string Path { get; set; }
            }

            [JsonProperty(PropertyName = "prototype")]
            public string Proto { get; set; }
            public string Description { get; set; }
            public Parameter[] Parameters { get; set; }
        }

        public string Type { get; set; }
        public Prototype[] Prototypes { get; set; }
        public string[] Symbols { get; set; }

        public override Symbol CreatePatch(in Symbol current)
        {
            var sym = new Symbol()
            {
                CreationDate = current.CreationDate,
                LastModificationDate = DateTime.UtcNow,
                Lang = current.Lang,
                Path = current.Path,
                Id = current.Id,
                User = current.User,
                Type = Type != null ? Type : current.Type
            };
            if (Prototypes != null)
            {
                foreach (var proto in Prototypes)
                {
                    var old = current.Prototypes.Where(p1 => p1.Data == proto.Proto).FirstOrDefault();
                    var p = new Data.Models.Prototype
                    {
                        Id = old != null ? old.Id : 0,
                        Data = proto.Proto != null ? proto.Proto : old.Data,
                        Description = proto.Description != null ? proto.Description : old.Description,
                        Symbol = sym
                    };
                    if (proto.Parameters != null)
                    {
                        foreach (var par in proto.Parameters)
                        {
                            var oldParam = old != null ? old.Parameters.Where(p1 => p1.Data == par.Proto).FirstOrDefault() : null;
                            var param = new PrototypeParam()
                            {
                                Id = oldParam != null ? oldParam.Id : 0,
                                Data = par.Proto != null ? par.Proto : oldParam.Data,
                                Description = par.Description != null ? par.Description : oldParam.Description,
                                Path = par.Path != null ? par.Path : oldParam.Path,
                                Prototype = p
                            };
                            p.Parameters.Add(param);
                        }
                    }
                    else
                        p.Parameters = old.Parameters;
                }
            }
            else
                sym.Prototypes = current.Prototypes;
            if (Symbols != null)
            {
                foreach (var sref in Symbols)
                {
                    var symRef = new SymbolRef()
                    {
                        Path = sref,
                        Symbol = sym
                    };
                    sym.Symbols.Add(symRef);
                }
            }
            else
                sym.Symbols = current.Symbols;
            return (sym);
        }
    }
}
