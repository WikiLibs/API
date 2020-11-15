using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WikiLibs.Shared;
using WikiLibs.Shared.Attributes;
using WikiLibs.Shared.Helpers;
using WikiLibs.Shared.Modules.Symbols;
using WikiLibs.Shared.Service;

namespace WikiLibs.API.Symbols
{
    [Authorize]
    [Route("/symbol")]
    public class SymbolController : Controller
    {
        private readonly ISymbolManager _symmgr;
        private readonly IUser _user;

        public SymbolController(ISymbolManager mgr, IUser usr)
        {
            _symmgr = mgr;
            _user = usr;
        }

        public class SymbolQuery
        {
            public long? Id { get; set; }
            public string Path { get; set; }
        }

        [ProducesResponseType(200, Type = typeof(Models.Output.Symbols.Symbol))]
        [Authorize(AuthenticationSchemes = AuthPolicy.ApiKey, Roles = AuthorizeApiKey.Standard)]
        [HttpGet]
        public async Task<IActionResult> GetSymbol([FromQuery] SymbolQuery query)
        {
            if (query == null || (query.Id == null && query.Path == null))
                throw new Shared.Exceptions.InvalidResource()
                {
                    ResourceName = "",
                    PropertyName = "Must specify at lease one query parameter",
                    ResourceType = typeof(Data.Models.Symbols.Symbol)
                };
            if (query.Path != null)
            {
                var sym = await _symmgr.GetAsync(query.Path);
                return (Json(Models.Output.Symbols.Symbol.CreateModel(sym)));
            }
            else
            {
                var sym = await _symmgr.GetAsync(query.Id.Value);
                return (Json(Models.Output.Symbols.Symbol.CreateModel(sym)));
            }
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(Models.Output.Symbols.Symbol))]
        public async Task<IActionResult> PostSymbol([FromBody, Required] Models.Input.Symbols.SymbolCreate sym)
        {
            if (!_user.HasPermission(Permissions.CREATE_SYMBOL))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceName = sym.Path,
                    ResourceId = sym.Path,
                    ResourceType = typeof(Data.Models.Symbols.Symbol),
                    MissingPermission = Permissions.CREATE_SYMBOL
                };
            var data = sym.CreateModel();
            data.User = _user.User;
            var mdl = await _symmgr.PostAsync(data);
            return (Json(Models.Output.Symbols.Symbol.CreateModel(mdl)));
        }

        [HttpPut("{*path}")]
        [ProducesResponseType(200, Type = typeof(Models.Output.Symbols.Symbol))]
        public async Task<IActionResult> PutSymbol([FromRoute] string path, [FromBody, Required] Models.Input.Symbols.SymbolMerge sym)
        {
            if (!_user.HasPermission(Permissions.UPDATE_SYMBOL) || !_user.HasPermission(Permissions.CREATE_SYMBOL))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceName = path,
                    ResourceId = path,
                    ResourceType = typeof(Data.Models.Symbols.Symbol),
                    MissingPermission = Permissions.UPDATE_SYMBOL
                };
            var mdl = _symmgr.Get((e) => e.Path == path).FirstOrDefault();
            Data.Models.Symbols.Symbol fasoi = null;
            if (mdl != null)
                fasoi = await _symmgr.PatchAsync(mdl.Id, sym.CreatePatch(mdl));
            else
            {
                var tmp = sym.CreateModel(path);
                tmp.User = _user.User;
                fasoi = await _symmgr.PostAsync(tmp);
            }
            return (Json(Models.Output.Symbols.Symbol.CreateModel(fasoi)));
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(200, Type = typeof(Models.Output.Symbols.Symbol))]
        public async Task<IActionResult> PatchSymbol([FromRoute] long id, [FromBody, Required] Models.Input.Symbols.SymbolUpdate sym)
        {
            if (!_user.HasPermission(Permissions.UPDATE_SYMBOL))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceName = id.ToString(),
                    ResourceId = id.ToString(),
                    ResourceType = typeof(Data.Models.Symbols.Symbol),
                    MissingPermission = Permissions.UPDATE_SYMBOL
                };
            var mdl = await _symmgr.PatchAsync(id, sym.CreatePatch(await _symmgr.GetAsync(id)));
            return (Json(Models.Output.Symbols.Symbol.CreateModel(mdl)));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSymbol([FromRoute] long id)
        {
            if (!_user.HasPermission(Permissions.DELETE_SYMBOL))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceName = id.ToString(),
                    ResourceId = id.ToString(),
                    ResourceType = typeof(Data.Models.Symbols.Symbol),
                    MissingPermission = Permissions.DELETE_SYMBOL
                };
            await _symmgr.DeleteAsync(id);
            return (Ok());
        }

        [HttpPatch("optimize")]
        public async Task<IActionResult> OptimizeAsync()
        {
            if (!_user.HasPermission(Permissions.OPTIMIZE_SYMBOL))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceName = "Optimize",
                    ResourceId = "Optimize",
                    ResourceType = typeof(Data.Models.Symbols.Symbol),
                    MissingPermission = Permissions.OPTIMIZE_SYMBOL
                };
            await _symmgr.OptimizeAsync();
            return (Ok());
        }

        [Authorize(AuthenticationSchemes = AuthPolicy.ApiKey, Roles = AuthorizeApiKey.Standard)]
        [HttpGet("search")]
        [ProducesResponseType(200, Type = typeof(PageResult<SymbolListItem>))]
        public IActionResult SearchSymbols([FromQuery]SearchQuery options)
        {
            return (Json(_symmgr.SearchSymbols(options)));
        }
    }
}
