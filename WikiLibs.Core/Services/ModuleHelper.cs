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
            info.ModuleInterface = InjectService(services, cfg, moduleClass, info.ModuleInterface);
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

        private static Type InjectService(IServiceCollection services, IConfiguration cfg, Type moduleClass, Type moduleInterface)
        {
            if (moduleInterface == typeof(IModule)) // API module no actual shared code
                return (null);
            foreach (var method in moduleClass.GetMethods())
            {
                if (method.IsStatic && method.IsPublic && method.GetCustomAttribute<Shared.Attributes.ModuleConfigurator>() != null)
                    method.Invoke(null, new object[] { services, cfg });
            }
            services.AddScoped(moduleInterface, moduleClass);
            return (moduleInterface);
        }
    }
}
