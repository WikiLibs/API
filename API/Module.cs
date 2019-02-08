using System;
using System.Collections.Generic;
using System.Text;

namespace API
{
    public class Module : Attribute
    {
        public Type RefType;

        public Module(Type reft)
        {
            RefType = reft;
        }
    }
}
