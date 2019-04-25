using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WikiLibs.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Module : Attribute
    {
        [Required]
        public Type Interface { get; set; }

        /*public Module(Type reft)
        {
            RefType = reft;
        }*/
    }
}
