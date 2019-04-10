using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.API
{
    public interface IModuleManager
    {
        T GetModule<T>() where T : IModule;
    }
}
