using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace WikiLibs.Shared.Attributes
{
    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Method)]
    public class AuthorizeApiKey : Attribute
    {
        public const int ErrorReport = 0x1;
        public const int Registration = 0x10;
        public const int Authentication = 0x20;
        public const int Standard = 0x40;
        public const int AuthBot = 0x80;
        public const int SelfDestruct = 0x100;

        [Required]
        public int Flag { get; set; }

        public static string GetFlagName(int flag)
        {
            switch (flag)
            {
                case ErrorReport:
                    return "ErrorReport";
                case Registration:
                    return "Registration";
                case Authentication:
                    return "Authentication";
                case Standard:
                    return "Standard";
                case AuthBot:
                    return "BotAuthentication";
                default:
                    return "Null";
            }
        }
    }
}
