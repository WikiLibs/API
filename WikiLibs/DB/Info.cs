using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikiLibs.DB
{
    public enum InfoType
    {
        LANG,
        LIB
    }
    public class Info
    {
        public InfoType Type { get; set; }
        public string Data { get; set; }
    }
}
