using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WikiLibs.Data.Models.Examples;

namespace WikiLibs.Models.Input.Examples
{
    public class ExampleRequestCreate : PostModel<ExampleRequestCreate, ExampleRequest>
    {
        [Required]
        public string Message { get; set; }
        [Required]
        public ExampleRequestType Method { get; set; }
        public ExampleCreate Data { get; set; }
        public long? ApplyTo { get; set; }

        public override ExampleRequest CreateModel()
        {
            return (new ExampleRequest()
            {
                ApplyToId = ApplyTo,
                CreationDate = DateTime.UtcNow,
                Data = Data.CreateModel(),
                Message = Message,
                Type = Method
            });
        }
    }
}
