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
        public void Apply(Operation operation, OperationFilterContext context)
        {
            operation.Responses.Add("403", new Response()
            {
                Description = "Forbidden / Missing permission",
                Schema = new Schema()
                {
                    Default = new ErrorHandlingMiddleware.JsonErrorResult()
                }
            });
            operation.Responses.Add("400", new Response()
            {
                Description = "Bad request / Invalid argument",
                Schema = new Schema()
                {
                    Default = new ErrorHandlingMiddleware.JsonErrorResult()
                }
            });
            operation.Responses.Add("409", new Response()
            {
                Description = "Conflict",
                Schema = new Schema()
                {
                    Default = new ErrorHandlingMiddleware.JsonErrorResult()
                }
            });
            operation.Responses.Add("404", new Response()
            {
                Description = "NotFound",
                Schema = new Schema()
                {
                    Default = new ErrorHandlingMiddleware.JsonErrorResult()
                }
            });
            operation.Responses.Add("500", new Response()
            {
                Description = "Internal Server Error, please report bug if this occurs",
                Schema = new Schema()
                {
                    Default = new ErrorHandlingMiddleware.JsonErrorResult()
                }
            });
        }
    }
}
