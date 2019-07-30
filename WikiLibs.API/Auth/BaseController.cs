using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.Shared.Attributes;
using WikiLibs.Shared.Modules;
using WikiLibs.Shared.Modules.Auth;
using WikiLibs.Shared.Service;

namespace WikiLibs.API.Auth
{
    [Route("/auth")]
    public class BaseController : Controller
    {
        private readonly IUserManager _userManager;
        private readonly IUser _user;
        private readonly IAuthManager _authManager;

        public BaseController(IUserManager userManager, IAuthManager authManager, IUser usr)
        {
            _userManager = userManager;
            _authManager = authManager;
            _user = usr;
        }

        [HttpPost("bot")]
        [AuthorizeApiKey(Flag = AuthorizeApiKey.AuthBot)]
        public async Task<IActionResult> BotLogin([FromBody, Required] Models.Input.Auth.Bot mdl)
        {
            var usr = await _userManager.GetAsync(mdl.AppId);

            if (!usr.IsBot || usr.Pass != mdl.AppSecret)
                throw new InvalidCredentials();
            string token = _authManager.Refresh(usr.Id);
            return (Json(token));
        }

        [HttpPatch("refresh")]
        [Authorize]
        public IActionResult Refresh()
        {
            return (Json(_authManager.Refresh(_user.UserId)));
        }
    }
}
