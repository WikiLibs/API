using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Shared.Attributes
{
    public class AuthPolicy
    {
        public const string Bearer = "Bearer";
        public const string ApiKey = "ApiKey";
        public const string BearerOrApiKey = "BearerOrApiKey";
    }
}
