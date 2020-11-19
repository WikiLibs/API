using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
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
    [Route("error")]
    public class ErrorController : Controller
    {
        private readonly IErrorManager _errors;
        private readonly IUser _user;

        public ErrorController(IUser user, IErrorManager errors)
        {
            _errors = errors;
            _user = user;
        }

        [Authorize(Policy = AuthPolicy.ApiKey, Roles = AuthorizeApiKey.ErrorReport)]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] ErrorCreate error)
        {
            var desc = User.FindFirst(ClaimTypes.Name)?.Value;
            if (desc == null)
                desc = "Unknwown";
            var mdl = error.CreateModel();
            mdl.Description = desc;
            await _errors.PostAsync(mdl);
            return Ok();
        }

        [Authorize(Policy = AuthPolicy.Bearer)]
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

        [Authorize(Policy = AuthPolicy.Bearer)]
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

        [Authorize(Policy = AuthPolicy.Bearer)]
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
