using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.Shared;
using WikiLibs.Shared.Modules.Admin;
using WikiLibs.Shared.Service;

namespace WikiLibs.API.Admin
{
    [Authorize]
    [Route("/admin/apikey")]
    public class APIKeyController : Controller
    {
        private readonly IUser _user;
        private readonly IAPIKeyManager _manager;

        public APIKeyController(IUser usr, IAdminManager mgr)
        {
            _manager = mgr.APIKeyManager;
            _user = usr;
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(Models.Output.Admin.APIKey))]
        public async Task<IActionResult> PostAsync([FromBody, Required] Models.Input.Admin.APIKeyCreate apikey)
        {
            if (!_user.HasPermission(Permissions.CREATE_APIKEY))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceName = apikey.Description,
                    ResourceType = typeof(Data.Models.APIKey),
                    MissingPermission = Permissions.CREATE_APIKEY
                };
            var mdl = await _manager.PostAsync(apikey.CreateModel());
            return (Json(Models.Output.Admin.APIKey.CreateModel(mdl)));
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(200, Type = typeof(Models.Output.Admin.APIKey))]
        public async Task<IActionResult> PatchAsync([FromRoute] string id, [FromBody, Required] Models.Input.Admin.APIKeyUpdate apikey)
        {
            var mdl = await _manager.GetAsync(id);

            if (!_user.HasPermission(Permissions.UPDATE_APIKEY))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceId = id,
                    ResourceName = mdl.Description,
                    ResourceType = typeof(Data.Models.APIKey),
                    MissingPermission = Permissions.UPDATE_APIKEY
                };
            var newMdl = await _manager.PatchAsync(id, apikey.CreatePatch(mdl));
            return (Json(Models.Output.Admin.APIKey.CreateModel(newMdl)));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] string id)
        {
            if (!_user.HasPermission(Permissions.DELETE_APIKEY))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceId = id,
                    ResourceName = id,
                    ResourceType = typeof(Data.Models.APIKey),
                    MissingPermission = Permissions.DELETE_APIKEY
                };
            await _manager.DeleteAsync(id);
            return (Ok());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Models.Output.Admin.APIKey>))]
        public IActionResult Get()
        {
            if (!_user.HasPermission(Permissions.LIST_APIKEY))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceName = "APIKey",
                    ResourceType = typeof(Data.Models.APIKey)
                };
            return (Json(Models.Output.Admin.APIKey.CreateModels(_manager.GetAll())));
        }
    }
}
