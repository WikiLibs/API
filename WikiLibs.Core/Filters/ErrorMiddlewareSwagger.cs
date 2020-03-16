using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikiLibs.Core.Middleware;

namespace WikiLibs.Core.Filters
{
    public class ErrorMiddlewareSwagger : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Responses.Add("403", new OpenApiResponse()
            {
                Description = "Forbidden / Missing permission"
            });
            operation.Responses.Add("400", new OpenApiResponse()
            {
                Description = "Bad request / Invalid argument"
            });
            operation.Responses.Add("409", new OpenApiResponse()
            {
                Description = "Conflict"
            });
            operation.Responses.Add("404", new OpenApiResponse()
            {
                Description = "NotFound"
            });
            operation.Responses.Add("500", new OpenApiResponse()
            {
                Description = "Internal Server Error, please report bug if this occurs"
            });
        }
    }
}
