using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikiLibs.DB
{
    public enum EInfoType
    {
        LANG,
        LIB
    }
    public class Info
    {
        public EInfoType Type { get; set; }
        public string Data { get; set; }
    }
}
