using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Shared.Attributes;

namespace WikiLibs.Smtp
{
    [Configurator]
    public class Config
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string From { get; set; }
    }
}
