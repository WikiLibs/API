using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Module : Attribute
    {
        public Type RefType;

        public Module(Type reft)
        {
            RefType = reft;
        }
    }
}
