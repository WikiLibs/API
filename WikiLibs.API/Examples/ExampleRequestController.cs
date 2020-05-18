using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.Shared;
using WikiLibs.Shared.Helpers;
using WikiLibs.Shared.Modules.Symbols;
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

        public class ExampleRequestQuery
        {
            public long? SymbolId { get; set; }
            public PageOptions PageOptions { get; set; }
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
        [ProducesResponseType(200, Type = typeof(Models.Output.Examples.ExampleRequest))]
        public async Task<IActionResult> PatchAsync([FromRoute] long id, [FromBody, Required] Models.Input.Examples.ExampleRequestUpdate example)
        {
            var mdl = await _manager.GetAsync(id);

            if ((mdl.Data != null && mdl.Data.UserId != _user.UserId) && !_user.HasPermission(Permissions.UPDATE_EXAMPLE_REQUEST))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceId = id.ToString(),
                    ResourceName = example.Message,
                    ResourceType = typeof(Data.Models.Examples.ExampleRequest),
                    MissingPermission = Permissions.UPDATE_EXAMPLE_REQUEST
                };
            var newMdl = await _manager.PatchAsync(id, example.CreatePatch(mdl));
            return (Json(Models.Output.Examples.ExampleRequest.CreateModel(newMdl)));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            var mdl = await _manager.GetAsync(id);

            if ((mdl.Data != null && mdl.Data.UserId != _user.UserId) && !_user.HasPermission(Permissions.DELETE_EXAMPLE_REQUEST))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceId = id.ToString(),
                    ResourceName = id.ToString(),
                    ResourceType = typeof(Data.Models.Examples.ExampleRequest),
                    MissingPermission = Permissions.DELETE_EXAMPLE_REQUEST
                };
            await _manager.DeleteAsync(mdl);
            return (Ok());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Models.Output.Examples.ExampleRequest))]
        public async Task<IActionResult> GetAsync([FromRoute] long id)
        {
            var mdl = await _manager.GetAsync(id);

            if ((mdl.Data != null && mdl.Data.UserId != _user.UserId) && !_user.HasPermission(Permissions.LIST_EXAMPLE_REQUEST))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceId = id.ToString(),
                    ResourceName = id.ToString(),
                    ResourceType = typeof(Data.Models.Examples.ExampleRequest)
                };
            return (Json(Models.Output.Examples.ExampleRequest.CreateModel(await _manager.GetAsync(id))));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ApplyRequest([FromRoute] long id)
        {
            if (!_user.HasPermission(Permissions.APPLY_EXAMPLE_REQUEST))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    MissingPermission = Permissions.APPLY_EXAMPLE_REQUEST,
                    ResourceId = id.ToString(),
                    ResourceName = id.ToString(),
                    ResourceType = typeof(Data.Models.Examples.ExampleRequest)
                };
            await _manager.ApplyRequest(id);
            return (Ok());
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(PageResult<Models.Output.Examples.ExampleRequest>))]
        public IActionResult Get([FromQuery] ExampleRequestQuery query)
        {
            if (query == null || (query.PageOptions == null && query.SymbolId == null))
                throw new Shared.Exceptions.InvalidResource()
                {
                    ResourceName = "",
                    PropertyName = "Must specify at lease one query parameter",
                    ResourceType = typeof(Data.Models.Examples.Example)
                };
            bool flag = _user.HasPermission(Permissions.LIST_EXAMPLE_REQUEST);
            if (query.SymbolId != null)
            {
                var mdls = _manager.GetForSymbol(query.SymbolId.Value).Where(e => (e.Data != null && e.Data.UserId == _user.UserId) || flag);
                return (Json(new PageResult<Models.Output.Examples.ExampleRequest>
                {
                    HasMorePages = false,
                    Count = mdls.Count(),
                    Page = 1,
                    Data = Models.Output.Examples.ExampleRequest.CreateModels(mdls)
                }));
            }
            else
            {
                if (!_user.HasPermission(Permissions.LIST_EXAMPLE_REQUEST))
                    throw new Shared.Exceptions.InsuficientPermission()
                    {
                        ResourceName = "ExampleRequest",
                        ResourceType = typeof(Data.Models.Examples.ExampleRequest)
                    };
                var result = _manager.GetAll(query.PageOptions);
                return (Json(new PageResult<Models.Output.Examples.ExampleRequest>
                {
                    HasMorePages = result.HasMorePages,
                    Count = result.Count,
                    Page = result.Page,
                    Data = Models.Output.Examples.ExampleRequest.CreateModels(result.Data)
                }));
            }
        }
    }
}
