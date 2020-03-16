using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikiLibs.Shared.Attributes;

namespace WikiLibs.Core.Filters
{
    public class AuthorizationSwagger : IOperationFilter
    {
        private List<string> ConvertAPIKeyFlagToName(IEnumerable<int> flags)
        {
            var strs = new List<string>();

            foreach (var i in flags)
            {
                switch (i)
                {
                    case AuthorizeApiKey.Authentication:
                        strs.Add("Authentication");
                        break;
                    case AuthorizeApiKey.Registration:
                        strs.Add("Registration");
                        break;
                    case AuthorizeApiKey.Standard:
                        strs.Add("Standard");
                        break;
                }
            }
            return (strs);
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var authBearer = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>();

            if (!authBearer.Any() && !context.MethodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any())
                authBearer = context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>();

            var authApi = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<AuthorizeApiKey>()
                .Select(attr => attr.Flag)
                .Distinct();

            if (authBearer.Any() || authApi.Any())
            {
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Security = new List<OpenApiSecurityRequirement>();
            }
            if (authBearer.Any())
            {
                OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference()
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                };
                operation.Security.Add(new OpenApiSecurityRequirement
                {
                    {securityScheme, new string[] { }}
                });
            }
            if (authApi.Any())
            {
                OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference()
                    {
                        Id = "APIKey",
                        Type = ReferenceType.SecurityScheme
                    }
                };
                operation.Security.Add(new OpenApiSecurityRequirement
                {
                    {securityScheme, new string[] { }}
                });
           }
        }
    }
}
