using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Shared.Modules.Smtp.Models
{
    public class UserReset
    {
        public string UserName { get; set; }
        public string NewPassword { get; set; }

        public const string Template = "UserReset";
    }
}
