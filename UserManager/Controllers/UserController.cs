using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserManager.Controllers
{
    public class UserController : Controller
    {
        private API.Modules.IUserManager _umgr;
        private WikiLibs.Services.UserTokenManager _tmgr;

        public UserController(API.IModuleManager mdmgr, WikiLibs.Services.UserTokenManager tmgr)
        {
            _umgr = mdmgr.GetModule<API.Modules.IUserManager>();
            _tmgr = tmgr;
        }

        [API.AuthorizeApiKey]
        [Route("/user/login")]
        [HttpPost]
        public IActionResult Login([FromHeader] string email, [FromHeader] string password)
        {
            API.Entities.User usr = _umgr.GetUser(email, password);

            if (usr == null)
                return (Unauthorized());
            return (Json(_tmgr.GenToken(usr.UUID)));
        }
    }
}
