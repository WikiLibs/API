using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using WikiLibs.Shared;
using WikiLibs.Shared.Service;

namespace WikiLibs.Core.Services
{
    internal class ModuleInfoInternal : ModuleInfo
    {
        public MethodInfo Initializer { get; set; }
        public Type ModuleInterface { get; set; }
    }

    class ModuleCollection : HashSet<ModuleInfo>, IModuleCollection
    {
    }

    static class ModuleHelper
    {
        private static void InjectModuleConfs(IServiceCollection services, string name, Type cfgClass, IConfiguration cfg)
        {
            object instance = Activator.CreateInstance(cfgClass);

            cfg.Bind(name, instance);
            services.AddSingleton(cfgClass, instance);
        }

        private static void AttemptLocateInitializer(Type mainClass, ref ModuleInfoInternal infos)
        {
            foreach (var m in mainClass.GetMethods())
            {
                if (m.IsStatic && m.GetCustomAttribute<Shared.Attributes.ModuleInitializer>() != null)
                {
                    infos.Initializer = m;
                    return;
                }
            }
        }

        public static ModuleInfoInternal InjectModule(IServiceCollection services, IConfiguration cfg, IMvcBuilder builder, string path)
        {
            FileInfo inf = new FileInfo(Program.AssemblyDirectory + "/" + path + ".dll");
            ModuleInfoInternal info = new ModuleInfoInternal() { Name = path };
            Type cfgClass = null;
            Type moduleClass = null;

            if (!inf.Exists)
                return (null);
            Assembly asm = AssemblyLoadContext.Default.LoadFromAssemblyPath(inf.FullName);
            info.Version = asm.GetName().Version.ToString();
            foreach (Type t in asm.GetExportedTypes())
            {

                if (t.GetCustomAttribute<Shared.Attributes.Module>() != null)
                {
                    if (moduleClass != null)
                        throw new ArgumentException("Duplicate Module Main class");
                    info.ModuleInterface = t.GetCustomAttribute<Shared.Attributes.Module>().Interface;
                    moduleClass = t;
                }
                else if (t.GetCustomAttribute<Shared.Attributes.Configurator>() != null)
                {
                    if (cfgClass != null)
                        throw new ArgumentException("Duplicate Config class");
                    cfgClass = t;
                }
            }
            if (!info.ModuleInterface.IsAssignableFrom(moduleClass))
                throw new ArgumentException("Cannot bind module : incorrect base class");
            builder.AddApplicationPart(asm);
            if (cfgClass != null)
                InjectModuleConfs(services, info.Name, cfgClass, cfg);
            AttemptLocateInitializer(moduleClass, ref info);
            info.ModuleInterface = InjectService(services, moduleClass, info.ModuleInterface);
            return (info);
        }

        public static void AttemptCallModuleInitializer(IHostingEnvironment host, ModuleInfoInternal info, object moduleClass)
        {
            if (info.Initializer != null)
            {
                var attribute = info.Initializer.GetCustomAttribute<Shared.Attributes.ModuleInitializer>();

                if (host.IsDevelopment() && !attribute.Debug)
                    return;
                else if (!attribute.Release && !host.IsDevelopment())
                    return;
                info.Initializer.Invoke(null, new object[] { moduleClass });
            }
        }

        private static Type InjectService(IServiceCollection services, Type moduleClass, Type moduleInterface)
        {
            if (moduleInterface == typeof(IModule)) // API module no actual shared code
                return (null);
            foreach (var method in moduleClass.GetMethods())
            {
                if (method.IsStatic && method.IsPublic && method.GetCustomAttribute<Shared.Attributes.ModuleConfigurator>() != null)
                    method.Invoke(null, new object[] { services });
            }
            services.AddScoped(moduleInterface, moduleClass);
            return (moduleInterface);
        }
    }

