using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.Models.Input.Symbols;
using WikiLibs.Shared;
using WikiLibs.Shared.Attributes;
using WikiLibs.Shared.Modules.Symbols;
using WikiLibs.Shared.Service;

namespace WikiLibs.API.Symbols
{
    [Route("/symbol/type")]
    public class TypeController : Controller
    {
        private readonly ISymbolManager _symmgr;
        private readonly IUser _user;

        public TypeController(ISymbolManager mgr, IUser usr)
        {
            _symmgr = mgr;
            _user = usr;
        }

        [HttpGet]
        [Authorize(Policy = AuthPolicy.ApiKey, Roles = AuthorizeApiKey.Standard)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Models.Output.Symbols.Type>))]
        public IActionResult AllTypes()
        {
            return (Json(Models.Output.Symbols.Type.CreateModels(_symmgr.TypeManager.Get())));
        }

        [HttpPost]
        [Authorize(Policy = AuthPolicy.Bearer)]
        [ProducesResponseType(200, Type = typeof(Models.Output.Symbols.Type))]
        public async Task<IActionResult> PostAsync([FromBody, Required]TypeCreate type)
        {
            if (!_user.HasPermission(Permissions.CREATE_SYMBOL_TYPE))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceName = type.Name,
                    ResourceId = type.Name,
                    ResourceType = typeof(Data.Models.Symbols.Type),
                    MissingPermission = Permissions.CREATE_SYMBOL_TYPE
                };
            var mdl = await _symmgr.TypeManager.PostAsync(type.CreateModel());
            return (Json(Models.Output.Symbols.Type.CreateModel(mdl)));
        }

        [HttpPatch("{id}")]
        [Authorize(Policy = AuthPolicy.Bearer)]
        [ProducesResponseType(200, Type = typeof(Models.Output.Symbols.Type))]
        public async Task<IActionResult> PatchAsync([FromRoute]long id, [FromBody, Required]TypeUpdate type)
        {
            if (!_user.HasPermission(Permissions.UPDATE_SYMBOL_TYPE))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceName = id.ToString(),
                    ResourceId = id.ToString(),
                    ResourceType = typeof(Data.Models.Symbols.Type),
                    MissingPermission = Permissions.UPDATE_SYMBOL_TYPE
                };
            var mdl = await _symmgr.TypeManager.PatchAsync(id, type.CreatePatch(await _symmgr.TypeManager.GetAsync(id)));
            return (Json(Models.Output.Symbols.Type.CreateModel(mdl)));
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = AuthPolicy.Bearer)]
        public async Task<IActionResult> DeleteAsync([FromRoute]long id)
        {
            if (!_user.HasPermission(Permissions.DELETE_SYMBOL_TYPE))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceName = id.ToString(),
                    ResourceId = id.ToString(),
                    ResourceType = typeof(Data.Models.Symbols.Type),
                    MissingPermission = Permissions.DELETE_SYMBOL_TYPE
                };
            await _symmgr.TypeManager.DeleteAsync(id);
            return (Ok());
        }
    }
}
