using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikiLibs.Data.Models.Examples;

namespace WikiLibs.Models.Input.Examples
{
    public class ExampleUpdate : PatchModel<ExampleUpdate, Example>
    {
        public class CodeLine
        {
            public string Data { get; set; }
            public string Comment { get; set; }
        }

        public CodeLine[] Code { get; set; }
        public string Description { get; set; }

        public override Example CreatePatch(in Example current)
        {
            var ex = new Example()
            {
                LastModificationDate = DateTime.UtcNow,
                CreationDate = current.CreationDate,
                Description = Description != null ? Description : current.Description,
                Symbol = current.Symbol,
                Request = current.Request,
                Id = current.Id,
                User = current.User
            };

            if (Code != null)
            {
                for (int i = 0; i != Code.Length; ++i)
                {
                    var code = Code[i];
                    var old = i < current.Code.Count ? current.Code.ElementAt(i) : null;
                    var p = new ExampleCodeLine()
                    {
                        Id = old != null ? old.Id : 0,
                        Data = code.Data != null ? code.Data : old.Data,
                        Comment = code.Comment != null ? code.Comment : old.Comment,
                        Example = ex
                    };
                    ex.Code.Add(p);
                }
            }
            else
                ex.Code = null;
            return (ex);
        }
    }
}
