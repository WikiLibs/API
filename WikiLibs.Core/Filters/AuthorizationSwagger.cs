﻿using Microsoft.AspNetCore.Authorization;
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
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var attrs = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>()
                .Concat(context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>());

            if (attrs.Any())
            {
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Security = new List<OpenApiSecurityRequirement>();
            }
            foreach (var a in attrs)
            {
                if (a.Policy == AuthPolicy.Bearer)
                {
                    //Handle bearer
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
                else if (a.Policy == AuthPolicy.ApiKey)
                {
                    //Handle api key
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
                else if (a.Policy == AuthPolicy.BearerOrApiKey)
                {
                    //Handle api key or bearer
                    OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference()
                        {
                            Id = "APIKey",
                            Type = ReferenceType.SecurityScheme
                        }
                    };
                    OpenApiSecurityScheme securityScheme1 = new OpenApiSecurityScheme()
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
                    operation.Security.Add(new OpenApiSecurityRequirement
                    {
                        {securityScheme1, new string[] { }}
                    });
                }
            }
        }
    }
}
