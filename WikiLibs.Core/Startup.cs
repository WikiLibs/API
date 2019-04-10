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
using WikiLibs.Core.Services;

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
              .AddJsonOptions(o => o.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore);

            foreach (var s in modules)
                mgr.LoadModule(builder, s);
            Configuration.Bind("Modules", modules);
            services.AddDbContext<Data.Context>(o => o.UseLazyLoadingProxies()
                                                      .UseSqlServer(Configuration.GetConnectionString("Default")));
            services.AddHttpContextAccessor();
            services.AddScoped<API.IModuleManager>(o =>
            {
                Data.Context ctx = o.GetService<Data.Context>();
                return (new ModuleManager(mgr, ctx));
            });
            services.AddScoped<IUser>(o => new StandardUser(o.GetService<IHttpContextAccessor>(), o.GetService<Data.Context>()));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<Middleware.ErrorHandlingMiddleware>();
            if (!env.IsDevelopment())
                app.UseHsts();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
