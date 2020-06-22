using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikiLibs.Shared.Attributes;
using WikiLibs.Shared.Modules.Symbols;
using WikiLibs.Shared.Service;

namespace WikiLibs.API.Symbols
{
    [Route("/symbol/lib")]
    public class LibController : Controller
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
        [AuthorizeApiKey(Flag = AuthorizeApiKey.Standard)]
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
                        map.Add(rf.RefId.Value, true);
                }
                if (map.ContainsKey(sym.Id))
                    continue; //I'm a bad symbol
                lst.Add(Models.Output.Symbols.LightweightSymbol.CreateModel(sym)); //I'm a good symbol
            }
            return (Json(lst));
        }

        [HttpGet("{id}/tree/{symid}")]
        [AuthorizeApiKey(Flag = AuthorizeApiKey.Standard)]
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
    }
}
