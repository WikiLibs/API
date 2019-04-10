﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Data.Models;

namespace WikiLibs.DTO.Output
{
    public class Symbol : IGetDTO<Data.Models.Symbol>
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

        public string UserId { get; set; }
        public string Date { get; set; }
        public string Lang { get; set; }
        public string Type { get; set; }
        public string Path { get; set; }
        public Prototype[] Prototypes { get; set; }
        public string[] Symbols { get; set; }

        public void Map(in Data.Models.Symbol model)
        {
            UserId = model.User.UUID;
            Date = model.LastModificationDate.ToString();
            Lang = model.Lang;
            Type = model.Type;
            Path = model.Path;
            Symbols = new string[model.Symbols.Count];
            Prototypes = new Prototype[model.Prototypes.Count];
            int i = 0;
            foreach (var proto in model.Prototypes)
            {
                Prototypes[i] = new Prototype()
                {
                    Proto = proto.Data,
                    Description = proto.Description,
                    Parameters = new Prototype.Parameter[proto.Parameters.Count]
                };
                int j = 0;
                foreach (var param in proto.Parameters)
                {
                    Prototypes[i].Parameters[j] = new Prototype.Parameter()
                    {
                        Description = param.Description,
                        Path = param.Path,
                        Proto = param.Data
                    };
                    ++j;
                }
                ++i;
            }
            int k = 0;
            foreach (var sref in model.Symbols)
                Symbols[k++] = sref.Path;
        }
    }
}
