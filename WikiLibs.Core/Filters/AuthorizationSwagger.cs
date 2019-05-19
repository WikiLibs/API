using Microsoft.AspNetCore.Authorization;
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

        public void Apply(Operation operation, OperationFilterContext context)
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
                operation.Responses.Add("401", new Response { Description = "Unauthorized" });
                operation.Security = new List<IDictionary<string, IEnumerable<string>>>();
            }
            if (authBearer.Any())
            {
                operation.Security.Add(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                });
            }
            if (authApi.Any())
            {
                operation.Security.Add(new Dictionary<string, IEnumerable<string>>
                {
                    { "APIKey", ConvertAPIKeyFlagToName(authApi) }
                });
            }
        }
    }
}
