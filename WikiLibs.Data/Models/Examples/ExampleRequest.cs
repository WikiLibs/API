using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace WikiLibs.Data.Models.Examples
{
    public class ExampleRequest : Model
    {
        public long? DataId { get; set; }
        public long? ApplyToId { get; set; }
        public string Message { get; set; }
        public DateTime CreationDate { get; set; }
        public ExampleRequestType Type { get; set; }

        public virtual Example Data { get; set; }
        public virtual Example ApplyTo { get; set; }
    }

    public enum ExampleRequestType
    {
        [EnumMember(Value = "POST")]
        POST = 1,
        [EnumMember(Value = "PATCH")]
        PATCH = 2,
        [EnumMember(Value = "DELETE")]
        DELETE = 3
    }
}
