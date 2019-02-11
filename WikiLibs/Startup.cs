using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WikiLibs
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            List<string> modules = new List<string>();
            Services.ModuleManager mgr = new Services.ModuleManager();
            Configuration.Bind(modules);
            IMvcBuilder builder = services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            foreach (string s in modules)
                mgr.LoadModule(builder, s);
            services.AddScoped<API.IModuleManager>(o =>
            {
                DB.Context ctx = o.GetService<DB.Context>();
                return (new Services.ModuleManager(mgr, ctx));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
