using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ModuleInitializer : Attribute
    {
        public bool Debug { get; set; } = false;
        public bool Release { get; set; } = false;
    }
}
