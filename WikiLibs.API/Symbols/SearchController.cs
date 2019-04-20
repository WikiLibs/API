using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Shared.Attributes;
using WikiLibs.Shared.Modules;
using WikiLibs.Shared.Service;

namespace WikiLibs.API.Symbols
{
    [Route("/search/")]
    public class SearchController : Controller
    {
        private ISymbolManager _symmgr;

        public SearchController(IModuleManager mdmgr)
        {
            _symmgr = mdmgr.GetModule<ISymbolManager>();
        }

        [AuthorizeApiKey(Flag = AuthorizeApiKey.Standard)]
        [HttpGet("lang")]
        public IActionResult AllLangs()
        {
            return (Json(_symmgr.GetFirstLangs()));
        }

        [AuthorizeApiKey(Flag = AuthorizeApiKey.Standard)]
        [HttpGet("lang/{*name}")]
        public IActionResult AllLibs(string name)
        {
            return (Json(_symmgr.GetFirstLibs(name)));
        }

        [AuthorizeApiKey(Flag = AuthorizeApiKey.Standard)]
        [HttpGet("string/{page}/{*path}")]
        public IActionResult SearchSymbols(int page, string path)
        {
            var res = _symmgr.SearchSymbols(path, new Shared.Helpers.PageOptions()
            {
                PageNum = page,
                PageSize = 0
            });

            return (Json(res));
        }
    }
}
