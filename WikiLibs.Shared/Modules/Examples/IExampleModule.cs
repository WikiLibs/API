using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Shared.Modules.Examples
{
    public interface IExampleModule : IModule
    {
        IExampleManager Manager { get; }
        IExampleRequestManager RequestManager { get; }
    }
}
