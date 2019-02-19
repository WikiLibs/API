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

        class SearchResult
        {
            public bool next { get; set; }
            public string[] results { get; set; }
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
        [Route("/search/string/{page}/{*path}")]
        public IActionResult SearchSymbols(int page, string path)
        {
            var res = _symmgr.SearchSymbols(page, path);
            var json = new SearchResult
            {
                next = res.HasNext,
                results = res.Symbols
            };

            return (Json(json));
        }
    }
}
