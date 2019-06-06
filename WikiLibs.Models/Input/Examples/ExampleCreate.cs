using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WikiLibs.Data.Models.Examples;

namespace WikiLibs.Models.Input.Examples
{
    public class ExampleCreate : PostModel<ExampleCreate, Example>
    {
        public class CodeLine
        {
            [Required]
            public string Data { get; set; }
            [Required]
            public string Comment { get; set; }
        }

        [Required]
        public long SymbolId { get; set; }
        [Required]
        public CodeLine[] Code { get; set; }
        [Required]
        public string Description { get; set; }

        public override Example CreateModel()
        {
            var ex = new Example()
            {
                CreationDate = DateTime.UtcNow,
                LastModificationDate = DateTime.UtcNow,
                Description = Description,
                SymbolId = SymbolId
            };

            foreach (var line in Code)
            {
                ex.Code.Add(new ExampleCodeLine()
                {
                    Comment = line.Comment,
                    Data = line.Data,
                    Example = ex
                });
            }
            return (ex);
        }
    }
}
