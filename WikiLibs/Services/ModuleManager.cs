using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WikiLibs.Services
{
    public class ModuleManager : API.IModuleManager
    {
        private Dictionary<Type, API.IModule> _moduleMap;

        public ModuleManager()
        {
        }

        public ModuleManager(ModuleManager other)
        {
            _moduleMap = other._moduleMap;
        }

        public T GetModule<T>() where T : API.IModule
        {
            if (!_moduleMap.ContainsKey(typeof(T)))
                return (default(T));
            return ((T)_moduleMap[typeof(T)]);
        }

        public void LoadModule(IMvcBuilder builder, string path)
        {
            FileInfo inf = new FileInfo("./Modules/" + path + ".dll");

            if (!inf.Exists)
                return;
            Assembly asm = Assembly.LoadFile(inf.FullName);
            foreach (Type t in asm.GetExportedTypes())
            {
                if (t.GetCustomAttribute<API.Module>() != null)
                {
                    _moduleMap[t.GetCustomAttribute<API.Module>().RefType] = (API.IModule)Activator.CreateInstance(t);
                    builder.AddApplicationPart(asm);
                }
            }
        }
    }
}
