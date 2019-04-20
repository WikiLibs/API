using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WikiLibs.Data.Models
{
    public class APIKey : Model<string>
    {
        public string Description { get; set; }
    }
}
