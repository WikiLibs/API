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
                public string prototype { get; set; }
                public string description { get; set; }
                public string path { get; set; }
            }

            public string prototype { get; set; }
            public string description { get; set; }
            public Parameter[] parameters { get; set; }
        }

        public string userId { get; set; }
        public string date { get; set; }
        public string lang { get; set; }
        public string type { get; set; }
        public string path { get; set; }
        public Prototype[] prototypes { get; set; }
        public string[] symbols { get; set; }

        public void Map(in Data.Models.Symbol model)
        {
            userId = model.User.UUID;
            date = model.LastModificationDate.ToString();
            lang = model.Lang;
            type = model.Type;
            path = model.Path;
            symbols = new string[model.Symbols.Count];
            prototypes = new Prototype[model.Prototypes.Count];
            int i = 0;
            foreach (var proto in model.Prototypes)
            {
                prototypes[i] = new Prototype()
                {
                    prototype = proto.Data,
                    description = proto.Description,
                    parameters = new Prototype.Parameter[proto.Parameters.Count]
                };
                int j = 0;
                foreach (var param in proto.Parameters)
                {
                    prototypes[i].parameters[j] = new Prototype.Parameter()
                    {
                        description = param.Description,
                        path = param.Path,
                        prototype = param.Data
                    };
                    ++j;
                }
                ++i;
            }
            int k = 0;
            foreach (var sref in model.Symbols)
                symbols[k++] = sref.Path;
        }
    }
}
