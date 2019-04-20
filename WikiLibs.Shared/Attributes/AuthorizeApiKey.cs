using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WikiLibs.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AuthorizeApiKey : Attribute
    {
        public const int Registration = 0x10;
        public const int Authentication = 0x20;
        public const int Standard = 0x40;

        [Required]
        public int Flag { get; set; }

        public static string GetFlagName(int flag)
        {
            switch (flag)
            {
                case Registration:
                    return "Registration";
                case Authentication:
                    return "Authentication";
                case Standard:
                    return "Standard";
                default:
                    return "Null";
            }
        }
    }
}
