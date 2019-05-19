using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Shared.Attributes;
using WikiLibs.Shared.Modules;
using WikiLibs.Shared.Service;

namespace WikiLibs.API.Symbols
{
    [Route("/search")]
    public class SearchController : Controller
    {
        private readonly ISymbolManager _symmgr;

        public SearchController(ISymbolManager mgr)
        {
            _symmgr = mgr;
        }

        [AuthorizeApiKey(Flag = AuthorizeApiKey.Standard)]
        [HttpGet("lang")]
        public IActionResult AllLangs()
        {
            return (Json(_symmgr.GetFirstLangs()));
        }

        [AuthorizeApiKey(Flag = AuthorizeApiKey.Standard)]
        [HttpGet("lang/{*name}")]
        public IActionResult AllLibs([FromRoute]string name)
        {
            return (Json(_symmgr.GetFirstLibs(name)));
        }

        [AuthorizeApiKey(Flag = AuthorizeApiKey.Standard)]
        [HttpGet("string/{*path}")]
        public IActionResult SearchSymbols([FromRoute]string path, [FromQuery]Shared.Helpers.PageOptions options)
        {
            var res = _symmgr.SearchSymbols(path, options);
            return (Json(res));
        }
    }
}
