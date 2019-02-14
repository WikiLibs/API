using Microsoft.Extensions.Configuration;
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
        private Dictionary<Type, API.IModule> _moduleMap = new Dictionary<Type, API.IModule>();
        private Dictionary<Type, Type> _moduleTypes = new Dictionary<Type, Type>();

        public ModuleManager()
        {
        }

        public ModuleManager(ModuleManager other, DB.Context ctx)
        {
            foreach (KeyValuePair<Type, Type> kv in other._moduleTypes)
            {
                if (kv.Value.GetConstructor(new Type[] { typeof(DB.Context) }) != null)
                    _moduleMap[kv.Key] = (API.IModule)Activator.CreateInstance(kv.Value, new object[] { ctx });
                else
                    _moduleMap[kv.Key] = (API.IModule)Activator.CreateInstance(kv.Value);
            }
        }

        public T GetModule<T>() where T : API.IModule
        {
            if (!_moduleMap.ContainsKey(typeof(T)))
                return (default(T));
            return ((T)_moduleMap[typeof(T)]);
        }

        public void LoadAll(IConfiguration cfg)
        {
            foreach (KeyValuePair<Type, API.IModule> kv in _moduleMap)
                kv.Value.LoadConfig(cfg);
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
                    _moduleTypes[t.GetCustomAttribute<API.Module>().RefType] = t;
                    builder.AddApplicationPart(asm);
                }
            }
        }
    }
}
