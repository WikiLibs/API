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
    [Route("/admin/group")]
    public class GroupController : Controller
    {
        private readonly IUser _user;
        private readonly IGroupManager _manager;

        public GroupController(IUser usr, IAdminManager mgr)
        {
            _user = usr;
            _manager = mgr.GroupManager;
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(Models.Output.Admin.Group))]
        public async Task<IActionResult> PostAsync([FromBody, Required] Models.Input.Admin.GroupCreate group)
        {
            if (!_user.HasPermission(Permissions.CREATE_GROUP))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceName = group.Name,
                    ResourceType = typeof(Data.Models.Group),
                    MissingPermission = Permissions.CREATE_GROUP
                };
            var mdl = await _manager.PostAsync(group.CreateModel());
            return (Json(Models.Output.Admin.Group.CreateModel(mdl)));
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(200, Type = typeof(Models.Output.Admin.Group))]
        public async Task<IActionResult> PatchAsync([FromRoute] long id, [FromBody, Required] Models.Input.Admin.GroupUpdate group)
        {
            var mdl = await _manager.GetAsync(id);

            if (!_user.HasPermission(Permissions.UPDATE_GROUP))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceId = id.ToString(),
                    ResourceName = mdl.Name,
                    ResourceType = typeof(Data.Models.Group),
                    MissingPermission = Permissions.UPDATE_GROUP
                };
            var newMdl = await _manager.PatchAsync(id, group.CreatePatch(mdl));
            return (Json(Models.Output.Admin.Group.CreateModel(newMdl)));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            if (!_user.HasPermission(Permissions.DELETE_GROUP))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceId = id.ToString(),
                    ResourceName = id.ToString(),
                    ResourceType = typeof(Data.Models.Group),
                    MissingPermission = Permissions.DELETE_GROUP
                };
            await _manager.DeleteAsync(id);
            return (Ok());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Models.Output.Admin.Group>))]
        public IActionResult Get()
        {
            if (!_user.HasPermission(Permissions.LIST_GROUP))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceName = "Group",
                    ResourceType = typeof(Data.Models.Group)
                };
            return (Json(Models.Output.Admin.Group.CreateModels(_manager.GetAll())));
        }
    }
}
