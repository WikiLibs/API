using System;

namespace WikiLibs.Data.Models
{
    public class Example : Model
    {
        public virtual Symbol Symbol { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public virtual User User { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModificationDate { get; set; }
    }

    public enum ExampleState
    {
        VALIDATED = 0,
        REQUEST_POST = 1,
        REQUEST_PATCH = 2,
        REQUEST_DELETE = 3
    }
}
