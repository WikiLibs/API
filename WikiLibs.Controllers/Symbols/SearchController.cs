using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Controllers.Symbols
{
    [Route("/search/")]
    public class SearchController : Controller
    {
        private API.Modules.ISymbolManager _symmgr;

        public SearchController(API.IModuleManager mdmgr)
        {
            _symmgr = mdmgr.GetModule<API.Modules.ISymbolManager>();
        }

        [API.AuthorizeApiKey]
        [HttpGet("lang")]
        public IActionResult AllLangs()
        {
            return (Json(_symmgr.GetFirstLangs()));
        }

        [API.AuthorizeApiKey]
        [HttpGet("lang/{*name}")]
        public IActionResult AllLibs(string name)
        {
            return (Json(_symmgr.GetFirstLibs(name)));
        }

        [API.AuthorizeApiKey]
        [HttpGet("string/{page}/{*path}")]
        public IActionResult SearchSymbols(int page, string path)
        {
            var res = _symmgr.SearchSymbols(page, path);

            return (Json(res));
        }
    }
}
