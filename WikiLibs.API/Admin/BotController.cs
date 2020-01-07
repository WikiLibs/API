using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.Shared;
using WikiLibs.Shared.Helpers;
using WikiLibs.Shared.Modules;
using WikiLibs.Shared.Modules.Admin;
using WikiLibs.Shared.Service;

namespace WikiLibs.API.Admin
{
    [Route("/bot")]
    [Authorize]
    public class BotController : FileController
    {
        private readonly IUserManager _userManager;
        private readonly IUser _user;
        private readonly IAdminManager _adminManager;

        public BotController(IUserManager userManager, IAdminManager adminManager, IUser usr)
        {
            _userManager = userManager;
            _adminManager = adminManager;
            _user = usr;
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(Models.Output.Bot))]
        public async Task<IActionResult> PostAsync([FromBody, Required] Models.Input.Admin.BotCreate mdl)
        {
            if (!_user.HasPermission(Permissions.CREATE_BOT))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceName = "Bot",
                    ResourceType = typeof(Data.Models.User),
                    MissingPermission = Permissions.CREATE_BOT
                };
            var created = mdl.CreateModel();
            created.Pass = PasswordUtils.NewPassword(PasswordOptions.Reinforced);
            created.Group = _adminManager.GroupManager.DefaultGroup;
            var obj = await _userManager.PostAsync(created);
            return (Json(Models.Output.Bot.CreateModel(obj)));
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(200, Type = typeof(Models.Output.Bot))]
        public async Task<IActionResult> PatchAsync([FromRoute] string id, [FromBody, Required] Models.Input.Admin.BotUpdate mdl)
        {
            var usr = await _userManager.GetAsync(id);
            if (!usr.IsBot || !_user.HasPermission(Permissions.UPDATE_BOT))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceName = "Bot",
                    ResourceType = typeof(Data.Models.User),
                    MissingPermission = Permissions.UPDATE_BOT
                };
            var created = mdl.CreatePatch(usr);
            created.Pass = PasswordUtils.NewPassword(PasswordOptions.Reinforced);
            var obj = await _userManager.PatchAsync(id, created);
            return (Json(Models.Output.Bot.CreateModel(obj)));
        }

        [HttpPut("{id}/icon")]
        [ProducesResponseType(200, Type = typeof(string))]
        //Parameter name is forced to be meaningless otherwise useless warning
        public async Task<IActionResult> PutIcon([FromRoute] string id, [FromForm, Required]FormFile seryhk)
        {
            var mdl = await _userManager.GetAsync(id);
            if (!mdl.IsBot || !_user.HasPermission(Permissions.UPDATE_BOT))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceName = "BotIcon",
                    ResourceId = id,
                    ResourceType = typeof(Data.Models.User),
                    MissingPermission = Permissions.UPDATE_BOT
                };
            await _userManager.PostFileAsync(mdl, ImageFileFromForm(seryhk));
            return (Ok());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] string id)
        {
            var mdl = await _userManager.GetAsync(id);
            if (!mdl.IsBot || !_user.HasPermission(Permissions.DELETE_BOT))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceName = "Bot",
                    ResourceType = typeof(Data.Models.User),
                    MissingPermission = Permissions.DELETE_BOT
                };
            await _userManager.DeleteAsync(id);
            return (Ok());
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Models.Output.User>))]
        public IActionResult Get()
        {
            if (!_user.HasPermission(Permissions.LIST_BOT))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceName = "Bot",
                    ResourceType = typeof(Data.Models.User),
                    MissingPermission = Permissions.LIST_BOT
                };
            return (Json(Models.Output.User.CreateModels(_userManager.Get(e => e.IsBot))));
        }
    }
}
