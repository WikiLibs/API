using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WikiLibs.Data.Models;
using WikiLibs.Models.Input;
using WikiLibs.Shared;
using WikiLibs.Shared.Attributes;
using WikiLibs.Shared.Modules;
using WikiLibs.Shared.Modules.Admin;
using WikiLibs.Shared.Service;

namespace WikiLibs.API
{
    [AllowAnonymous]
    [Route("error")]
    public class ErrorController : Controller
    {
        private readonly IErrorManager _errors;
        private readonly IHttpContextAccessor _http;
        private readonly IApiKeyManager _apiKeys;
        private readonly IUser _user;

        public ErrorController(IUser user, IErrorManager errors, IHttpContextAccessor http, IAdminManager admin)
        {
            _errors = errors;
            _http = http;
            _apiKeys = admin.ApiKeyManager;
            _user = user;
        }

        [AuthorizeApiKey(Flag = AuthorizeApiKey.ErrorReport)]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] ErrorCreate error)
        {
            var mdl = error.CreateModel();
            var key = await _apiKeys.GetAsync(_http.HttpContext.Request.Headers["Authorization"]);
            mdl.Description = key.Description;
            await _errors.PostAsync(mdl);
            return Ok();
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Models.Output.Error>))]
        public IActionResult GetErrors()
        {
            if (!_user.HasPermission(Permissions.MANAGE_ERRORS))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceType = typeof(Error),
                    ResourceName = "Errors",
                    ResourceId = "",
                    MissingPermission = Permissions.MANAGE_ERRORS
                };
            return Json(Models.Output.Error.CreateModels(_errors.GetLatestErrors()));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            if (!_user.HasPermission(Permissions.MANAGE_ERRORS))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceType = typeof(Error),
                    ResourceName = "Errors",
                    ResourceId = "",
                    MissingPermission = Permissions.MANAGE_ERRORS
                };
            await _errors.DeleteAsync(id);
            return Ok();
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> CleanupAsync()
        {
            if (!_user.HasPermission(Permissions.MANAGE_ERRORS))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceType = typeof(Error),
                    ResourceName = "Errors",
                    ResourceId = "",
                    MissingPermission = Permissions.MANAGE_ERRORS
                };
            await _errors.CleanupAsync();
            return Ok();
        }
    }
}
