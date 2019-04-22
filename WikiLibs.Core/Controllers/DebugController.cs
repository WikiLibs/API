using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
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
        private readonly IModuleManager _modules;
        private readonly IAdminManager _admin;

        public DebugController(ApplicationPartManager partManager, IModuleManager mgr)
        {
            _partManager = partManager;
            _modules = mgr;
            _admin = mgr.GetModule<IAdminManager>();
        }

        private string GetCPUId()
        {
            ManagementClass mgt = new ManagementClass("Win32_Processor");
            ManagementObjectCollection procs = mgt.GetInstances();

            if (procs == null)
                return ("Unknown");
            foreach (ManagementObject item in procs)
                return item.Properties["Name"].Value.ToString();
            return ("Unknown");
        }

        private int GetCPUCount()
        {
            int count = 0;

            foreach (var item in new ManagementObjectSearcher("Select * from Win32_ComputerSystem").Get())
                count += int.Parse(item["NumberOfProcessors"].ToString());
            return (count);
        }

        private int GetCores()
        {
            int count = 0;

            foreach (var item in new ManagementObjectSearcher("Select * from Win32_Processor").Get())
                count += int.Parse(item["NumberOfCores"].ToString());
            return (count);
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
                    CPUName = GetCPUId(),
                    CPUCount = GetCPUCount(),
                    Cores = GetCores(),
                    Threads = Environment.ProcessorCount
                },
                Controllers = controllers,
                Modules = _modules.GetModules()
            };
            return (Json(dbgView, new Newtonsoft.Json.JsonSerializerSettings()
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));
        }
    }
}
