using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WikiLibs.Models.Input.Users;
using WikiLibs.Shared;
using WikiLibs.Shared.Attributes;
using WikiLibs.Shared.Modules;
using WikiLibs.Shared.Service;

namespace WikiLibs.API
{
    [Authorize]
    [Route("/user")]
    public class UserController : Controller
    {
        private readonly IUser _user;
        private readonly IUserManager _ummgr;

        public UserController(IUser usr, IUserManager ummgr)
        {
            _user = usr;
            _ummgr = ummgr;
        }

        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(Models.Output.User))]
        [AuthorizeApiKey(Flag = AuthorizeApiKey.Standard)]
        [HttpGet("{uid}")]
        public async Task<IActionResult> GetUser([FromRoute] string uid)
        {
            var mdl = await _ummgr.GetAsync(uid);
            var data = Models.Output.User.CreateModel(mdl);

            if (data.Private)
            {
                data.FirstName = null;
                data.LastName = null;
                data.Email = null;
            }
            return (Json(data));
        }

        [ProducesResponseType(200, Type = typeof(Models.Output.User))]
        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var mdl = await _ummgr.GetAsync(_user.UserId);
            return (Json(Models.Output.User.CreateModel(mdl)));
        }

        [HttpDelete("{uid}")]
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

        [HttpPatch("{uid}")]
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
            return (Json(Models.Output.User.CreateModel(mdl)));
        }

        [HttpPatch("me")]
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
            return (Json(Models.Output.User.CreateModel(mdl)));
        }
    }
}
