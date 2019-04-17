using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WikiLibs.API
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AuthorizeApiKey : Attribute
    {
        public const int Registration = 1;
        public const int Authentication = 2;
        public const int Standard = 4;

        [Required]
        public int Flag { get; set; }
    }
}
