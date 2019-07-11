using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.Shared;
using WikiLibs.Shared.Attributes;
using WikiLibs.Shared.Modules;
using WikiLibs.Shared.Modules.Examples;
using WikiLibs.Shared.Service;

namespace WikiLibs.API.Examples
{
    [Route("example")]
    [Authorize]
    public class ExampleController : Controller
    {
        private readonly IUser _user;
        private readonly IExampleManager _manager;
        private readonly ISymbolManager _symbolManager;

        public ExampleController(IUser usr, IExampleModule module, ISymbolManager symbolManager)
        {
            _user = usr;
            _manager = module.Manager;
            _symbolManager = symbolManager;
        }

        public class ExampleQuery
        {
            public long? SymbolId { get; set; }
            public string Token { get; set; }
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(Models.Output.Examples.Example))]
        public async Task<IActionResult> PostAsync([FromBody, Required] Models.Input.Examples.ExampleCreate example)
        {
            if (!_user.HasPermission(Permissions.CREATE_EXAMPLE))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceName = example.Description,
                    ResourceType = typeof(Data.Models.Examples.Example),
                    MissingPermission = Permissions.CREATE_EXAMPLE
                };
            var ex = example.CreateModel();
            ex.User = _user.User;
            ex.UserId = _user.UserId;
            ex.Symbol = await _symbolManager.GetAsync(ex.SymbolId);
            var mdl = await _manager.PostAsync(ex);
            return (Json(Models.Output.Examples.Example.CreateModel(mdl)));
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(200, Type = typeof(Models.Output.Examples.Example))]
        public async Task<IActionResult> PatchAsync([FromRoute] long id, [FromBody, Required] Models.Input.Examples.ExampleUpdate example)
        {
            if (!_user.HasPermission(Permissions.UPDATE_EXAMPLE))
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
                    MissingPermission = Permissions.DELETE_EXAMPLE
                };
            await _manager.DeleteAsync(id);
            return (Ok());
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        [AuthorizeApiKey(Flag = AuthorizeApiKey.Standard)]
        [ProducesResponseType(200, Type = typeof(Models.Output.Examples.Example))]
        public async Task<IActionResult> GetAsync([FromRoute] long id)
        {
            return (Json(Models.Output.Examples.Example.CreateModel(await _manager.GetAsync(id))));
        }

        [AllowAnonymous]
        [HttpGet]
        [AuthorizeApiKey(Flag = AuthorizeApiKey.Standard)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Models.Output.Examples.Example>))]
        public IActionResult Get([FromQuery] ExampleQuery query)
        {
            if (query == null || (query.Token == null && query.SymbolId == null))
                throw new Shared.Exceptions.InvalidResource()
                {
                    ResourceName = "",
                    PropertyName = "Must specify at lease one query parameter",
                    ResourceType = typeof(Data.Models.Examples.Example)
                };
            if (query.SymbolId != null)
                return (Json(Models.Output.Examples.Example.CreateModels(_manager.GetForSymbol(query.SymbolId.Value))));
            else
                return (Json(Models.Output.Examples.Example.CreateModels(_manager.Get(e => e.Description.Contains(query.Token)))));
        }
    }
}
