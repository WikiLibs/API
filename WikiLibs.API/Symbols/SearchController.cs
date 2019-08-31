using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
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
        public IActionResult SymbolsFromLib([FromRoute]long id, [FromQuery]PageOptions options)
        {
            return (Json(_symmgr.SymbolsForLib(id, options)));
        }

        [AuthorizeApiKey(Flag = AuthorizeApiKey.Standard)]
        [HttpGet("search/{*path}")]
        [ProducesResponseType(200, Type = typeof(PageResult<SymbolListItem>))]
        public IActionResult SearchSymbols([FromRoute]string path, [FromQuery]PageOptions options)
        {
            return (Json(_symmgr.SearchSymbols(path, options)));
        }
    }
}
