using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace SymbolManager.Controllers
{
    public class SearchController : Controller
    {
        private API.Modules.ISymbolManager _symmgr;

        public SearchController(API.IModuleManager mdmgr)
        {
            _symmgr = mdmgr.GetModule<API.Modules.ISymbolManager>();
        }

        [API.AuthorizeApiKey]
        [HttpGet]
        [Route("/search/lang")]
        public IActionResult AllLangs()
        {
            return (Json(_symmgr.GetFirstLangs()));
        }

        [API.AuthorizeApiKey]
        [HttpGet]
        [Route("/search/lang/{*name}")]
        public IActionResult AllLibs(string name)
        {
            return (Json(_symmgr.GetFirstLibs(name)));
        }

        [API.AuthorizeApiKey]
        [HttpGet]
        [Route("/search/string/{*path}")]
        public IActionResult SearchSymbols(string path)
        {
            return (Json(_symmgr.SearchSymbols(path)));
        }
    }
}
