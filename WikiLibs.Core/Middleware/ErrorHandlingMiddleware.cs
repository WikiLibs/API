using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikiLibs.Shared.Modules.Auth;

namespace WikiLibs.Core.Middleware
{
    public class ErrorHandlingMiddleware
    {
        public class JsonErrorResult
        {
            public string Code { get; set; }
            public string Message { get; set; }
            public string Resource { get; set; }
            public string Route { get; set; }
        }

        private readonly RequestDelegate _next;
        private readonly TelemetryClient _telemetryClient;

        public ErrorHandlingMiddleware(RequestDelegate next, TelemetryClient client)
        {
            _next = next;
            _telemetryClient = client;
        }

        private string GenObjectString(JsonErrorResult res)
        {
            return (JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));
        }

        private void PutCORSHeaders(HttpContext ctx)
        {
            ctx.Response.Headers["Access-Control-Allow-Origin"] = "*";
        }

        public async Task Invoke(HttpContext ctx)
        {
            JsonErrorResult res = null;

            try
            {
                await _next(ctx);
            }
            catch (InvalidCredentials)
            {
                res = new JsonErrorResult()
                {
                    Code = "401:Unauthorized",
                    Message = "Invalid credentials",
                    Resource = "Not applicable",
                    Route = ctx.Request.Path
                };
                ctx.Response.Clear();
                ctx.Response.ContentType = "application/json";
                ctx.Response.StatusCode = 401;
                PutCORSHeaders(ctx);
                await ctx.Response.WriteAsync(GenObjectString(res));
            }
            catch (Shared.Exceptions.InsuficientPermission ex)
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
                PutCORSHeaders(ctx);
                await ctx.Response.WriteAsync(GenObjectString(res));
            }
            catch (Shared.Exceptions.InvalidResource ex)
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
                PutCORSHeaders(ctx);
                await ctx.Response.WriteAsync(GenObjectString(res));
            }
            catch (Shared.Exceptions.ResourceAlreadyExists ex)
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
                PutCORSHeaders(ctx);
                await ctx.Response.WriteAsync(GenObjectString(res));
            }
            catch (Shared.Exceptions.ResourceNotFound ex)
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
                PutCORSHeaders(ctx);
                await ctx.Response.WriteAsync(GenObjectString(res));
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
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
                PutCORSHeaders(ctx);
                await ctx.Response.WriteAsync(GenObjectString(res));
            }
        }
    }
}
