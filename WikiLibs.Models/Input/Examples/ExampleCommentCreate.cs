using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Data.Models.Examples;

namespace WikiLibs.Models.Input.Examples
{
    public class ExampleCommentCreate : PostModel<ExampleCommentCreate, ExampleComment>
    {
        public string Data { get; set; }

        public override ExampleComment CreateModel()
        {
            return (new ExampleComment()
            {
                Data = Data,
                CreationDate = DateTime.UtcNow
            });
        }
    }
}
