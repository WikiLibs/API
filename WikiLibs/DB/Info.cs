using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint __useless__ { get; set; } //EF Core forced me to do add this useless attribute
        public EInfoType Type { get; set; }
        public string Data { get; set; }
    }
}
