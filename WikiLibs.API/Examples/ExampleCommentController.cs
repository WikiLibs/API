using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.Shared;
using WikiLibs.Shared.Attributes;
using WikiLibs.Shared.Helpers;
using WikiLibs.Shared.Modules.Examples;
using WikiLibs.Shared.Service;

namespace WikiLibs.API.Examples
{
    [Route("example/comment")]
    [Authorize]
    public class ExampleCommentController : Controller
    {
        private readonly IUser _user;
        private readonly IExampleCommentsManager _manager;
        private readonly IExampleManager _emanager;

        public ExampleCommentController(IUser usr, IExampleModule module)
        {
            _user = usr;
            _manager = module.CommentsManager;
            _emanager = module.Manager;
        }

        public class ExampleCommentQuery
        {
            public long ExampleId { get; set; }
            public PageOptions PageOptions { get; set; }
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(Models.Output.Examples.Example))]
        public async Task<IActionResult> PostAsync([FromBody, Required] Models.Input.Examples.ExampleCommentCreate example)
        {
            if (!_user.HasPermission(Permissions.CREATE_EXAMPLE_COMMENT))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceName = "ExampleComment",
                    ResourceType = typeof(Data.Models.Examples.ExampleComment),
                    MissingPermission = Permissions.CREATE_EXAMPLE
                };
            var ex = example.CreateModel();
            ex.User = _user.User;
            ex.UserId = _user.UserId;
            var mdl = await _manager.PostAsync(ex);
            return (Json(Models.Output.Examples.ExampleComment.CreateModel(mdl)));
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(200, Type = typeof(Models.Output.Examples.Example))]
        public async Task<IActionResult> PatchAsync([FromRoute] long id, [FromBody, Required] Models.Input.Examples.ExampleCommentUpdate example)
        {
            var mdl = await _manager.GetAsync(id);
            if (mdl.UserId != _user.UserId && !_user.HasPermission(Permissions.UPDATE_EXAMPLE))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceId = id.ToString(),
                    ResourceName = "ExampleComment",
                    ResourceType = typeof(Data.Models.Examples.ExampleComment),
                    MissingPermission = Permissions.UPDATE_EXAMPLE
                };
            var m = await _manager.PatchAsync(id, example.CreatePatch(mdl));
            return (Json(Models.Output.Examples.ExampleComment.CreateModel(m)));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            if (!_user.HasPermission(Permissions.DELETE_EXAMPLE))
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceId = id.ToString(),
                    ResourceName = id.ToString(),
                    ResourceType = typeof(Data.Models.Examples.ExampleComment),
                    MissingPermission = Permissions.DELETE_EXAMPLE
                };
            await _manager.DeleteAsync(id);
            return (Ok());
        }

        [AllowAnonymous]
        [HttpGet]
        [AuthorizeApiKey(Flag = AuthorizeApiKey.Standard)]
        [ProducesResponseType(200, Type = typeof(PageResult<Models.Output.Examples.ExampleComment>))]
        public IActionResult Get([FromQuery] ExampleCommentQuery query)
        {
            if (query == null)
                throw new Shared.Exceptions.InvalidResource()
                {
                    ResourceName = "",
                    PropertyName = "Must specify at lease one query parameter",
                    ResourceType = typeof(Data.Models.Examples.Example)
                };
            var result = _manager.GetByExample(query.ExampleId, query.PageOptions);
            return (Json(new PageResult<Models.Output.Examples.ExampleComment>
            {
                HasMorePages = result.HasMorePages,
                Count = result.Count,
                Page = result.Page,
                Data = Models.Output.Examples.ExampleComment.CreateModels(result.Data)
            }));
        }
    }
}
