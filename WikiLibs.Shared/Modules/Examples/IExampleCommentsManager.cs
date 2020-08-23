using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikiLibs.Data.Models.Examples;
using WikiLibs.Shared.Helpers;

namespace WikiLibs.Shared.Modules.Examples
{
    public interface IExampleCommentsManager : ICRUDOperations<ExampleComment>
    {
        PageResult<ExampleComment> GetByExample(long example, PageOptions options);
    }
}
