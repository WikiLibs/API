using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace WikiLibs.Controllers
{
    public class ValuesController : Controller
    {
        private readonly ApplicationPartManager _partManager;

        public ValuesController(ApplicationPartManager partManager)
        {
            _partManager = partManager;
        }

        [HttpGet]
        [Route("/debug")]
        public IActionResult ShowAllControllers()
        {
            List<TypeInfo> controllers;
            var controllerFeature = new ControllerFeature();

            _partManager.PopulateFeature(controllerFeature);
            controllers = controllerFeature.Controllers.ToList();
            return (Json(controllers));
        }
    }
}
