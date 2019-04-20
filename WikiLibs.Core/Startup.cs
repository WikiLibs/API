using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
            var mgr = new ModuleManager();

            IMvcBuilder builder = services.AddMvc(o =>
            {
                o.ModelMetadataDetailsProviders.Add(new ModelRequiredBinding());
                o.Filters.Add(new Filters.APIKeyFilter());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
              .AddJsonOptions(o =>
              {
                  o.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                  o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
              });
            Configuration.Bind("Modules", modules);
            foreach (var s in modules)
                mgr.LoadModule(builder, s);
            mgr.LoadAll(Configuration);
            services.AddDbContext<Data.Context>(o => o.UseLazyLoadingProxies()
                                                      .UseSqlServer(Configuration.GetConnectionString("Default")));
            services.AddHttpContextAccessor();
            services.AddScoped<IModuleManager>(o =>
            {
                Data.Context ctx = o.GetService<Data.Context>();
                return (new ModuleManager(mgr, ctx));
            });
            services.AddScoped<IUser>(o => new StandardUser(o.GetService<IHttpContextAccessor>(), o.GetService<Data.Context>()));
            services.AddCors(o =>
                o.AddPolicy("AllowAll", p =>
                    p.AllowAnyHeader()
                     .AllowAnyMethod()
                     .AllowAnyOrigin()
                )
            );
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<Middleware.ErrorHandlingMiddleware>();
            if (!env.IsDevelopment())
                app.UseHsts();
            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseMvc();
        }
    }
}
