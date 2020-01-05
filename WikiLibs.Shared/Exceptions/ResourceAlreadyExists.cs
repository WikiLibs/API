using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Shared.Exceptions
{
    public class ResourceAlreadyExists : Exception
    {
        public Type ResourceType { get; set; }
        public string ResourceId { get; set; }
        public string ResourceName { get; set; }
    }
}
