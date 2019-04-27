using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using WikiLibs.Core.Services;
using WikiLibs.Shared.Service;

namespace WikiLibs.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var modules = new List<string>();
            var collection = new ModuleCollection();

            IMvcBuilder builder = services.AddMvc(o =>
            {
                o.ModelMetadataDetailsProviders.Add(new ModelRequiredBinding());
                o.Filters.Add(new Filters.APIKeyFilter());
                o.Filters.Add(new Filters.ModelStateFilter());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
              .AddJsonOptions(o =>
              {
                  o.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                  o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
              });
            Configuration.Bind("Modules", modules);
            foreach (var s in modules)
                collection.Add(ModuleHelper.InjectModule(services, Configuration, builder, s));
            services.AddDbContext<Data.Context>(o => o.UseLazyLoadingProxies()
                                                      .UseSqlServer(Configuration.GetConnectionString("Default")));
            services.AddHttpContextAccessor();
            services.AddScoped<IUser>(o => new StandardUser(o.GetService<IHttpContextAccessor>(), o.GetService<Data.Context>()));
            services.AddCors(o =>
                o.AddPolicy("AllowAll", p =>
                    p.AllowAnyHeader()
                     .AllowAnyMethod()
                     .AllowAnyOrigin()
                )
            );

            services.AddSingleton<IModuleCollection>(collection);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory factory)
        {
            app.UseMiddleware<Middleware.ErrorHandlingMiddleware>();
            if (!env.IsDevelopment())
                app.UseHsts();
            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseMvc();

            app.UseAuthentication();

            using (var scope = app.ApplicationServices.CreateScope())
            {
                /*var mdMgr = scope.ServiceProvider.GetService<IModuleManager>();
                Data.Context ctx = scope.ServiceProvider.GetService<Data.Context>();
                ((ModuleManager)mdMgr).CallModuleInitializers(factory, ctx, env);*/
                var collection = scope.ServiceProvider.GetService<IModuleCollection>();
                foreach (var module in collection)
                {
                    var info = (ModuleInfoInternal)module;
                    if (info.Initializer == null || info.ModuleInterface == null)
                        continue;
                    var mdInstance = scope.ServiceProvider.GetService(info.ModuleInterface);
                    ModuleHelper.AttemptCallModuleInitializer(env, info, mdInstance);
                }
            }
        }
    }
}
