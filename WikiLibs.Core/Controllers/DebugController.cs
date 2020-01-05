using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Newtonsoft.Json.Serialization;
using WikiLibs.Shared.Modules.Admin;
using WikiLibs.Shared.Modules.Smtp;
using WikiLibs.Shared.Service;

namespace WikiLibs.Core.Controllers
{
    public class TestEmailModel
    {
        public string Title { get; set; }
    }

    [ApiController]
    public class DebugController : Controller
    {
        private readonly ApplicationPartManager _partManager;
        private readonly IModuleCollection _modules;
        private readonly IAdminManager _admin;
        private readonly ISmtpManager _smtpManager;

        public DebugController(ApplicationPartManager partManager, ISmtpManager smtp, IModuleCollection mgr, IAdminManager admin)
        {
            _partManager = partManager;
            _modules = mgr;
            _admin = admin;
            _smtpManager = smtp;
        }

#if DEBUG
        [HttpGet("/debug/mail")]
        public async Task<IActionResult> SendTestEmail()
        {
            await _smtpManager.SendAsync(new Mail()
            {
                Subject = "This is a test",
                Template = "TestEmail",
                Model = new TestEmailModel()
                {
                    Title = "This is a test message"
                },
                Recipients = new HashSet<Recipient>()
                {
                    new Recipient()
                    {
                        Email = "sldt.yuri6037@gmail.com",
                        Name = "Yuri Edward"
                    }
                }
            });
            return (Ok());
        }
#endif

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
                DevKey = _admin.ApiKeyManager.GetAllOfDescription("[WIKILIBS_SUPER_DEV_API_KEY]").First().Id,
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
