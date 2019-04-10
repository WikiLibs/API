using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.API
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AuthorizeApiKey : Attribute
    {
    }
}
