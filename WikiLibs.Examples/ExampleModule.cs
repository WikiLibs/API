using System;
using WikiLibs.Shared.Attributes;
using WikiLibs.Shared.Modules.Examples;

namespace WikiLibs.Examples
{
    [Module]
    public class ExampleModule : IExampleModule
    {
        public IExampleManager Manager { get; }

        public IExampleRequestManager RequestManager { get; }

        public ExampleModule(Data.Context ctx)
        {
            Manager = new ExampleManager(ctx);
            RequestManager = new ExampleRequestManager(ctx);
        }
    }
}
