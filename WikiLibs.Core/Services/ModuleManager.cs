using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace WikiLibs.Core.Services
{
    public class ModuleManager : API.IModuleManager
    {
        struct ModuleTypeInfo
        {
            public Type MainClass { get; set; }
            public Type ConfigClass { get; set; }
            public string Name { get; set; }
        }

        private Dictionary<Type, API.IModule> _moduleMap = new Dictionary<Type, API.IModule>();
        private Dictionary<Type, object> _configuratorMap = new Dictionary<Type, object>();
        private Dictionary<Type, ModuleTypeInfo> _moduleTypes = new Dictionary<Type, ModuleTypeInfo>();

        public ModuleManager()
        {
        }

        public ModuleManager(ModuleManager other, Data.Context ctx)
        {
            foreach (var kv in other._moduleTypes)
            {
                if (kv.Value.MainClass.GetConstructor(new Type[] { typeof(Data.Context) }) != null)
                {
                    if (kv.Value.ConfigClass != null)
                        _moduleMap[kv.Key] = (API.IModule)Activator.CreateInstance(kv.Value.MainClass,
                            new object[] { ctx, other._configuratorMap[kv.Value.ConfigClass] });
                    else
                        _moduleMap[kv.Key] = (API.IModule)Activator.CreateInstance(kv.Value.MainClass,
                            new object[] { ctx });
                }
                else
                {
                    if (kv.Value.ConfigClass != null)
                        _moduleMap[kv.Key] = (API.IModule)Activator.CreateInstance(kv.Value.MainClass,
                            new object[] { other._configuratorMap[kv.Value.ConfigClass] });
                    else
                        _moduleMap[kv.Key] = (API.IModule)Activator.CreateInstance(kv.Value.MainClass);
                }
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
            foreach (var kv in _moduleTypes)
            {
                if (kv.Value.ConfigClass != null)
                {
                    object cfgcl = Activator.CreateInstance(kv.Value.ConfigClass);
                    cfg.Bind(kv.Value.Name, cfgcl);
                    _configuratorMap[kv.Value.ConfigClass] = cfgcl;
                }
            }
        }

        private void CheckModule(ModuleTypeInfo infos)
        {
            if (infos.ConfigClass == null)
                return;
            if (infos.MainClass.GetConstructors().Length != 1)
                throw new ArgumentException("Unable to find module main class constructor");
            var ctx = infos.MainClass.GetConstructors()[0];
            if (ctx.GetParameters().Length <= 0)
                throw new ArgumentException("Configuration class was provided but never used");
            if (ctx.GetParameters()[ctx.GetParameters().Length - 1].ParameterType != infos.ConfigClass)
                throw new ArgumentException("Configuration class must be last argument of module constructor");
        }

        public void LoadModule(IMvcBuilder builder, string path)
        {
            FileInfo inf = new FileInfo("./Modules/" + path + ".dll");
            ModuleTypeInfo cur = new ModuleTypeInfo() { Name = path };

            if (!inf.Exists)
                return;
            Assembly asm = AssemblyLoadContext.Default.LoadFromAssemblyPath(inf.FullName);
            foreach (Type t in asm.GetExportedTypes())
            {
                if (t.GetCustomAttribute<API.Module>() != null)
                {
                    if (cur.MainClass != null)
                        throw new ArgumentException("Duplicate Module Main class");
                    cur.MainClass = t;
                }
                else if (t.GetCustomAttribute<API.Configurator>() != null)
                {
                    if (cur.ConfigClass != null)
                        throw new ArgumentException("Duplicate Config class");
                    cur.ConfigClass = t;
                }
            }
            CheckModule(cur);
            _moduleTypes[cur.MainClass] = cur;
            builder.AddApplicationPart(asm);
            Console.WriteLine("Adding module '" + path + "'...");
        }
    }
}
