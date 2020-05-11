﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Shared.Modules.Auth
{
    public class InvalidCredentials : Exception
    {
        public InvalidCredentials(string msg)
            : base(msg)
        {
        }

        public InvalidCredentials()
            : base()
        {
        }
    }
}
