using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Models.Input.Symbols;
using WikiLibs.Shared.Attributes;
using WikiLibs.Shared.Helpers;
using WikiLibs.Shared.Modules.Symbols;
using WikiLibs.Shared.Service;

namespace WikiLibs.API.Symbols
{
    [Route("/symbol")]
    public class SearchController : Controller
    {
        private readonly ISymbolManager _symmgr;

        public SearchController(ISymbolManager mgr)
        {
            _symmgr = mgr;
        }

        [AuthorizeApiKey(Flag = AuthorizeApiKey.Standard)]
        [HttpGet("lib/{id}")]
        [ProducesResponseType(200, Type = typeof(PageResult<SymbolListItem>))]
        public IActionResult GetSymbolsForLib([FromRoute]long id, [FromQuery]PageOptions options)
        {
            return (Json(_symmgr.GetSymbolsForLib(id, options)));
        }

        [AuthorizeApiKey(Flag = AuthorizeApiKey.Standard)]
        [HttpGet("search")]
        [ProducesResponseType(200, Type = typeof(PageResult<SymbolListItem>))]
        public IActionResult SearchSymbols([FromQuery]SearchQuery options)
        {
            return (Json(_symmgr.SearchSymbols(options)));
        }
    }
}
