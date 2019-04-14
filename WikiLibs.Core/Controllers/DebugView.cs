﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WikiLibs.Core.Controllers
{
    public class DebugView
    {
        public class HostView
        {
            public string Framework { get; set; }
            public string CPUName { get; set; }
            public int CPUCount { get; set; }
            public int Cores { get; set; }
            public int Threads { get; set; }
        }

        public string Name { get; set; }
        public string Version { get; set; }
        public HostView Host { get; set; }
        public ICollection<API.ModuleInfo> Modules { get; set; }
        public ICollection<TypeInfo> Controllers { get; set; }
    }
}
