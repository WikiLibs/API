using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikiLibs.Core.Middleware
{
    public class ErrorHandlingMiddleware
    {
        class JsonErrorResult
        {
            public string Code { get; set; }
            public string Message { get; set; }
            public string Resource { get; set; }
            public string Route { get; set; }
        }

        private RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext ctx)
        {
            JsonErrorResult res = null;

            try
            {
                await _next(ctx);
            }
            catch (API.Exceptions.InsuficientPermission ex)
            {
                res = new JsonErrorResult()
                {
                    Code = "403:Forbidden",
                    Message = "Missing required permission : '" + ex.MissingPermission + "'",
                    Resource = ex.ResourceType.ToString() + ":" + ex.ResourceName + ":" + ex.ResourceId,
                    Route = ctx.Request.Path
                };
                ctx.Response.Clear();
                ctx.Response.ContentType = "application/json";
                ctx.Response.StatusCode = 403;
                await ctx.Response.WriteAsync(JsonConvert.SerializeObject(res, Formatting.Indented));
            }
            catch (API.Exceptions.InvalidResource ex)
            {
                res = new JsonErrorResult()
                {
                    Code = "400:Invalid",
                    Message = "Invalid property : '" + ex.PropertyName + "'",
                    Resource = ex.ResourceType.ToString() + ":" + ex.ResourceName,
                    Route = ctx.Request.Path
                };
                ctx.Response.Clear();
                ctx.Response.ContentType = "application/json";
                ctx.Response.StatusCode = 400;
                await ctx.Response.WriteAsync(JsonConvert.SerializeObject(res, Formatting.Indented));
            }
            catch (API.Exceptions.ResourceAlreadyExists ex)
            {
                res = new JsonErrorResult()
                {
                    Code = "409:Conflict",
                    Message = "Resource already exists",
                    Resource = ex.ResourceType.ToString() + ":" + ex.ResourceName + ":" + ex.ResourceId,
                    Route = ctx.Request.Path
                };
                ctx.Response.Clear();
                ctx.Response.ContentType = "application/json";
                ctx.Response.StatusCode = 409;
                await ctx.Response.WriteAsync(JsonConvert.SerializeObject(res, Formatting.Indented));
            }
            catch (API.Exceptions.ResourceNotFound ex)
            {
                res = new JsonErrorResult()
                {
                    Code = "404:NotFound",
                    Message = "The specified resource could not be found",
                    Resource = ex.ResourceType.ToString() + ":" + ex.ResourceName + ":" + ex.ResourceId,
                    Route = ctx.Request.Path
                };
                ctx.Response.Clear();
                ctx.Response.ContentType = "application/json";
                ctx.Response.StatusCode = 404;
                await ctx.Response.WriteAsync(JsonConvert.SerializeObject(res, Formatting.Indented));
            }
            catch (Exception ex)
            {
                res = new JsonErrorResult()
                {
                    Code = "500:Internal",
                    Message = ex.GetType().ToString() + ": " + ex.Message,
                    Resource = "An unhandled exception occured while processing your request",
                    Route = ctx.Request.Path
                };
                ctx.Response.Clear();
                ctx.Response.ContentType = "application/json";
                ctx.Response.StatusCode = 500;
                await ctx.Response.WriteAsync(JsonConvert.SerializeObject(res, Formatting.Indented));
            }
        }
    }
}
