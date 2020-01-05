using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Shared.Modules.Smtp.Models
{
    public class UserRegistration
    {
        public string UserName { get; set; }
        public string ConfirmCode { get; set; }
        public string Link { get; set; }

        public const string Template = "UserRegistration";
    }
}
