using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WikiLibs.Data.Models
{
    public enum EInfoType
    {
        LANG,
        LIB
    }
    public class Info : Model
    {
        public EInfoType Type { get; set; }
        public string Data { get; set; }
    }
}
