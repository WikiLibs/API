using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WikiLibs.Models.Input.Users;
using WikiLibs.Shared;
using WikiLibs.Shared.Attributes;
using WikiLibs.Shared.Modules;
using WikiLibs.Shared.Service;

namespace WikiLibs.API
{
    [Route("/user")]
    public class UserController : FileController
    {
        private readonly IUser _user;
        private readonly IUserManager _ummgr;

        public UserController(IUser usr, IUserManager ummgr)
        {
            _user = usr;
            _ummgr = ummgr;
        }

        [ProducesResponseType(200, Type = typeof(Models.Output.UserGlobal))]
        [Authorize(Policy = AuthPolicy.ApiKey, Roles = AuthorizeApiKey.Standard)]
        [HttpGet("{uid}")]
        public async Task<IActionResult> GetUser([FromRoute] string uid)
        {
            var mdl = await _ummgr.GetAsync(uid);
            var data = Models.Output.UserGlobal.CreateModel(mdl);

            if (data.Private)
            {
                data.FirstName = null;
                data.LastName = null;
                data.Email = null;
            }
            return (Json(data));
        }

        [ProducesResponseType(200, Type = typeof(Models.Output.UserLocal))]
        [Authorize(Policy = AuthPolicy.Bearer)]
        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var mdl = await _ummgr.GetAsync(_user.UserId);
            return (Json(Models.Output.UserLocal.CreateModel(mdl)));
        }

        [HttpDelete("{uid}")]
        [Authorize(Policy = AuthPolicy.Bearer)]
        public async Task<IActionResult> DeleteUser([FromRoute] string uid)
        {
            if (!_user.HasPermission(Permissions.DELETE_USER))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceName = uid,
                    ResourceId = uid,
                    ResourceType = typeof(Data.Models.User),
                    MissingPermission = Permissions.DELETE_USER
                };

            await _ummgr.DeleteAsync(uid);
            return (Ok());
        }

        [HttpDelete("me")]
        [Authorize(Policy = AuthPolicy.Bearer)]
        public async Task<IActionResult> DeleteUser()
        {
            if (!_user.HasPermission(Permissions.DELETE_ME))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceName = _user.UserId,
                    ResourceId = _user.UserId,
                    ResourceType = typeof(Data.Models.User),
                    MissingPermission = Permissions.DELETE_ME
                };

            await _ummgr.DeleteAsync(_user.UserId);
            return (Ok());
        }

        [ProducesResponseType(200, Type = typeof(Models.Output.UserGlobal))]
        [HttpPatch("{uid}")]
        [Authorize(Policy = AuthPolicy.Bearer)]
        public async Task<IActionResult> PatchUser([FromRoute] string uid, [FromBody] UserUpdateGlobal usr)
        {
            if (!_user.HasPermission(Permissions.UPDATE_USER))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceName = uid,
                    ResourceId = uid,
                    ResourceType = typeof(Data.Models.User),
                    MissingPermission = Permissions.UPDATE_USER
                };

            var mdl = await _ummgr.PatchAsync(uid, usr.CreatePatch(await _ummgr.GetAsync(uid)));
            return (Json(Models.Output.UserGlobal.CreateModel(mdl)));
        }

        [ProducesResponseType(200, Type = typeof(Models.Output.UserLocal))]
        [HttpPatch("me")]
        [Authorize(Policy = AuthPolicy.Bearer)]
        public async Task<IActionResult> PatchMe([FromBody] UserUpdate usr)
        {
            if (!_user.HasPermission(Permissions.UPDATE_ME))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceName = _user.UserId,
                    ResourceId = _user.UserId,
                    ResourceType = typeof(Data.Models.User),
                    MissingPermission = Permissions.UPDATE_ME
                };
            if (usr.CurPassword != _user.User.Pass)
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceName = _user.UserId,
                    ResourceId = _user.UserId,
                    ResourceType = typeof(Data.Models.User),
                    MissingPermission = Permissions.UPDATE_ME
                };

            var mdl = await _ummgr.PatchAsync(_user.UserId, usr.CreatePatch(_user.User));
            return (Json(Models.Output.UserLocal.CreateModel(mdl)));
        }

        [Authorize(Policy = AuthPolicy.ApiKey, Roles = AuthorizeApiKey.Standard)]
        [HttpGet("{uid}/icon")]
        [ProducesResponseType(200, Type = typeof(string))]
        public async Task<IActionResult> GetIconGlobal([FromRoute]string uid)
        {
            var mdl = await _ummgr.GetAsync(uid);
            var img = _ummgr.GetFile(mdl);
            return (Json(await img.ToBase64()));
        }

        [HttpGet("me/icon")]
        [Authorize(Policy = AuthPolicy.Bearer)]
        [ProducesResponseType(200, Type = typeof(string))]
        public async Task<IActionResult> GetIconMe()
        {
            var img = _ummgr.GetFile(_user.User);
            return (Json(await img.ToBase64()));
        }

        [HttpPut("{uid}/icon")]
        [Authorize(Policy = AuthPolicy.Bearer)]
        [ProducesResponseType(200, Type = typeof(string))]
        //Parameter name is forced to be meaningless otherwise useless warning
        public async Task<IActionResult> PutIconGlobal([FromRoute]string uid, [FromForm, Required]FormFile seryhk)
        {
            if (!_user.HasPermission(Permissions.UPDATE_USER))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceName = "Icon",
                    ResourceId = uid,
                    ResourceType = typeof(Data.Models.User),
                    MissingPermission = Permissions.UPDATE_USER
                };
            var mdl = await _ummgr.GetAsync(uid);
            await _ummgr.PostFileAsync(mdl, ImageFileFromForm(seryhk));
            return (Ok());
        }

        [HttpPut("me/icon")]
        [Authorize(Policy = AuthPolicy.Bearer)]
        [ProducesResponseType(200, Type = typeof(string))]
        //Parameter name is forced to be meaningless otherwise useless warning
        public async Task<IActionResult> PutIconMe([FromForm, Required]FormFile seryhk)
        {
            if (!_user.HasPermission(Permissions.UPDATE_ME))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceName = "Icon",
                    ResourceId = _user.UserId,
                    ResourceType = typeof(Data.Models.User),
                    MissingPermission = Permissions.UPDATE_ME
                };
            await _ummgr.PostFileAsync(_user.User, ImageFileFromForm(seryhk));
            return (Ok());
        }
    }
}
