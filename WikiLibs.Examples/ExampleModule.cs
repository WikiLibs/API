using System;
using WikiLibs.Shared.Attributes;
using WikiLibs.Shared.Modules.Examples;

namespace WikiLibs.Examples
{
    [Module]
    public class ExampleModule : IExampleModule
    {
        public IExampleManager Manager => throw new NotImplementedException();

        public IExampleRequestManager RequestManager => throw new NotImplementedException();
    }
}
