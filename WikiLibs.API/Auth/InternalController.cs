using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
        [ProducesResponseType(200, Type = typeof(string))]
        public async Task<IActionResult> Login([FromBody, Required] Models.Input.Auth.Login mdl)
        {
            var token = await _internal.LegacyLogin(mdl.Email, mdl.Password);
            return (Json(token));
        }

        class ResetObject
        {
            public string Email { get; set; }
        }

        [AuthorizeApiKey(Flag = AuthorizeApiKey.Registration)]
        [HttpPost("reset")]
        public async Task<IActionResult> Reset([FromBody, Required] ResetObject obj)
        {
            await _internal.LegacyReset(obj.Email);
            return (Ok());
        }

        [AuthorizeApiKey(Flag = AuthorizeApiKey.Registration)]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody, Required] Models.Input.Users.UserCreate mdl)
        {
            await _internal.LegacyRegister(mdl.CreateModel());
            return (Ok());
        }

        public class ConfirmQuery
        {
            public string Code { get; set; }
            public string RedirectOK { get; set; }
            public string RedirectKO { get; set; }
        }

        [HttpGet("confirm")]
        public async Task<IActionResult> Confirm([FromQuery] ConfirmQuery query)
        {
            try
            {
                await _internal.LegacyVerifyEmail(query.Code);
                if (query.RedirectOK != null)
                    return (Redirect(query.RedirectOK));
                else
                    return (Ok());
            }
            catch (InvalidCredentials e)
            {
                if (query.RedirectKO == null)
                    return (Unauthorized());
                string url = query.RedirectKO;
                if (url.Contains("?"))
                    url += "&error=";
                else
                    url += "?error=";
                url += HttpUtility.UrlEncode(e.Message);
                return (Redirect(url));
            }
            catch (Shared.Exceptions.ResourceNotFound e)
            {
                if (query.RedirectKO == null)
                    return (NotFound());
                string url = query.RedirectKO;
                if (url.Contains("?"))
                    url += "&error=";
                else
                    url += "?error=";
                url += HttpUtility.UrlEncode("Resource not found: " + e.ResourceId);
                return (Redirect(url));
            }
        }
    }
}
