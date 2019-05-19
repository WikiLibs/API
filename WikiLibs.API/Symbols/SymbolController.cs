using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WikiLibs.Shared;
using WikiLibs.Shared.Attributes;
using WikiLibs.Shared.Modules;
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

        [AllowAnonymous]
        [AuthorizeApiKey(Flag = AuthorizeApiKey.Standard)]
        [HttpGet("{*path}")]
        public IActionResult GetSymbol([Required, FromRoute] string path)
        {
            var sym = _symmgr.Get(path);
            return (Json(Models.Output.Symbol.CreateModel(sym)));
        }

        [HttpPost]
        public async Task<IActionResult> PostSymbol([FromBody] Models.Input.SymbolCreate sym)
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
            return (Json(Models.Output.Symbol.CreateModel(mdl)));
        }

        [HttpPatch("{*path}")]
        public async Task<IActionResult> PatchSymbol([Required, FromRoute] string path, [FromBody] Models.Input.SymbolUpdate sym)
        {
            if (!_user.HasPermission(Permissions.UPDATE_SYMBOL))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceName = path,
                    ResourceId = path,
                    ResourceType = typeof(Data.Models.Symbols.Symbol),
                    MissingPermission = Permissions.UPDATE_SYMBOL
                };
            var mdl = await _symmgr.PatchAsync(_symmgr.Get(path).Id, sym.CreatePatch(_symmgr.Get(path)));
            return (Json(Models.Output.Symbol.CreateModel(mdl)));
        }

        [HttpDelete("{*path}")]
        public async Task<IActionResult> DeleteSymbol([Required, FromRoute] string path)
        {
            if (!_user.HasPermission(Permissions.DELETE_SYMBOL))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceName = path,
                    ResourceId = path,
                    ResourceType = typeof(Data.Models.Symbols.Symbol),
                    MissingPermission = Permissions.DELETE_SYMBOL
                };
            await _symmgr.DeleteAsync(_symmgr.Get(path));
            return (Ok());
        }
    }
}
