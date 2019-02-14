﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WikiLibs.Filters
{
    public class APIKeyFilter : IAsyncAuthorizationFilter
    {
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
            string controller = descriptor?.ControllerName;
            TypeInfo ctrl = descriptor?.ControllerTypeInfo;
            string action = descriptor?.ActionName;
            IEnumerable<MethodInfo> methods = ctrl.GetMethods().Where(m => m.Name == action);
            if (methods.Count() <= 0 || methods.ElementAt(0).GetCustomAttribute<API.AuthorizeApiKey>() == null)
                return (Task.FromResult(0));
            if (!context.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                context.Result = new UnauthorizedResult();
                return (Task.FromResult(0));
            }
            var mdmgr = (API.IModuleManager)context.HttpContext.RequestServices
                .GetService(typeof(API.IModuleManager));
            var adminmgr = mdmgr.GetModule<API.Modules.IAdminManager>();
            string auth = context.HttpContext.Request.Headers["Authorization"];
            if (auth == null || auth == "" || !adminmgr.APIKeyExists(auth))
            {
                context.Result = new UnauthorizedResult();
                return (Task.FromResult(0));
            }
            return (Task.FromResult(0));
        }
    }
}
