using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Models.Output.Examples
{
    public class ExampleComment : GetModel<ExampleComment, Data.Models.Examples.ExampleComment>
    {
        public long Id { get; set; }
        public long ExampleId { get; set; }
        public string UserId { get; set; }
        public string Data { get; set; }
        public DateTime CreationDate { get; set; }

        public override void Map(in Data.Models.Examples.ExampleComment model)
        {
            Id = model.Id;
            ExampleId = model.ExampleId;
            UserId = model.UserId;
            Data = model.Data;
            CreationDate = model.CreationDate;
        }
    }
}
