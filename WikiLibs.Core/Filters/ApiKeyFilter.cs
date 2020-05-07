﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WikiLibs.Shared.Attributes;
using WikiLibs.Shared.Modules.Admin;
using WikiLibs.Shared.Service;

namespace WikiLibs.Core.Filters
{
    public class ApiKeyFilter : IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
            string controller = descriptor?.ControllerName;
            TypeInfo ctrl = descriptor?.ControllerTypeInfo;
            string action = descriptor?.ActionName;
            IEnumerable<MethodInfo> methods = ctrl.GetMethods().Where(m => m.Name == action);
            var attribute = methods.Count() > 0 ? methods.ElementAt(0).GetCustomAttribute<AuthorizeApiKey>() : null;
            if (attribute == null)
                return;
            if (!context.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceName = "ApiKey",
                    ResourceId = "ApiKey",
                    MissingPermission = "ApiKey",
                    ResourceType = typeof(Data.Models.ApiKey)
                };
            }
            var adminmgr = (IAdminManager)context.HttpContext.RequestServices
                .GetService(typeof(IAdminManager));
            string auth = context.HttpContext.Request.Headers["Authorization"];
            if (auth == null || auth == "" || !adminmgr.ApiKeyManager.Exists(auth))
            {
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceName = "ApiKey",
                    ResourceId = "ApiKey",
                    MissingPermission = "ApiKey",
                    ResourceType = typeof(Data.Models.ApiKey)
                };
            }
            var addr = context.HttpContext.Connection.RemoteIpAddress;
            var mdl = await adminmgr.ApiKeyManager.GetAsync(auth);
            if (mdl.Origin != null && addr.ToString() != mdl.Origin)
            {
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceName = "ApiKey",
                    ResourceId = "ApiKey",
                    MissingPermission = "ApiKey",
                    ResourceType = typeof(Data.Models.ApiKey)
                };
            }
            if ((mdl.Flags & attribute.Flag) == 0)
            {
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceName = "ApiKey",
                    ResourceId = "ApiKey",
                    MissingPermission = "ApiKey." + AuthorizeApiKey.GetFlagName(attribute.Flag),
                    ResourceType = typeof(Data.Models.ApiKey)
                };
            }
            await adminmgr.ApiKeyManager.Use(auth);
        }
    }
}