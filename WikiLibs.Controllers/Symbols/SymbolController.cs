﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using WikiLibs.API;

namespace WikiLibs.Controllers.Symbols
{
    [Authorize]
    [Route("/symbol/")]
    public class SymbolController : Controller
    {
        private API.Modules.ISymbolManager _symmgr;
        private IUser _user;

        public SymbolController(IModuleManager mgr, IUser usr)
        {
            _symmgr = mgr.GetModule<API.Modules.ISymbolManager>();
            _user = usr;
        }

        [AllowAnonymous]
        [AuthorizeApiKey(Flag = AuthorizeApiKey.Standard)]
        [HttpGet("{*path}")]
        public IActionResult GetSymbol([Required] string path)
        {
            var sym = _symmgr.Get(path);
            return (Json(Models.Output.Symbol.CreateModel(sym)));
        }

        [HttpPost]
        public IActionResult PostSymbol([FromBody] Models.Input.SymbolCreate sym)
        {
            if (!_user.HasPermission(API.Permissions.CREATE_SYMBOL))
                throw new API.Exceptions.InsuficientPermission()
                {
                    ResourceName = sym.Path,
                    ResourceId = sym.Path,
                    ResourceType = typeof(Data.Models.Symbol),
                    MissingPermission = Permissions.CREATE_SYMBOL
                };
            _symmgr.Post(sym.CreateModel());
            return (Ok());
        }

        [HttpPatch("{*path}")]
        public IActionResult PatchSymbol([Required] string path, [FromBody] Models.Input.SymbolUpdate sym)
        {
            if (!_user.HasPermission(API.Permissions.UPDATE_SYMBOL))
                throw new API.Exceptions.InsuficientPermission()
                {
                    ResourceName = path,
                    ResourceId = path,
                    ResourceType = typeof(Data.Models.Symbol),
                    MissingPermission = Permissions.UPDATE_SYMBOL
                };
            _symmgr.Patch(sym.CreatePatch(_symmgr.Get(path)));
            return (Ok());
        }

        [HttpDelete("{*path}")]
        public IActionResult DeleteSymbol([Required] string path)
        {
            if (!_user.HasPermission(Permissions.DELETE_SYMBOL))
                throw new API.Exceptions.InsuficientPermission()
                {
                    ResourceName = path,
                    ResourceId = path,
                    ResourceType = typeof(Data.Models.Symbol),
                    MissingPermission = Permissions.DELETE_SYMBOL
                };
            _symmgr.Delete(path);
            return (Ok());
        }
    }
}
