using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Data.Models.Examples;

namespace WikiLibs.Models.Input.Examples
{
    public class ExampleCommentCreate : PostModel<ExampleCommentCreate, ExampleComment>
    {
        public long ExampleId { get; set; }
        public string Data { get; set; }

        public override ExampleComment CreateModel()
        {
            return (new ExampleComment()
            {
                ExampleId = ExampleId,
                Data = Data,
                CreationDate = DateTime.UtcNow
            });
        }
    }
}
