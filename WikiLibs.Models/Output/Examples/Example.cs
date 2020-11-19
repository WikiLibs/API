using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Data.Models.Examples;

namespace WikiLibs.Models.Output.Examples
{
    public class Example : GetModel<Example, Data.Models.Examples.Example>
    {
        public class ExampleCodeLine
        {
            public string Comment { get; set; }
            public string Data { get; set; }
        }

        public long Id { get; set; }
        public long SymbolId { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public ExampleCodeLine[] Code { get; set; }
        public int VoteCount { get; set; }
        public bool? HasVoted { get; set; }

        public override void Map(in Data.Models.Examples.Example model)
        {
            Id = model.Id;
            SymbolId = model.SymbolId;
            UserId = model.UserId;
            Description = model.Description;
            CreationDate = model.CreationDate;
            Code = new ExampleCodeLine[model.Code.Count];
            int x = 0;
            foreach (var ex in model.Code)
                Code[x++] = new ExampleCodeLine()
                {
                    Comment = ex.Comment,
                    Data = ex.Data
                };
        }
    }
}
