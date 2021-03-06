﻿using System.Collections.Generic;
using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using WikiLibs.Core.Auth;
using WikiLibs.Core.Filters;
using WikiLibs.Core.Services;
using WikiLibs.Shared.Attributes;
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
                o.EnableEndpointRouting = false;
                o.ModelMetadataDetailsProviders.Add(new ModelRequiredBinding());
                //o.Filters.Add(new Filters.ApiKeyFilter());
                o.Filters.Add(new Filters.ModelStateFilter());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
              .AddNewtonsoftJson(o =>
              {
                  o.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                  o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
              });
            Configuration.Bind("Modules", modules);
            foreach (var s in modules)
                collection.Add(ModuleHelper.InjectModule(services, Configuration, builder, s));
            var str = Configuration.GetConnectionString("Default");
            if (str.StartsWith("mysql:"))
                services.AddDbContext<Data.Context>(o => o.UseLazyLoadingProxies()
                                                          .UseMySql(str.Substring(str.IndexOf(":") + 1)));
            else
                services.AddDbContext<Data.Context>(o => o.UseLazyLoadingProxies()
                                                          .UseSqlServer(str));
            services.AddHttpContextAccessor();
            services.AddApplicationInsightsTelemetry();
            services.AddScoped<IUser>(o => new StandardUser(o.GetService<IHttpContextAccessor>(), o.GetService<Data.Context>()));
            services.AddCors(o =>
                o.AddPolicy("AllowAll", p =>
                    p.AllowAnyHeader()
                     .AllowAnyMethod()
                     .AllowAnyOrigin()
                )
            );

            services.AddSingleton<IModuleCollection>(collection);

            services.AddAuthentication(ApiKeyAuthentication.SCHEME).AddScheme<AuthenticationSchemeOptions, ApiKeyAuthentication>(ApiKeyAuthentication.SCHEME, null);
            services.AddAuthorization(o =>
            {
                o.AddPolicy(AuthPolicy.Bearer, policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireClaim(ClaimTypes.AuthenticationMethod, "Bearer");
                });
                o.AddPolicy(AuthPolicy.ApiKey, policy =>
                {
                    policy.AuthenticationSchemes.Add(ApiKeyAuthentication.SCHEME);
                    policy.RequireClaim(ClaimTypes.AuthenticationMethod, "APIKey");
                });
                o.AddPolicy(AuthPolicy.BearerOrApiKey, policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.AuthenticationSchemes.Add(ApiKeyAuthentication.SCHEME);
                    policy.RequireClaim(ClaimTypes.AuthenticationMethod, "Bearer", "APIKey");
                });
            });

            services.AddSwaggerGen(c =>
            {
                c.TagActionsBy(api =>
                {
                    var ctrl = api.ActionDescriptor as ControllerActionDescriptor;
                    return (new List<string>() { ctrl.ControllerTypeInfo.Namespace.Replace("WikiLibs.API.", "") + "." + ctrl.ControllerName });
                });
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WikiLibs API", Version = Assembly.GetExecutingAssembly().GetName().Version.ToString() });
                c.CustomSchemaIds(x => x.Assembly.IsDynamic ? "Dynamic." + x.FullName : x.FullName);
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http
                });
                c.AddSecurityDefinition("APIKey", new OpenApiSecurityScheme()
                {
                    Description = "API Key scheme. Example: \"Authorization: {token}\"",
                    Name = "Authorization",
                    Scheme = "",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                c.OperationFilter<AuthorizationSwagger>();
                c.OperationFilter<ErrorMiddlewareSwagger>();
            });
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env, ILoggerFactory factory)
        {
            app.UseMiddleware<Middleware.ErrorHandlingMiddleware>();
            if (!env.IsDevelopment())
                app.UseHsts();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WikiLibs API"));
            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMvc();

            using (var scope = app.ApplicationServices.CreateScope())
            {
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
