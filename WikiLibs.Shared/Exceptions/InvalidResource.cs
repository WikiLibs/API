using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Shared.Exceptions
{
    public class InvalidResource : Exception
    {
        public Type ResourceType { get; set; }
        public string ResourceName { get; set; }
        public string PropertyName { get; set; }
    }
}
