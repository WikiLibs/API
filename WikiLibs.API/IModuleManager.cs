using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.API
{
    public class ModuleInfo
    {
        public string Name { get; set; }
        public string Version { get; set; }
    }

    public interface IModuleManager
    {
        T GetModule<T>() where T : IModule;
        ICollection<ModuleInfo> GetModules();
    }
}
