using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Newtonsoft.Json.Serialization;
using WikiLibs.Shared.Modules.Admin;
using WikiLibs.Shared.Service;

namespace WikiLibs.Core.Controllers
{
    [ApiController]
    public class DebugController : Controller
    {
        private readonly ApplicationPartManager _partManager;
        private readonly IModuleCollection _modules;
        private readonly IAdminManager _admin;

        public DebugController(ApplicationPartManager partManager, IModuleCollection mgr, IAdminManager admin)
        {
            _partManager = partManager;
            _modules = mgr;
            _admin = admin;
        }

        [HttpGet]
        [Route("/debug")]
        public IActionResult ShowAllControllers()
        {
#if !DEBUG
            throw new API.Exceptions.ResourceNotFound()
            {
                ResourceId = "debug",
                ResourceName = "Debug",
                ResourceType = typeof(DebugView)
            };
#endif
            List<TypeInfo> controllers;
            var controllerFeature = new ControllerFeature();

            _partManager.PopulateFeature(controllerFeature);
            controllers = controllerFeature.Controllers.ToList();
            var dbgView = new DebugView()
            {
                Name = "WikiLibs API Server",
                Version = Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                DevKey = _admin.APIKeyManager.GetAllOfDescription("[WIKILIBS_SUPER_DEV_API_KEY]").First().Id,
                Host = new DebugView.HostView()
                {
                    Framework = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
                    Threads = Environment.ProcessorCount
                },
                Controllers = controllers,
                Modules = _modules.AsQueryable().Select(item => new DebugView.ModuleView()
                {
                    Name = item.Name,
                    Version = item.Version
                }).ToList()
            };
            return (Json(dbgView, new Newtonsoft.Json.JsonSerializerSettings()
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));
        }
    }
}
