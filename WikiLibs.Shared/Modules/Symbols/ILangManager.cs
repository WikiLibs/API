using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Shared.Helpers;

namespace WikiLibs.Shared.Modules.Symbols
{
    public interface ILangManager : ICRUDOperations<Data.Models.Symbols.Lang>
    {
        PageResult<LangListItem> GetFirstLangs(PageOptions options);
        PageResult<LibListItem> GetFirstLibs(long lang, PageOptions options);
    }
}
