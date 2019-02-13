using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikiLibs.Helpers
{
    public class JWT
    {
        public string Authority { get; set; }
        public string Secret { get; set; }
        //Token lifetime in minutes
        public int Lifetime { get; set; }
    }
}