    /*[Obsolete]
    public class ModuleManager : IModuleManager
    {
        struct ModuleTypeInfo
        {
            public Type AbstractClass { get; set; }
            public Type MainClass { get; set; }
            public Type ConfigClass { get; set; }
            public MethodInfo Initializer { get; set; }
            public string Name { get; set; }
            public string Version { get; set; }
        }

        private Dictionary<Type, IModule> _moduleMap = new Dictionary<Type, IModule>();
        private Dictionary<Type, object> _configuratorMap = new Dictionary<Type, object>();
        private Dictionary<Type, ModuleTypeInfo> _moduleTypes = new Dictionary<Type, ModuleTypeInfo>();
        private List<ModuleInfo> _moduleList = new List<ModuleInfo>();

        private readonly Data.Context _context;
        private readonly ILoggerFactory _loggerFactory;

        public ModuleManager()
        {
        }

        private object[] GetParameters(ParameterInfo[] paris, Type configClass,
                                       string modName, ILoggerFactory factory,
                                       ModuleManager other, Data.Context ctx)
        {
            List<object> pars = new List<object>();

            foreach (var par in paris)
            {
                if (par.ParameterType == typeof(Data.Context))
                    pars.Add(ctx);
                else if (par.ParameterType == configClass)
                    pars.Add(other._configuratorMap[configClass]);
                else if (par.ParameterType == typeof(IModuleManager))
                    pars.Add(this);
                else if (par.ParameterType == typeof(ILogger))
                    pars.Add(factory.CreateLogger(modName));
            }
            if (pars.Count == 0)
                return (null);
            return (pars.ToArray());
        }

        public ModuleManager(ModuleManager other, Data.Context ctx, ILoggerFactory factory)
        {
            _loggerFactory = factory;
            _context = ctx;
            /*foreach (var kv in other._moduleTypes)
            {
                var pars = GetParameters(kv.Value.MainClass.GetConstructors()[0].GetParameters(), kv.Value.ConfigClass,
                                         kv.Value.Name, factory, other, ctx);

                if (pars == null)
                    _moduleMap[kv.Value.AbstractClass] = (IModule)Activator.CreateInstance(kv.Value.MainClass);
                else
                    _moduleMap[kv.Value.AbstractClass] = (IModule)Activator.CreateInstance(kv.Value.MainClass, pars);
                _moduleList.Add(new ModuleInfo()
                {
                    Name = kv.Value.Name,
                    Version = kv.Value.Version
                });
            }*/
            /*_moduleTypes = other._moduleTypes;
            _moduleList = other._moduleList;
            _configuratorMap = other._configuratorMap;
        }

        private void SetupInstance<T>()
        {
            var module = _moduleTypes[typeof(T)];
            var pars = GetParameters(module.MainClass.GetConstructors()[0].GetParameters(), module.ConfigClass,
                                     module.Name, _loggerFactory, this, _context);

            if (pars == null)
                _moduleMap[module.AbstractClass] = (IModule)Activator.CreateInstance(module.MainClass);
            else
                _moduleMap[module.AbstractClass] = (IModule)Activator.CreateInstance(module.MainClass, pars);
        }

        public T GetModule<T>() where T : IModule
        {
            if (!_moduleMap.ContainsKey(typeof(T)))
            {
                if (_moduleTypes.ContainsKey(typeof(T)))
                    SetupInstance<T>();
                else
                    return (default(T));
            }
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

        private void AttemptLocateInitializer(ref ModuleTypeInfo infos)
        {
            foreach (var m in infos.MainClass.GetMethods())
            {
                if (m.IsStatic && m.GetCustomAttribute<Shared.Attributes.ModuleInitializer>() != null)
                {
                    infos.Initializer = m;
                    return;
                }
            }
        }

        private void CheckModule(ref ModuleTypeInfo infos)
        {
            AttemptLocateInitializer(ref infos);
            if (!infos.AbstractClass.IsAssignableFrom(infos.MainClass))
                throw new ArgumentException("Cannot bind module : incorrect base class");
            if (infos.ConfigClass == null)
                return;
            if (infos.MainClass.GetConstructors().Length != 1)
                throw new ArgumentException("Unable to find module main class constructor");
            var ctx = infos.MainClass.GetConstructors()[0];
            if (ctx.GetParameters().Length <= 0)
                throw new ArgumentException("Configuration class was provided but never used");
        }

        public void LoadModule(IMvcBuilder builder, string path)
        {
            FileInfo inf = new FileInfo(Program.AssemblyDirectory + "/" + path + ".dll");
            ModuleTypeInfo cur = new ModuleTypeInfo() { Name = path };

            if (!inf.Exists)
                return;
            Assembly asm = AssemblyLoadContext.Default.LoadFromAssemblyPath(inf.FullName);
            cur.Version = asm.GetName().Version.ToString();
            foreach (Type t in asm.GetExportedTypes())
            {
                if (t.GetCustomAttribute<Shared.Attributes.Module>() != null)
                {
                    cur.AbstractClass = t.GetCustomAttribute<Shared.Attributes.Module>().RefType;
                    if (cur.MainClass != null)
                        throw new ArgumentException("Duplicate Module Main class");
                    cur.MainClass = t;
                }
                else if (t.GetCustomAttribute<Shared.Attributes.Configurator>() != null)
                {
                    if (cur.ConfigClass != null)
                        throw new ArgumentException("Duplicate Config class");
                    cur.ConfigClass = t;
                }
            }
            CheckModule(ref cur);
            _moduleTypes[cur.AbstractClass] = cur;
            _moduleList.Add(new ModuleInfo()
            {
                Name = cur.Name,
                Version = cur.Version
            });
            builder.AddApplicationPart(asm);
        }

        public void CallModuleInitializers(ILoggerFactory factory, Data.Context ctx, IHostingEnvironment host)
        {
            foreach (var kv in _moduleTypes)
            {
                if (kv.Value.Initializer != null)
                {
                    var attribute = kv.Value.Initializer.GetCustomAttribute<Shared.Attributes.ModuleInitializer>();

                    if (host.IsDevelopment() && !attribute.Debug)
                        continue;
                    else if (!attribute.Release && !host.IsDevelopment())
                        continue;
                    kv.Value.Initializer.Invoke(null, GetParameters(kv.Value.Initializer.GetParameters(),
                                                                    kv.Value.ConfigClass, kv.Value.Name,
                                                                    factory, this, ctx));
                }
            }
        }

        public ICollection<ModuleInfo> GetModules()
        {
            return (_moduleList);
        }
    }*/
}
