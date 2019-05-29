using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WikiLibs.Shared;
using WikiLibs.Shared.Attributes;
using WikiLibs.Shared.Modules;
using WikiLibs.Shared.Service;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WikiLibs.API
{
    [Authorize]
    [Route("/user")]
    public class UserController : Controller
    {
         private readonly IUser _user;
         private readonly IUserManager _ummgr;

         public UserController(IUser usr, IUserManager ummgr) {
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
             return (Json(Models.Output.User.CreateModel(mdl)));
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

        [HttpPatch]
        public IActionResult PatchUser()
        {
            return (Ok());
        }

        [HttpPatch("me")]
        public IActionResult PatchMe()
        {
            return (Ok());
        }

    }
}
