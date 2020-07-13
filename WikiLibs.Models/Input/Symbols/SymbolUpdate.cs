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
            public class Exception
            {
                public string Description { get; set; }
                public string Ref { get; set; }
            }

            public class Parameter
            {
                [JsonProperty(PropertyName = "prototype")]
                public string Proto { get; set; }
                public string Description { get; set; }
                public string Ref { get; set; }
            }

            [JsonProperty(PropertyName = "prototype")]
            public string Proto { get; set; }
            public string Description { get; set; }
            public Parameter[] Parameters { get; set; }
            public Exception[] Exceptions { get; set; }
        }

        public string Type { get; set; }
        public string Import { get; set; }
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
                Type = Type != null ? new Data.Models.Symbols.Type() { Name = Type } : null,
                Import = Import != null ? new Data.Models.Symbols.Import() { Name = Import } : current.Import
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
                        Data = proto.Proto != null ? proto.Proto : (old != null ? old.Data : null),
                        Description = proto.Description != null ? proto.Description : (old != null ? old.Description : null),
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
                                Data = par.Proto != null ? par.Proto : (oldParam != null ? oldParam.Data : null),
                                Description = par.Description != null ? par.Description : (oldParam != null ? oldParam.Description : null),
                                SymbolRef = par.Ref != null ? new PrototypeParamSymbolRef() { RefPath = par.Ref } : null,
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
                                SymbolRef = par.SymbolRef != null ? new PrototypeParamSymbolRef()
                                {
                                    RefPath = par.SymbolRef.RefPath,
                                    RefId = par.SymbolRef.RefId
                                } : null,
                                Prototype = p
                            });
                        }
                    }
                    if (proto.Exceptions != null)
                    {
                        for (int j = 0; j != proto.Exceptions.Length; ++j)
                        {
                            var ex = proto.Exceptions[j];
                            var oldEx = (old != null && j < old.Exceptions.Count) ? old.Exceptions.ElementAt(j) : null;
                            var exception = new Data.Models.Symbols.Exception()
                            {
                                Id = oldEx != null ? oldEx.Id : 0,
                                Description = ex.Description != null ? ex.Description : (oldEx != null ? oldEx.Description : null),
                                RefPath = ex.Ref != null ? ex.Ref : null,
                                Prototype = p
                            };
                            p.Exceptions.Add(exception);
                        }
                    }
                    else
                    {
                        foreach (var ex in old.Exceptions)
                        {
                            p.Exceptions.Add(new Data.Models.Symbols.Exception()
                            {
                                Description = ex.Description,
                                Id = ex.Id,
                                RefId = ex.RefId,
                                RefPath = ex.RefPath,
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
                        RefPath = sref,
                        Symbol = sym
                    };
                    sym.Symbols.Add(symRef);
                }
            }
            else
                sym.Symbols = null;
            return (sym);
        }
    }
}
