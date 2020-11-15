using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;
using WikiLibs.Models.Input.Symbols;
using WikiLibs.Models.Output.Symbols;
using WikiLibs.Shared;
using WikiLibs.Shared.Attributes;
using WikiLibs.Shared.Modules.Symbols;
using WikiLibs.Shared.Service;

namespace WikiLibs.API.Symbols
{
    [Route("/symbol/lib")]
    [Authorize]
    public class LibController : FileController
    {
        private readonly ISymbolManager _symmgr;
        private readonly IUser _user;

        public LibController(ISymbolManager mgr, IUser usr)
        {
            _symmgr = mgr;
            _user = usr;
        }

        /**
         * Algorthm proposition:
         * 1) API sends to React the root level symbols of a library.
         * 2) React handles on click event on each symbol sent by API.
         * 3) When a symbol is click, React asks API for the list of contained symbols.
         * 4) When the API is asked for contained symbols, it does SymbolManager.GetAsync(symid).Symbols and sends minimal information for each symbol in the list
         */

        [HttpGet("{id}/tree/root")]
        [Authorize(AuthenticationSchemes = AuthPolicy.ApiKey, Roles = AuthorizeApiKey.Standard)]
        [ProducesResponseType(200, Type = typeof(Models.Output.Symbols.LightweightSymbol))]
        public async Task<IActionResult> GetTreeRoot([FromRoute] long id)
        {
            var symbols = await _symmgr.Get(e => e.LibId == id && !e.Type.Name.Contains("member")).OrderBy(e => e.Path.Length).ToListAsync();
            var map = new Dictionary<long, bool>();
            var lst = new List<Models.Output.Symbols.LightweightSymbol>();

            foreach (var sym in symbols)
            {
                foreach (var rf in sym.Symbols)
                {
                    if (rf.RefId != null)
                        map[rf.RefId.Value] = true;
                }
                if (map.ContainsKey(sym.Id))
                    continue; //I'm a bad symbol
                lst.Add(Models.Output.Symbols.LightweightSymbol.CreateModel(sym)); //I'm a good symbol
            }
            return (Json(lst));
        }

        [HttpGet("{id}/tree/{symid}")]
        [Authorize(AuthenticationSchemes = AuthPolicy.ApiKey, Roles = AuthorizeApiKey.Standard)]
        [ProducesResponseType(200, Type = typeof(Models.Output.Symbols.LightweightSymbol))]
        public async Task<IActionResult> GetTree([FromRoute] long id, [FromRoute] long symid)
        {
            var sym = await _symmgr.GetAsync(symid);
            if (sym.LibId != id)
                throw new Shared.Exceptions.InvalidResource()
                {
                    ResourceName = "LibraryTree",
                    ResourceType = typeof(Data.Models.Symbols.Lib)
                };
            var lst = new List<Models.Output.Symbols.LightweightSymbol>();

            foreach (var rf in sym.Symbols.OrderBy(e => e.RefPath.Length))
            {
                if (rf.Ref != null)
                    lst.Add(Models.Output.Symbols.LightweightSymbol.CreateModel(rf.Ref));
            }
            return (Json(lst));
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(Lib))]
        public async Task<IActionResult> PostAsync([FromBody] LibCreate mdl)
        {
            if (!_user.HasPermission(Permissions.CREATE_LIB))
                throw new Shared.Exceptions.InsuficientPermission
                {
                    ResourceId = "0",
                    ResourceName = mdl.Name,
                    ResourceType = typeof(Data.Models.Symbols.Lib),
                    MissingPermission = Permissions.CREATE_LIB
                };
            var tmp = mdl.CreateModel();

            tmp.UserId = _user.UserId;
            var obj = await _symmgr.LibManager.PostAsync(tmp);
            return Json(Lib.CreateModel(obj));
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(200, Type = typeof(Lib))]
        public async Task<IActionResult> PatchAsync([FromRoute] long id, [FromBody] LibUpdate mdl)
        {
            var obj1 = await _symmgr.LibManager.GetAsync(id);

            if (!_user.HasPermission(Permissions.UPDATE_LIB) && _user.UserId  != obj1.UserId)
                throw new Shared.Exceptions.InsuficientPermission
                {
                    ResourceId = obj1.Id.ToString(),
                    ResourceName = mdl.DisplayName,
                    ResourceType = typeof(Data.Models.Symbols.Lib),
                    MissingPermission = Permissions.UPDATE_LIB
                };
            var tmp = mdl.CreatePatch(obj1);
            var obj = await _symmgr.LibManager.PatchAsync(id, tmp);

            return Json(Lib.CreateModel(obj));
        }

        [Authorize(AuthenticationSchemes = AuthPolicy.ApiKey, Roles = AuthorizeApiKey.Standard)]
        [HttpGet("{id}/icon")]
        [ProducesResponseType(200, Type = typeof(string))]
        public async Task<IActionResult> GetIcon([FromRoute] long id)
        {
            var mdl = await _symmgr.LibManager.GetAsync(id);
            var img = _symmgr.LibManager.GetFile(mdl);
            return (Json(await img.ToBase64()));
        }

        [HttpPut("{id}/icon")]
        [ProducesResponseType(200, Type = typeof(string))]
        //Parameter name is forced to be meaningless otherwise useless warning
        public async Task<IActionResult> PutIcon([FromRoute] long id, [FromForm, Required] FormFile seryhk)
        {
            var obj1 = await _symmgr.LibManager.GetAsync(id);

            if (!_user.HasPermission(Permissions.UPDATE_LIB) && _user.UserId != obj1.UserId)
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceName = "Icon",
                    ResourceId = id.ToString(),
                    ResourceType = typeof(Data.Models.Symbols.Lang),
                    MissingPermission = Permissions.UPDATE_LIB
                };
            await _symmgr.LibManager.PostFileAsync(obj1, ImageFileFromForm(seryhk));
            return (Ok());
        }

        [ProducesResponseType(200, Type = typeof(Models.Output.Symbols.Lib))]
        [Authorize(AuthenticationSchemes = AuthPolicy.ApiKey, Roles = AuthorizeApiKey.Standard)]
        [HttpGet("/symbol/lib/{id}")]
        public async Task<IActionResult> Get([FromQuery] long id)
        {
            var sym = await _symmgr.LibManager.GetAsync(id);
            return (Json(Models.Output.Symbols.Lib.CreateModel(sym)));
        }
    }
}
