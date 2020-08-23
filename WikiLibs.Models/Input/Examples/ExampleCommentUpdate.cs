using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Data.Models.Examples;

namespace WikiLibs.Models.Input.Examples
{
    public class ExampleCommentUpdate : PatchModel<ExampleCommentUpdate, ExampleComment>
    {
        public string Data { get; set; }

        public override ExampleComment CreatePatch(in ExampleComment current)
        {
            return (new ExampleComment()
            {
                ExampleId = current.ExampleId,
                UserId = current.UserId,
                Data = Data != null ? Data : current.Data,
                CreationDate = current.CreationDate,
                Example = current.Example,
                User = current.User
            });
        }
    }
}
