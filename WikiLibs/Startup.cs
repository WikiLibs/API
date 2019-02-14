using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace WikiLibs
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private bool CheckTokenLifeTime(Nullable<DateTime> notBefore, Nullable<DateTime> expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            return (expires < DateTime.UtcNow);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var sqlConfig = new Helpers.SQL();
            var jwtConfig = new Helpers.JWT();

            Configuration.Bind("SQL", sqlConfig);
            Configuration.Bind("JWT", jwtConfig);
            services.AddDbContext<DB.Context>(o =>
            {
                if (sqlConfig.Type == "MYSQL")
                    o.UseMySql("server=" + sqlConfig.Address + ";port=" + sqlConfig.Port
                        + ";uid=" + sqlConfig.User + ";password=" + sqlConfig.Pass
                        + ";database=" + sqlConfig.DBName);
                else if (sqlConfig.Type == "SQLSRV")
                    o.UseSqlServer("server=" + sqlConfig.Address + ";port=" + sqlConfig.Port
                        + ";uid=" + sqlConfig.User + ";password=" + sqlConfig.Pass
                        + ";database=" + sqlConfig.DBName);
            });
            List<string> modules = new List<string>();
            Services.ModuleManager mgr = new Services.ModuleManager();
            Configuration.Bind("Modules", modules);
            IMvcBuilder builder = services.AddMvc(o =>
            {
                o.Filters.Add(new Filters.APIKeyFilter());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            foreach (string s in modules)
                mgr.LoadModule(builder, s);
            services.AddScoped<API.IModuleManager>(o =>
            {
                DB.Context ctx = o.GetService<DB.Context>();
                return (new Services.ModuleManager(mgr, ctx));
            });
            services.AddScoped<Services.ITokenManager>(o =>
            {
                return (new Services.UserTokenManager(jwtConfig));
            });
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(jwtConfig.Secret)),
                    ValidateIssuer = true,
                    ValidIssuer = jwtConfig.Authority,
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    LifetimeValidator = CheckTokenLifeTime
                };
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetService<DB.Context>();
                ctx.Database.EnsureCreated();
                ctx.SaveChanges();
                var mdmgr = scope.ServiceProvider.GetService<API.IModuleManager>();
                mdmgr.LoadAll(Configuration);
            }
        }
    }
}
