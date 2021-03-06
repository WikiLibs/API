﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikiLibs.Data.Models;
using WikiLibs.Data.Models.Symbols;
using WikiLibs.Shared.Modules.Symbols;

namespace WikiLibs.Models.Output.Symbols
{
    public class Symbol : GetModel<Symbol, Data.Models.Symbols.Symbol>
    {
        public class Prototype
        {
            public class Exception
            {
                public string Description { get; set; }
                public SymbolReference Ref { get; set; }
            }

            public class Parameter
            {
                [JsonProperty(PropertyName = "prototype")]
                public string Proto { get; set; }
                public string Description { get; set; }
                public SymbolReference Ref { get; set; }
            }

            [JsonProperty(PropertyName = "prototype")]
            public string Proto { get; set; }
            public string Description { get; set; }
            public Parameter[] Parameters { get; set; }
            public List<Exception> Exceptions { get; set; }
        }

        public class LibObject
        {
            public long Id { get; set; }
            public string Name { get; set; }
        }

        public class TypeObject
        {
            public string Name { get; set; }
            public string DisplayName { get; set; }
        }

        public long Id { get; set; }
        public long Views { get; set; }
        public string UserId { get; set; }
        public DateTime LastModificationDate { get; set; }
        public Lang Lang { get; set; }
        public LibObject Lib { get; set; }
        public string Import { get; set; }
        public TypeObject Type { get; set; }
        public string Path { get; set; }
        public Prototype[] Prototypes { get; set; }
        public List<SymbolReference> Symbols { get; set; }

        public override void Map(in Data.Models.Symbols.Symbol model)
        {
            Id = model.Id;
            Views = model.Views;
            UserId = model.UserId;
            LastModificationDate = model.LastModificationDate;
            Lang = Lang.CreateModel(model.Lang);
            Lib = new LibObject()
            {
                Name = model.Lib.Name,
                Id = model.LibId
            };
            Type = new TypeObject()
            {
                Name = model.Type.Name,
                DisplayName = model.Type.DisplayName
            };
            Import = model.Import != null ? model.Import.Name : null;
            Path = model.Path;
            Symbols = new List<SymbolReference>();
            Prototypes = new Prototype[model.Prototypes.Count];
            int i = 0;
            foreach (var proto in model.Prototypes)
            {
                Prototypes[i] = new Prototype()
                {
                    Proto = proto.Data,
                    Description = proto.Description,
                    Parameters = new Prototype.Parameter[proto.Parameters.Count],
                    Exceptions = new List<Prototype.Exception>()
                };
                int j = 0;
                foreach (var param in proto.Parameters)
                {
                    Prototypes[i].Parameters[j] = new Prototype.Parameter()
                    {
                        Description = param.Description,
                        Ref = (param.SymbolRef != null && param.SymbolRef.RefId != null) ? new SymbolReference()
                        {
                            Id = param.SymbolRef.RefId.Value,
                            Path = param.SymbolRef.RefPath,
                            Type = param.SymbolRef.Ref.Type.Name,
                            FirstPrototype = param.SymbolRef.Ref.Prototypes.FirstOrDefault() != null ? param.SymbolRef.Ref.Prototypes.FirstOrDefault().Data : null
                        } : null,
                        Proto = param.Data
                    };
                    ++j;
                }
                foreach (var eref in proto.Exceptions)
                {
                    if (eref.RefId != null)
                        Prototypes[i].Exceptions.Add(new Prototype.Exception()
                        {
                            Description = eref.Description,
                            Ref = new SymbolReference()
                            {
                                Id = eref.RefId.Value,
                                Path = eref.RefPath,
                                Type = eref.Ref.Type.Name,
                                FirstPrototype = eref.Ref.Prototypes.FirstOrDefault() != null ? eref.Ref.Prototypes.FirstOrDefault().Data : null
                            }
                        });
                }
                ++i;
            }
            foreach (var sref in model.Symbols)
            {
                if (sref.RefId != null)
                    Symbols.Add(new SymbolReference()
                    {
                        Id = sref.RefId.Value,
                        Path = sref.RefPath,
                        Type = sref.Ref.Type.Name,
                        FirstPrototype = sref.Ref.Prototypes.FirstOrDefault() != null ? sref.Ref.Prototypes.FirstOrDefault().Data : null
                    });
            }
        }
    }
}
