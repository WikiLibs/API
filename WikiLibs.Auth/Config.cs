using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Shared.Attributes;

namespace WikiLibs.Auth
{
    [Configurator]
    public class Config
    {
        public class CInternal
        {
            public string TokenIssuer { get; set; }
            public string TokenAudiance { get; set; }
            public string TokenSecret { get; set; }
            public int TokenLifeMinutes { get; set; }
        }

        public CInternal Internal { get; set; }
        public string DefaultGroupName { get; set; }
    }
}
