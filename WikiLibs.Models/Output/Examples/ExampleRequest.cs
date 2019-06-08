using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Data.Models.Examples;

namespace WikiLibs.Models.Output.Examples
{
    public class ExampleRequest : GetModel<ExampleRequest, Data.Models.Examples.ExampleRequest>
    {
        public long Id { get; set; }
        public Example Data { get; set; }
        public long? ApplyToId { get; set; }
        public string Message { get; set; }
        public DateTime CreationDate { get; set; }
        public ExampleRequestType Type { get; set; }

        public override void Map(in Data.Models.Examples.ExampleRequest model)
        {
            Id = model.Id;
            Data = model.Data != null ? Example.CreateModel(model.Data) : null;
            ApplyToId = model.ApplyToId;
            Message = model.Message;
            CreationDate = model.CreationDate;
            Type = model.Type;
        }
    }
}
