using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.Shared;
using WikiLibs.Shared.Modules;
using WikiLibs.Shared.Modules.Examples;
using WikiLibs.Shared.Service;

namespace WikiLibs.API.Examples
{
    [Route("example/request")]
    [Authorize]
    public class ExampleRequestController : Controller
    {
        private readonly IUser _user;
        private readonly IExampleRequestManager _manager;
        private readonly ISymbolManager _symbolManager;

        public ExampleRequestController(IUser usr, IExampleModule module, ISymbolManager symbolManager)
        {
            _user = usr;
            _manager = module.RequestManager;
            _symbolManager = symbolManager;
        }

        private void CheckPermissions(Models.Input.Examples.ExampleRequestCreate ex)
        {
            switch (ex.Method)
            {
                case Data.Models.Examples.ExampleRequestType.DELETE:
                    if (!_user.HasPermission(Permissions.CREATE_EXAMPLE_REQUEST_DELETE))
                        throw new Shared.Exceptions.InsuficientPermission()
                        {
                            ResourceName = ex.Message,
                            ResourceType = typeof(Data.Models.Examples.ExampleRequest),
                            MissingPermission = Permissions.CREATE_EXAMPLE_REQUEST_DELETE
                        };
                    break;
                case Data.Models.Examples.ExampleRequestType.PATCH:
                    if (!_user.HasPermission(Permissions.CREATE_EXAMPLE_REQUEST_PATCH))
                        throw new Shared.Exceptions.InsuficientPermission()
                        {
                            ResourceName = ex.Message,
                            ResourceType = typeof(Data.Models.Examples.ExampleRequest),
                            MissingPermission = Permissions.CREATE_EXAMPLE_REQUEST_PATCH
                        };
                    break;
                case Data.Models.Examples.ExampleRequestType.POST:
                    if (!_user.HasPermission(Permissions.CREATE_EXAMPLE_REQUEST_POST))
                        throw new Shared.Exceptions.InsuficientPermission()
                        {
                            ResourceName = ex.Message,
                            ResourceType = typeof(Data.Models.Examples.ExampleRequest),
                            MissingPermission = Permissions.CREATE_EXAMPLE_REQUEST_POST
                        };
                    break;
            }
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(Models.Output.Examples.ExampleRequest))]
        public async Task<IActionResult> PostAsync([FromBody, Required] Models.Input.Examples.ExampleRequestCreate example)
        {
            CheckPermissions(example);
            var ex = example.CreateModel();
            if (ex.Data != null)
            {
                ex.Data.User = _user.User;
                ex.Data.UserId = _user.UserId;
                ex.Data.Symbol = await _symbolManager.GetAsync(ex.Data.SymbolId);
            }
            var mdl = await _manager.PostAsync(ex);
            return (Json(Models.Output.Examples.ExampleRequest.CreateModel(mdl)));
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(200, Type = typeof(Models.Output.Examples.Example))]
        public async Task<IActionResult> PatchAsync([FromRoute] long id, [FromBody, Required] Models.Input.Examples.ExampleRequestUpdate example)
        {
            if (!_user.HasPermission(Permissions.UPDATE_EXAMPLE_REQUEST))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceId = id.ToString(),
                    ResourceName = example.Description,
                    ResourceType = typeof(Data.Models.Examples.Example),
                    MissingPermission = Permissions.UPDATE_EXAMPLE
                };
            var mdl = await _manager.PatchAsync(id, example.CreatePatch(await _manager.GetAsync(id)));
            return (Json(Models.Output.Examples.Example.CreateModel(mdl)));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            if (!_user.HasPermission(Permissions.DELETE_EXAMPLE))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceId = id.ToString(),
                    ResourceName = id.ToString(),
                    ResourceType = typeof(Data.Models.Examples.Example),
                    MissingPermission = Permissions.UPDATE_EXAMPLE
                };
            await _manager.DeleteAsync(id);
            return (Ok());
        }
    }
}
