using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikiLibs.Data.Models;

namespace WikiLibs.DTO.Input
{
    public class SymbolUpdate : IPatchDTO<Data.Models.Symbol>
    {
        public class Prototype
        {
            public class Parameter
            {
                public string prototype { get; set; }
                public string description { get; set; }
                public string path { get; set; }
            }

            public string prototype { get; set; }
            public string description { get; set; }
            public Parameter[] parameters { get; set; }
        }

        public string type { get; set; }
        public Prototype[] prototypes { get; set; }
        public string[] symbols { get; set; }

        public Symbol CreatePatch(in Symbol current)
        {
            var sym = new Symbol()
            {
                CreationDate = current.CreationDate,
                LastModificationDate = DateTime.UtcNow,
                Lang = current.Lang,
                Path = current.Path,
                Id = current.Id,
                User = current.User,
                Type = type != null ? type : current.Type
            };
            if (prototypes != null)
            {
                foreach (var proto in prototypes)
                {
                    var old = current.Prototypes.Where(p1 => p1.Data == proto.prototype).FirstOrDefault();
                    var p = new Data.Models.Prototype
                    {
                        Id = old != null ? old.Id : 0,
                        Data = proto.prototype != null ? proto.prototype : old.Data,
                        Description = proto.description != null ? proto.description : old.Description,
                        Symbol = sym
                    };
                    if (proto.parameters != null)
                    {
                        foreach (var par in proto.parameters)
                        {
                            var oldParam = old != null ? old.Parameters.Where(p1 => p1.Data == par.prototype).FirstOrDefault() : null;
                            var param = new PrototypeParam()
                            {
                                Id = oldParam != null ? oldParam.Id : 0,
                                Data = par.prototype != null ? par.prototype : oldParam.Data,
                                Description = par.description != null ? par.description : oldParam.Description,
                                Path = par.path != null ? par.path : oldParam.Path,
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
            if (symbols != null)
            {
                foreach (var sref in symbols)
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
