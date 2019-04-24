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
            var mgr = new ModuleManager();

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
                mgr.LoadModule(builder, s);
            mgr.LoadAll(Configuration);
            services.AddDbContext<Data.Context>(o => o.UseLazyLoadingProxies()
                                                      .UseSqlServer(Configuration.GetConnectionString("Default")));
            services.AddHttpContextAccessor();
            services.AddScoped<IModuleManager>(o =>
            {
                var factory = o.GetService<ILoggerFactory>();
                Data.Context ctx = o.GetService<Data.Context>();
                return (new ModuleManager(mgr, ctx, factory));
            });
            services.AddScoped<IUser>(o => new StandardUser(o.GetService<IHttpContextAccessor>(), o.GetService<Data.Context>()));
            services.AddCors(o =>
                o.AddPolicy("AllowAll", p =>
                    p.AllowAnyHeader()
                     .AllowAnyMethod()
                     .AllowAnyOrigin()
                )
            );

            var jwtCfg = new Auth.Config();
            Configuration.Bind("WikiLibs.Auth", jwtCfg);

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(jwtCfg.Internal.TokenSecret)),
                    ValidateIssuer = true,
                    ValidIssuer = jwtCfg.Internal.TokenIssuer,
                    ValidateLifetime = true,
                    ValidateAudience = true,
                    ValidAudience = jwtCfg.Internal.TokenAudiance
                };
            });
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
                var mdMgr = scope.ServiceProvider.GetService<IModuleManager>();
                Data.Context ctx = scope.ServiceProvider.GetService<Data.Context>();
                ((ModuleManager)mdMgr).CallModuleInitializers(factory, ctx, env);
            }
        }
    }
}
