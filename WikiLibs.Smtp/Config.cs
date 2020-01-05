using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Shared.Attributes;

namespace WikiLibs.Smtp
{
    [Configurator]
    public class Config
    {
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public string Prefix { get; set; }
    }
}
