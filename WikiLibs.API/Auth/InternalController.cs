using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.Shared.Attributes;
using WikiLibs.Shared.Modules.Auth;

namespace WikiLibs.API.Auth
{
    [Route("/auth/internal")]
    public class InternalController : Controller
    {
        private readonly IAuthProvider _internal;

        public InternalController(IAuthManager manager)
        {
            _internal = manager.GetAuthenticator("internal");
        }

        [AuthorizeApiKey(Flag = AuthorizeApiKey.Authentication)]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Models.Input.Auth.Login mdl)
        {
            var token = await _internal.LegacyLogin(mdl.Email, mdl.Password);
            return (Json(token));
        }

        [AuthorizeApiKey(Flag = AuthorizeApiKey.Registration)]
        [HttpPost("reset")]
        public async Task<IActionResult> Reset([FromBody] string email)
        {
            await _internal.LegacyReset(email);
            return (Ok());
        }

        [AuthorizeApiKey(Flag = AuthorizeApiKey.Registration)]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Models.Input.UserCreate mdl)
        {
            await _internal.LegacyRegister(mdl.CreateModel());
            return (Ok());
        }

        [HttpGet("confirm/{*code}")]
        public async Task<IActionResult> Confirm([FromRoute] string code)
        {
            await _internal.LegacyVerifyEmail(code);
            return (Ok());
        }
    }
}
