using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.Models.Input.Symbols;
using WikiLibs.Shared;
using WikiLibs.Shared.Attributes;
using WikiLibs.Shared.Helpers;
using WikiLibs.Shared.Modules.Symbols;
using WikiLibs.Shared.Service;

namespace WikiLibs.API.Symbols
{
    [Authorize]
    [Route("/symbol/lang")]
    public class LangController : FileController
    {
        private readonly ISymbolManager _symmgr;
        private readonly IUser _user;

        public LangController(ISymbolManager mgr, IUser usr)
        {
            _symmgr = mgr;
            _user = usr;
        }

        [AllowAnonymous]
        [AuthorizeApiKey(Flag = AuthorizeApiKey.Standard)]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Models.Output.Symbols.Lang>))]
        public IActionResult AllLangs()
        {
            return (Json(Models.Output.Symbols.Lang.CreateModels(_symmgr.LangManager.GetAllLangs())));
        }

        [AllowAnonymous]
        [AuthorizeApiKey(Flag = AuthorizeApiKey.Standard)]
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(PageResult<LibListItem>))]
        public IActionResult AllLibs([FromRoute]long id, [FromQuery]PageOptions options)
        {
            return (Json(_symmgr.LangManager.GetFirstLibs(id, options)));
        }

        [AllowAnonymous]
        [AuthorizeApiKey(Flag = AuthorizeApiKey.Standard)]
        [HttpGet("{id}/icon")]
        [ProducesResponseType(200, Type = typeof(string))]
        public async Task<IActionResult> GetIcon([FromRoute]long id)
        {
            var mdl = await _symmgr.LangManager.GetAsync(id);
            var img = _symmgr.LangManager.GetFile(mdl);
            return (Json(await img.ToBase64()));
        }

        [HttpPost("{id}/icon")]
        [ProducesResponseType(200, Type = typeof(string))]
        public async Task<IActionResult> PostIcon([FromRoute]long id, [FromForm, Required]FormFile file)
        {
            var mdl = await _symmgr.LangManager.GetAsync(id);
            await _symmgr.LangManager.PostFileAsync(mdl, ImageFileFromForm(file));
            return (Ok());
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(Models.Output.Symbols.Lang))]
        public async Task<IActionResult> PostLang([FromBody, Required]LangCreate lang)
        {
            if (!_user.HasPermission(Permissions.CREATE_SYMBOL_LANG))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceName = lang.Name,
                    ResourceId = lang.Name,
                    ResourceType = typeof(Data.Models.Symbols.Lang),
                    MissingPermission = Permissions.CREATE_SYMBOL_LANG
                };
            var mdl = await _symmgr.LangManager.PostAsync(lang.CreateModel());
            return (Json(Models.Output.Symbols.Lang.CreateModel(mdl)));
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(200, Type = typeof(Models.Output.Symbols.Lang))]
        public async Task<IActionResult> PatchLang([FromRoute]long id, [FromBody, Required]LangUpdate lang)
        {
            if (!_user.HasPermission(Permissions.UPDATE_SYMBOL_LANG))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceName = id.ToString(),
                    ResourceId = id.ToString(),
                    ResourceType = typeof(Data.Models.Symbols.Lang),
                    MissingPermission = Permissions.UPDATE_SYMBOL_LANG
                };
            var mdl = await _symmgr.LangManager.PatchAsync(id, lang.CreatePatch(await _symmgr.LangManager.GetAsync(id)));
            return (Json(Models.Output.Symbols.Lang.CreateModel(mdl)));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLang([FromRoute]long id)
        {
            if (!_user.HasPermission(Permissions.DELETE_SYMBOL_LANG))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceName = id.ToString(),
                    ResourceId = id.ToString(),
                    ResourceType = typeof(Data.Models.Symbols.Lang),
                    MissingPermission = Permissions.DELETE_SYMBOL_LANG
                };
            await _symmgr.LangManager.DeleteAsync(id);
            return (Ok());
        }
    }
}
