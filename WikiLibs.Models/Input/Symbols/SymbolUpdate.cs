using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikiLibs.Data.Models;
using WikiLibs.Data.Models.Symbols;

namespace WikiLibs.Models.Input.Symbols
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
                for (int i = 0; i != Prototypes.Length; ++i)
                {
                    var proto = Prototypes[i];
                    var old = i < current.Prototypes.Count ? current.Prototypes.ElementAt(i) : null;
                    var p = new Data.Models.Symbols.Prototype
                    {
                        Id = old != null ? old.Id : 0,
                        Data = proto.Proto != null ? proto.Proto : old.Data,
                        Description = proto.Description != null ? proto.Description : old.Description,
                        Symbol = sym
                    };
                    if (proto.Parameters != null)
                    {
                        for (int j = 0; j != proto.Parameters.Length; ++j)
                        {
                            var par = proto.Parameters[j];
                            var oldParam = (old != null && j < old.Parameters.Count) ? old.Parameters.ElementAt(j) : null;
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
                    {
                        foreach (var par in old.Parameters)
                        {
                            p.Parameters.Add(new PrototypeParam()
                            {
                                Data = par.Data,
                                Description = par.Description,
                                Id = par.Id,
                                Path = par.Path,
                                Prototype = p
                            });
                        }
                    }
                    sym.Prototypes.Add(p);
                }
            }
            else
                sym.Prototypes = null;
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
