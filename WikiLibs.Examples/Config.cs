using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Shared.Attributes;

namespace WikiLibs.Examples
{
    [Configurator]
    public class Config
    {
        public int MaxExampleRequestsPerPage { get; set; }
    }
}
