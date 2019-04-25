using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace WikiLibs.Shared.Modules
{
    public class ModuleLoader<ModuleInterface, ModuleClass>
        where ModuleClass : ModuleLoader<ModuleInterface, ModuleClass>, ModuleInterface
        where ModuleInterface : class, IModule
    {
        public static Type InjectService(IServiceCollection services)
        {
            if (typeof(ModuleInterface) == typeof(IModule)) // API module no actual shared code
                return (null);
            foreach (var method in typeof(ModuleClass).GetMethods())
            {
                if (method.IsStatic && method.IsPublic && method.GetCustomAttribute<Attributes.ModuleConfigurator>() != null)
                    method.Invoke(null, new object[] { services });
            }
            services.AddScoped<ModuleInterface, ModuleClass>();
            return (typeof(ModuleInterface));
        }
    }
}
