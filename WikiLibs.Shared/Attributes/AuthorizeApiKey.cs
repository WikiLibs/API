using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace WikiLibs.Shared.Attributes
{
    [ExcludeFromCodeCoverage]
    //[AttributeUsage(AttributeTargets.Method)]
    public class AuthorizeApiKey/* : Attribute*/
    {
        public const string ErrorReport = "ErrorReport";
        public const string Registration = "Registration";
        public const string Authentication = "Authentication";
        public const string Standard = "Standard";
        public const string AuthBot = "BotAuthentication";
        public const string SelfDestruct = "SelfDestruct";

        public const int FlagErrorReport = 0x1;
        public const int FlagRegistration = 0x10;
        public const int FlagAuthentication = 0x20;
        public const int FlagStandard = 0x40;
        public const int FlagAuthBot = 0x80;
        public const int FlagSelfDestruct = 0x100;

        //[Required]
        //public int Flag { get; set; }

        public static string GetFlagName(int flag)
        {
            switch (flag)
            {
                case FlagErrorReport:
                    return ErrorReport;
                case FlagRegistration:
                    return Registration;
                case FlagAuthentication:
                    return Authentication;
                case FlagStandard:
                    return Standard;
                case FlagAuthBot:
                    return AuthBot;
                case FlagSelfDestruct:
                    return SelfDestruct;
                default:
                    return "Null";
            }
        }
    }
}
