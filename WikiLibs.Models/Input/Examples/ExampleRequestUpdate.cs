using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Data.Models.Examples;

namespace WikiLibs.Models.Input.Examples
{
    public class ExampleRequestUpdate : PatchModel<ExampleRequestUpdate, ExampleRequest>
    {
        public string Message { get; set; }
        public ExampleUpdate Data { get; set; }

        public override ExampleRequest CreatePatch(in ExampleRequest current)
        {
            return (new ExampleRequest()
            {
                ApplyTo = current.ApplyTo,
                ApplyToId = current.ApplyToId,
                CreationDate = current.CreationDate,
                DataId = null,
                Data = current.Data == null ? null : (Data != null ? Data.CreatePatch(current.Data) : new ExampleUpdate().CreatePatch(current.Data)),
                Id = current.Id,
                Message = Message != null ? Message : current.Message
            });
        }
    }
}
