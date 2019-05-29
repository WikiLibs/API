using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.API
{
    [Authorize]
    [Route("/user")]
    public class UserController : Controller
    {
         private readonly IUser _user;
         private readonly IUserManager _ummgr;

         public UserController(IUser usr) {
             _user = usr
         }

        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(Models.Output.User))]
        [AuthorizeApiKey(Flag = AuthorizeApiKey.Standard)]
        [HttpGet("{uid}")]
        public IActionResult GetUser([FromRoute] string uid)
        {
             var mdl = await _ummgr.GetAsync(uid);
             return (Json(Models.Output.User.CreateModel(mdl)));
        }

        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(Models.Output.User))]
        [AuthorizeApiKey(Flag = AuthorizeApiKey.Standard)]
        [HttpGet]
        public IActionResult GetMe()
        {
             var mdl = await _ummgr.GetAsync(_user.UserId);
             return (Json(Models.Output.User.CreateModel(mdl)));
        }

        [HttpDelete("{uid}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string uid)
        {
            await _ummgr.DeleteAsync(_ummgr.GetAsync(uid));
            return (Ok());
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser()
        {
            await _ummgr.DeleteAsync(_ummgr.GetAsync(_user.UserId));
            return (Ok());
        }

    }
}
