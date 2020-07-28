using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Data.Models.Examples
{
    public class ExampleComment : Model
    {
        public long ExampleId { get; set; }
        public string UserId { get; set; }
        public string Data { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual Example Example { get; set; }
        public virtual User User { get; set; }
    }
}
